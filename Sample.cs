using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EFRelatedData
{
    public class Sample
    {
        public static void Example1(Dictionary<string, dynamic> request)
        {
            SetupDatabase();

            // Partial Update (nome e tipo proprietà devono matchare)
            using (var context = new BloggingContext())
            {
                // Entità da aggiornare
                var blog = context.Blogs.Single(b => b.BlogId == 1);
                
                // Copio solo le proprietà presenti nella request che hanno stesso nome e tipo
                blog = PropertiesCopier.CopyPropertiesFrom(blog, request);

                // Update
                context.Blogs.Update(blog);
                context.SaveChanges();
            }
        }

        public static void Example2(int? entityId)
        {
            //SetupDatabase();

            using (var context = new BloggingContext())
            {
                Blog blog;
                if (entityId.HasValue)
                {
                    blog = new Blog()
                    {
                        BlogId = entityId.Value,
                        InsertDate = DateTime.Now,
                        Note = "Blog N. 222",
                        Ranking = 222,
                        Url = "http://www.blog2222.com"
                    };
                }
                else
                {
                    blog = new Blog()
                    {
                        InsertDate = DateTime.Now,
                        Note = "NEW",
                        Ranking = 3,
                        Url = "http://www.newblog.com",
                        Posts = new List<Post>
                        {
                            new Post {Title = "Advanced C# programming"},
                            new Post {Title = "Advanced VB.NET programming"}
                        }
                    };
                }

                context.Blogs.AddOrUpdate<Blog>(b => b.BlogId, blog);
                context.SaveChanges();
            }
        }

        private static void SetupDatabase()
        {
            using (var context = new BloggingContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            using (var context = new BloggingContext())
            {
                var blog = new Blog
                {
                    Url = "http://blogs.msdn.com/dotnet",
                    Note = "Note varie",
                    InsertDate = DateTime.Now,
                    Ranking = 1,
                    Posts = new List<Post>
                    {
                        new Post {Title = "Intro to C#"},
                        new Post {Title = "Intro to VB.NET"},
                        new Post {Title = "Intro to F#"}
                    }
                };

                context.Blogs.Add(blog);
                context.SaveChanges();
            }
        }

        public static Blog GetBlogById(int id)
        {
            using (var context = new BloggingContext())
            {
                return  context.Blogs.FirstOrDefault(b => b.BlogId == id);
            }
        }

        public static void Run()
        {
          SetupDatabase();
          
            /*
            #region AddingRelatedEntity
            using (var context = new BloggingContext())
            {
                var blog = context.Blogs.Include(b => b.Posts).First();
                var post = new Post { Title = "Intro to EF Core" };

                blog.Posts.Add(post);
                context.SaveChanges();
            }
            #endregion

            #region ChangingRelationships
            using (var context = new BloggingContext())
            {
                var blog = new Blog { Url = "http://blogs.msdn.com/visualstudio" };
                var post = context.Posts.First();

                post.Blog = blog;
                context.SaveChanges();
            }
            #endregion

            #region RemovingRelationships
            using (var context = new BloggingContext())
            {
                var blog = context.Blogs.Include(b => b.Posts).First();
                var post = blog.Posts.First();

                blog.Posts.Remove(post);
                context.SaveChanges();
            }
            #endregion
            */
        }

        public class BloggingContext : DbContext
        {
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Post> Posts { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-GSGE42P\MSSQLSERVER01;Database=EFSaving.RelatedData;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }

        public class Blog
        {
            public int BlogId { get; set; }
            public string Url { get; set; }
            public string Note { get; set; }
            public DateTime? InsertDate { get; set; }
            public int? Ranking { get; set; }

            public List<Post> Posts { get; set; }
        }

        public class Post
        {
            public int PostId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public int BlogId { get; set; }
            public Blog Blog { get; set; }
        }
    }
}