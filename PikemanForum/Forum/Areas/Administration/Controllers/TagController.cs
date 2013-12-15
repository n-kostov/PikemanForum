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

namespace Forum.Areas.Administration.Controllers
{

    [Authorize(Roles="Administrator")]
    public class TagController : Controller
    {
        private IUowData db = new UowData();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.Tags.All().Select(tag => new TagViewModel
            {
                TagId = tag.Id,
                TagName = tag.Name
            }).ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, TagViewModel tag)
        {
            if (tag != null && ModelState.IsValid)
            {
                Tag t = new Tag
                {
                    Name = tag.TagName
                };

                db.Tags.Add(t);
                db.SaveChanges();
            }

            return Json(new[] { tag }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, TagViewModel tag)
        {
            if (tag != null && ModelState.IsValid)
            {
                var target = db.Tags.GetById(tag.TagId);
                if (target != null)
                {
                    target.Name = tag.TagName;
                    db.Tags.Update(target);
                    db.SaveChanges();
                }
            }

            return Json(new[] { tag }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, TagViewModel tag)
        {
            if (tag != null)
            {
                db.Tags.Delete(tag.TagId);
                db.SaveChanges();
            }

            return Json(new[] { tag }.ToDataSourceResult(request, ModelState));
        }
    }
}
