using Forum.Data;
using Forum.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading;

namespace Forum.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserAdministrationController : Controller
    {
        private IUowData db = new UowData();

        public AuthenticationIdentityManager IdentityManager { get; set; }

        public UserAdministrationController()
        {
            IdentityManager = new AuthenticationIdentityManager(new IdentityStore(new ForumDbContext()));
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.Users.All().Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                }).ToDataSourceResult(request));
        }

        //[HttpPost]
        //public ActionResult Create([DataSourceRequest] DataSourceRequest request, UserViewModel user)
        //{
        //    if (user != null && ModelState.IsValid)
        //    {
        //        ApplicationUser u = new ApplicationUser
        //        {
        //            UserName = user.UserName,
        //            FirstName = user.FirstName,
        //            LastName = user.LastName,
        //            Email = user.Email
        //        };

        //        db.Users.Add(u);
        //        db.SaveChanges();
        //    }

        //    return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        //}

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, UserViewModel user)
        {
            if (user != null && ModelState.IsValid)
            {
                var target = db.Users.All().FirstOrDefault(u => u.Id == user.Id);
                if (target != null)
                {
                    target.UserName = user.UserName;
                    target.FirstName = user.FirstName;
                    target.LastName = user.LastName;
                    target.Email = user.Email;
                    db.Users.Update(target);
                    db.SaveChanges();
                }
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, UserViewModel user)
        {
            if (user != null)
            {
                var u = db.Users.All().First(x => x.Id == user.Id);
                db.Users.Delete(u);
                db.SaveChanges();
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult EditRoles(string username)
        {
            var user = db.Users.All().FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
            if (user == null)
            {
                return HttpNotFound();
            }

            var roles = db.Roles.All().ToList();
            var userRoles = db.UserRoles.All().ToList();
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = userRoles.Where(x => x.UserId == user.Id).Select(x => x.Role.Name).ToArray()
            };

            ViewData["roles"] = roles.Select(role => role.Name).ToArray();

            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoles([DataSourceRequest] DataSourceRequest request, UserRolesViewModel viewModel)
        {
            if (viewModel != null && ModelState.IsValid)
            {
                var target = db.Users.All().FirstOrDefault(u => u.Id == viewModel.Id);
                if (target != null)
                {
                    SetRoles(target, viewModel.Roles);
                    db.Users.Update(target);
                    db.SaveChanges();
                }
            }

            return View("Index");
            //return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
        }

        private void SetRoles(ApplicationUser user, ICollection<string> roles)
        {
            var userRoles = db.UserRoles.All().Where(x => x.UserId == user.Id).ToList();
            while (userRoles.Count > 0)
            {
                db.UserRoles.Delete(userRoles.First());
                userRoles.RemoveAt(0);
            }

            foreach (var role in roles)
            {
                user.Roles.Add(new UserRole { RoleId = db.Roles.All().First(x => x.Name == role).Id, UserId = user.Id });
            }
        }
    }
}
