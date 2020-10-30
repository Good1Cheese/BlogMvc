using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TestBlog2.Models;
using TestBlog2.ViewModels;
using AppContext = TestBlog2.Data.AppContext;


namespace TestBlog2.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppContext db;
        public HomeController(AppContext context)
        {
            db = context;
        }

        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Post = db.Posts.OrderByDescending(p => p.Id).Include(c => c.Category).Include(u => u.User).Include(comm => comm.Comments).Take(5).ToList();
            return View();
        }

        [Route("single/{id}")]
        public IActionResult Single(int id)
        {
            ViewBag.Post = db.Posts.Where(p => p.Id == id).Include(c => c.Category).Include(u => u.User).Include(comm => comm.Comments).ThenInclude(commus => commus.User).ToList();
            return View(new Comment { PostId = id });
        }

        [HttpPost("Create")]
        public IActionResult Create(Comment CommentData)
        {
            var user = db.Users.FirstOrDefault(u => u.Name == User.Identity.Name);
            db.Comments.Add(new Comment { Message = CommentData.Message, PostId = CommentData.PostId, UserId = user.Id });
            db.SaveChanges();
            return RedirectToAction("single", "home", new { id = CommentData.PostId });
        }

        [Route("category/{id}/{page?}")]
        public async Task<IActionResult> CategorySingleAsync(int id, int page = 1)
        {
            int pageSize = 4;
            IQueryable<Post> source = db.Posts.Where(p => p.CategoryId == id).Include(c => c.Category).Include(u => u.User).Include(comm => comm.Comments);
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(id, count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Posts = items
            };
            return View(viewModel);
        }

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

        [Route("game")]
        public IActionResult Game()
        {
            return View();
        }

        [Route("reviews")]
        public IActionResult Reviews()
        {
            return View();
        }

        [Route("characters")]
        public IActionResult Characters()
        {
            return View();
        }

    }
}
