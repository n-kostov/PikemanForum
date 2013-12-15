using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Forum.ViewModels
{
    public class UsersListViewModel
    {
        public static Expression<Func<ApplicationUser, UsersListViewModel>> FromUser
        {
            get
            {
                return user => new UsersListViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName ?? "",
                    LastName = user.LastName ?? "",
                    Email = user.Email ?? "",
                    PhotoUrl = user.PhotoUrl,
                    PostCount = user.Posts.Count,
                };
            }
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhotoUrl { get; set; }

        public int PostCount { get; set; }
    }
}