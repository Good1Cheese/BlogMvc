using Microsoft.AspNetCore.Identity;

namespace TestBlog2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Password { get; set;  }

    }
}
