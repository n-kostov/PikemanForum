using Forum.Data;
using Forum.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PagedList;

namespace Forum.Controllers
{
    public class UsersController : Controller
    {
        private UowData db = new UowData();

        public ActionResult Users()
        {
            return View();
        }

        public JsonResult GetUsers([DataSourceRequest]DataSourceRequest request)
        {
            var users = GetAllUsers().OrderByDescending(u => u.PostCount);

            return Json(users.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<UsersListViewModel> GetAllUsers()
        {
            var users = db.Users.All()
                .Select(UsersListViewModel.FromUser);

            return users;
        }

        public ActionResult Details(int? page, string username)
        {
            var user = db.Users.All().FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
            ViewBag.CurrentUser = user;

            var posts = user.Posts.AsQueryable().Select(PostCategoryUserViewModel.FromPost);
            var orderedPosts = posts.OrderBy(p => p.Id);

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(orderedPosts.ToPagedList(pageNumber, pageSize));
        }
    }
}