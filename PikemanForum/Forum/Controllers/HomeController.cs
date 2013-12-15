using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum.Controllers
{
    using System.Data.Entity;
    using System.Web.Security;

    using Forum.Models;

    using Microsoft.AspNet.Identity;
    using Forum.Data;
    using Forum.ViewModels;
    using PagedList;

    public class HomeController : Controller
    {
        IUowData db = new UowData();

        [ValidateInput(false)]
        public ActionResult Index(int? page)
        {
            var posts = db.Posts.All().Select(PostCategoryUserViewModel.FromPost);
            var orderedPosts = posts.OrderByDescending(post => post.Id);

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(orderedPosts.ToPagedList(pageNumber, pageSize));
        }

        [ChildActionOnly]
        public PartialViewResult ListCategories()
        {
            var model = db.Categories.All().ToList();
            return PartialView("_ListCategories", model);
        }

        [ValidateInput(false)]
        public ActionResult Search(string searchString, int? page)
        {
            if (searchString != null)
            {
                if (searchString.Contains('<'))
                {
                    searchString = searchString.Replace("<", "&lt;");
                }
                if (searchString.Contains('>'))
                {
                    searchString = searchString.Replace(">", "&gt;");
                }
            }

            ViewBag.SearchString = searchString;


            var posts = db.Posts.All().Include("Tags").Select(PostCategoryUserViewModel.FromPost);

            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper())
                                       || s.PostContent.ToUpper().Contains(searchString.ToUpper()));
            }

            var orderedPosts = posts.OrderBy(post => post.Id);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(orderedPosts.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}