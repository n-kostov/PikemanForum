using Forum.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Forum.ViewModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using PagedList;

namespace Forum.Controllers
{
    using Microsoft.Ajax.Utilities;

    public class TagsController : Controller
    {
        private UowData db = new UowData();

        public ActionResult Tags()
        {
            return View(GetTags().ToList());
        }

        public ActionResult TagPosts(int? page, int tagId)
        {
            var currentTag = db.Tags.GetById(tagId);
            ViewBag.CurrentTagName = currentTag.Name;
            ViewBag.CurrentTagId = currentTag.Id;

            if (currentTag == null)
            {
                return View();
            }

            var posts = currentTag.Posts.AsQueryable().Select(PostCategoryUserViewModel.FromPost);
            var orderedPosts = posts.OrderBy(p => p.Id);

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(orderedPosts.ToPagedList(pageNumber, pageSize));
        }

        private IEnumerable<TagViewModel> GetTags() 
        {
            var tags = db.Tags.All().Include("Posts")
               .Select(TagViewModel.FromTag)
               .OrderByDescending(t => t.PostsCount).Take(30);

            return tags;
        }
	}
}