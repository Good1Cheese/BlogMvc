using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppContext = TestBlog2.Data.AppContext;

namespace TestBlog2.ViewComponents
{
    public class Sidebar : ViewComponent
    {
        private readonly AppContext db;
        public Sidebar(AppContext context)
        {
            db = context;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.Categories = db.Categories.Take(6);
            var latestPosts = db.Posts.Include(c => c.Category).Include(u => u.User).Include(comm => comm.Comments).ThenInclude(commus => commus.User).OrderByDescending(p => p.Id).Take(5).ToList();
            return View(latestPosts);
        }
       
    }
}
