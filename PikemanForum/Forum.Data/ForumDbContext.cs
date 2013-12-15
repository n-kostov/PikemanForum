using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Forum.Data
{
    public class ForumDbContext : IdentityDbContextWithCustomUser<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}
