using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EFRelatedData
{
    public class Sample
    {
        public static void Run()
        {
            using (var context = new BloggingContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            #region AddingGraphOfEntities
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
            #endregion
        
            #region Come aggiornare solo le properties passate nella request
            
            // Request che arriva dal client tramite API
            var request = new BlogDto()
            {
                Url = "Nuovo Blog del cacchio",
                Ranking = 9
            };

            // Partial Update
            using (var context = new BloggingContext())
            {
                // Entità da aggiornare
                var blog = context.Blogs.Single(b => b.BlogId == 1);

                // Copio solo le proprietà presenti nella request che hanno stesso nome e tipo
                blog.CopyPropertiesFrom(request);

                // Update
                context.Blogs.Update(blog);
                context.SaveChanges();
            }
            #endregion

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