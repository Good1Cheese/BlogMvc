using System.Collections.Generic;
using BlogMvc.Models;

namespace BlogMvc.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
