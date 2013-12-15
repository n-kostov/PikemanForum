using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum.Controllers
{
    using Forum.Data;
    using Forum.ViewModels;

    using PagedList;

    public class CategoriesController : Controller
    {
        IUowData db = new UowData();


        [ValidateInput(false)]
        public ActionResult CategoryPosts(int? page, int categoryId)
        {
            var posts = db.Posts.All().Select(PostCategoryUserViewModel.FromPost).Where(po => po.CategoryId == categoryId);

            var orderedPosts = posts.OrderByDescending(post => post.Id);

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var category = db.Categories.GetById(categoryId);
            ViewBag.Category = category;
            return View(orderedPosts.ToPagedList(pageNumber, pageSize));
        }
    }
}