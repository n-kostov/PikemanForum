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
    public class CategoryController : Controller
    {
        private IUowData db = new UowData();

        public ActionResult Index()
        {
            

            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.Categories.All().Select(cat => new CategoryViewModel
            {
                CategoryId = cat.Id,
                CategoryName = cat.Name
            }).ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, CategoryViewModel category)
        {
            if (category != null && ModelState.IsValid)
            {
                Category cat = new Category
                {
                    Name = category.CategoryName
                };

                db.Categories.Add(cat);
                db.SaveChanges();
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, CategoryViewModel category)
        {
            if (category != null && ModelState.IsValid)
            {
                var target = db.Categories.GetById(category.CategoryId);
                if (target != null)
                {
                    target.Name = category.CategoryName;
                    db.Categories.Update(target);
                    db.SaveChanges();
                }
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, CategoryViewModel category)
        {
            if (category != null)
            {
                db.Categories.Delete(category.CategoryId);
                db.SaveChanges();
            }

            return Json(new[] { category }.ToDataSourceResult(request, ModelState));
        }
    }
}
