using System.Collections.Generic;
using TestBlog2.Models;

namespace TestBlog2.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
