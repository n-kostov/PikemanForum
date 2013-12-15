using Forum.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Forum.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Forum.Data.ForumDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Forum.Data.ForumDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Roles.AddOrUpdate(
                    r => r.Name,
                    new Role { Name = "Registered user" },
                    new Role { Name = "Administrator" }
                    );

           this.InstertDB(context);
        }

        private void InstertDB(ForumDbContext context)
        {
            if (context.Posts.Count() > 0)
            {
                return;
            }

            var user = new ApplicationUser
            { 
                UserName = "JustAnUser",
                FirstName = "UserFirstName", 
                LastName = "UserLastName",
            };

            for (int i = 1; i <= 10; i++)
            {
                var category = new Category { Name = "Category" + i };
                context.Categories.AddOrUpdate(c => c.Name, category);

                for (int j = 1; j <= 10; j++)
                {
                    var post = new Post
                    {
                        User = user,
                        Category = category,
                        Title = "Post" + i + " " + j,
                        Content = "dsajkhdsakj" + j + " " + i + " djsahdjska whj"
                    };

                    var tag = new Tag
                    {
                        Name = "tag" + j + 1 + " " + j
                    };

                    var comment = new Comment
                    {
                        User = user,
                        Post = post,
                        Content = "content " + j * j
                    };

                    tag.Posts.Add(post);
                    post.Tags.Add(tag);

                    context.Posts.AddOrUpdate(p => p.Title, post);

                    context.Comments.AddOrUpdate(c => c.Id, comment);

                    context.Tags.AddOrUpdate(t => t.Name, tag);
                }
            }
        }
    }
}
