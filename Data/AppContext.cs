using Microsoft.EntityFrameworkCore;
using BlogMvc.Models;

namespace BlogMvc.Data
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // добавялем роли
            modelBuilder.Entity<Role>().HasData(
                new Role[] {
                    new Role { Id = 1, Name = "admin" },
                    new Role { Id = 2, Name = "user" }
                });

            // добавялем юзеров
            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                    new User { Id = 1,Name="Dr.Bright", Image="Test img", Email="test@gmail.com", Password = "123"},
                    new User { Id = 2,Name="Maks", Image="Test imgAv", Email="lox.com", Password = "123"},
                    new User { Id = 3, Name="Pasha", Email = "admin@mail.ru", Password = "123456", RoleId = 1 }
                });

            // добавялем категории
            modelBuilder.Entity<Category>().HasData(
                new Category[]
                {
                    new Category { Id=1, Title="Games" },
                    new Category { Id=2, Title="Coding"},
                    new Category { Id=3, Title="Maks lox"}
                });

            // добавялем посты
            modelBuilder.Entity<Post>().HasData(
                new Post[]
                {
                    new Post { Id=1, Title="The best Games 2019", Text = "Lorem Ipsum", Image="Spaciba", UserId=1, CategoryId=1},
                    new Post { Id=2, Title="Assasins creed Review", Text = "Lorem Ipsum", Image="Aga", UserId=1, CategoryId=2},
                    new Post { Id=3, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=4, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=5, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=6, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=7, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=8, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=9, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=10, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=11, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=12, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=13, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=14, Title="Serios Sam. What is this?", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3},
                    new Post { Id=15, Title="Test name", Text = "Lorem Ipsum", Image="Idi naxoy", UserId=1, CategoryId=3}
                });

            // добавялем комментарии
            modelBuilder.Entity<Comment>().HasData(
                new Comment[]
                {
                    new Comment { Id=1, Message="This gamer sucks", UserId=2, PostId=1},
                    new Comment { Id=2, Message="Maksa v rot ebal", UserId=2, PostId=1},
                    new Comment { Id=3, Message="Sam is top", UserId=2, PostId=2}
                });
        }
    }
}
