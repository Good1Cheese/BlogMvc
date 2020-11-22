using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.Models;
using BlogMvc.ViewModels;
using AppContext = BlogMvc.Data.AppContext;
using System;
using Microsoft.AspNetCore.Authorization;

namespace BlogMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppContext db;
        public HomeController(AppContext context)
        {
            db = context;
        }

        /// <summary>
        /// Главная
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Post = db.Posts.OrderByDescending(p => p.Id).Include(c => c.Category).Include(u => u.User).Include(comm => comm.Comments).Take(6).ToList();
            return View();
        }

        /// <summary>
        /// Метод для просмотра одного поста
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("single/{id}")]
        public IActionResult Single(int id)
        {
            ViewBag.Post = db.Posts.Where(p => p.Id == id).Include(c => c.Category).Include(u => u.User).Include(comm => comm.Comments).ThenInclude(commus => commus.User).ToList();
            return View(new Comment { PostId = id });
        }

        /// <summary>
        /// Метод для создания комментариев
        /// </summary>
        /// <param name="CommentData"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public IActionResult Create(Comment CommentData)
        {
            var user = db.Users.FirstOrDefault(u => u.Name == User.Identity.Name);
            db.Comments.Add(new Comment { Message = CommentData.Message, PostId = CommentData.PostId, UserId = user.Id });
            db.SaveChanges();
            return RedirectToAction("single", "home", new { id = CommentData.PostId });
        }

        /// <summary>
        /// Метод для постов по категориям
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("category/{id}/{page?}")]
        public async Task<IActionResult> SingleCategoryAsync(int id, int page = 1)
        {
            int pageSize = 4;
            IQueryable<Post> source = db.Posts.Where(p => p.CategoryId == id).Include(c => c.Category).Include(u => u.User).Include(comm => comm.Comments);
            if(source.Count() == 0) { return NotFound("Ничего не найдено"); }
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(id, count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Posts = items
            };

            if (page > pageViewModel.TotalPages)
            {
                return Redirect($"https://localhost:44365/category/{ id }/{ pageViewModel.TotalPages }");
            }
            else
            {
                return View(viewModel);
            }

        }

        /// <summary>
        /// Метод для формы поиска
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [Route("get")]
        public async Task<IActionResult> Search(string title)
        {
            var post = await db.Posts.FirstOrDefaultAsync(p => EF.Functions.Like(p.Title, $"%{title}%"));
            if (post == null)
            {
                return NotFound("Ничего не найдено");
            }
            return RedirectPermanent("https://localhost:44365/single/" + post.Id);
        }


        /// <summary>
        /// Метод для отзывов
        /// </summary>
        [Route("reviews")]
        public IActionResult Reviews()
        {
            return View();
        }

        /// <summary>
        /// Метод для 
        /// </summary>  
        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [Route("admin-panel")]
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
