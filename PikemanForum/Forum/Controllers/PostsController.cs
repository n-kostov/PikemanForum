using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using Forum.Data;
using Forum.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Forum.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Net;

    using Forum.ViewModels;

    [Authorize]
    public class PostsController : Controller
    {
        private UowData db = new UowData();

        [AllowAnonymous]
        public ActionResult PostAndComments(int postId, int page = 1)
        {
            {
                if (page < 1)
                {
                    page = 1;
                }

                var post = db.Posts.GetById(postId);
                ViewBag.post = post;

                int pid = post.Id;
                //IQueryable<Comment> commentsProba = db.Comments.All().Where(c => c.Post.Id == postId).OrderBy(comment => comment.Id);
                IQueryable<Comment> comments = post.Comments.OrderByDescending(d => d.Timespan).AsQueryable();
                IPagedList<Comment> productPage = comments.ToPagedList(page, 3);

                return View(productPage);
            }
        }

        //public ActionResult WriteComment(string content, int? postId, string id)
        //{
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("_writeComment", postId);
        //    }

        //    var user = db.Users.FindUser(User.Identity.GetUserId());
        //    var pId = int.Parse(id);
        //    var post = db.Posts.GetById(pId);

        //    var comment = new Comment
        //    {
        //        Content = content,
        //        User = user,
        //        Post = post
        //    };
        //    db.Comments.Add(comment);
        //    db.SaveChanges();

        //    return RedirectToAction("PostAndComments", "Posts", new { postId = pId });
        //}

        public ActionResult WriteComment(int postId)
        {
            var post = db.Posts.GetById(postId);
            return PartialView("_writeComment", new Comment { Post = post });
        }

        [HttpPost]
        public ActionResult WriteComment(Comment comment)
        {

            if (comment != null)
            {
                var post = db.Posts.GetById(comment.Post.Id);
                if (post != null)
                {
                    comment.Post = post;
                    var user = db.Users.FindUser(User.Identity.GetUserId());
                    comment.User = user;
                    db.Comments.Add(comment);
                    db.SaveChanges();

                    return PartialView("_SingleComment", comment);
                }
            }

            return PartialView("_writeComment", comment);
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
            ViewBag.Categories = db.Categories.All().ToList()
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

            return View();
        }


        [HttpPost]
        public ActionResult CreatePost(PostViewModel post)
        {
            var user = db.Users.FindUser(User.Identity.GetUserId());
            if (post != null && user != null && ModelState.IsValid)
            {
                Post postToAdd = new Post
                {
                    CategoryId = post.CategoryId,
                    Content = post.Content,
                    Title = post.Title,
                    User = user
                };

                if (post.Tags != null && post.Tags.Count() > 0)
                {
                    var tags = post.Tags.ElementAt(0).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tag in tags)
                    {
                        var target = db.Tags.All().FirstOrDefault(t => t.Name == tag);
                        if (target == null)
                        {
                            target = new Tag
                            {
                                Name = tag
                            };
                        }

                        postToAdd.Tags.Add(target);
                    }
                }

                db.Posts.Add(postToAdd);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            var modelPost = db.Posts.All().Select(PostViewModel.FromPost).FirstOrDefault();

            ViewBag.Categories = db.Categories.All().ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            return View(modelPost);
        }

        // GET: /BugsAdministration/Edit/5
        [HttpGet]
        public ActionResult EditPost(int? postId)
        {
            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = db.Posts.GetById((int)postId);
            var user = db.Users.FindUser(User.Identity.GetUserId());

            if (post == null)
            {
                return HttpNotFound();
            }

            var modelPost = new PostViewModel
                            {
                                Id = post.Id,
                                Title = post.Title,
                                Content = post.Content,
                                CategoryName = post.Category.Name,
                                CategoryId = post.Category.Id,
                                Creator = user.UserName,
                                CreatorId = user.Id,
                                Tags = post.Tags.Select(tag => tag.Name).ToList(),
                                Timespan = post.Timespan
                            };

            ViewBag.Categories = db.Categories.All().ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            return View(modelPost);
        }

        // POST: /BugsAdministration/Edit/5
        // To protect from over posting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // 
        // Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(PostViewModel post)
        {
            if (post != null && ModelState.IsValid)
            {
                Post postToEdit = db.Posts.GetById(post.Id);
                postToEdit.CategoryId = post.CategoryId;
                postToEdit.Content = post.Content;
                postToEdit.Title = post.Title;

                if (post.Tags != null && post.Tags.Count() > 0)
                {
                    foreach (var tag in postToEdit.Tags.ToList())
                    {
                        postToEdit.Tags.Remove(tag);
                    }

                    var tags = post.Tags.ElementAt(0).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tag in tags)
                    {
                        var target = db.Tags.All().FirstOrDefault(t => t.Name == tag);
                        if (target == null)
                        {
                            target = new Tag
                            {
                                Name = tag
                            };
                        }

                        postToEdit.Tags.Add(target);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("PostAndComments", "Posts", new { postId = post.Id });
            }

            ViewBag.Categories = db.Categories.All().ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            return View(post);
        }

        // GET: /BugsAdministration/Delete/5
        [HttpGet]
        public ActionResult DeletePost(int? postId)
        {
            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.GetById((int)postId);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //// POST: /BugsAdministration/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int postId)
        {
            var post = db.Posts.GetById(postId);
            var comments = db.Comments.All().Where(x => x.Post.Id == postId);
            foreach (var comment in comments)
            {
                db.Comments.Delete(comment);
                db.SaveChanges();
            }
            db.Posts.Delete(post);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}