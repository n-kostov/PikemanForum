using Forum.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Data
{
    public interface IUowData : IDisposable
    {
        IRepository<Category> Categories { get; }

        IRepository<ApplicationUser> Users { get; }

        IRepository<Comment> Comments { get; }

        IRepository<Post> Posts { get; }

        IRepository<Tag> Tags { get; }

        IRepository<Role> Roles { get; }

        IRepository<UserRole> UserRoles { get; }

        int SaveChanges();
    }
}
