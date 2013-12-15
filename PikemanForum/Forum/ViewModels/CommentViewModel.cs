using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    using Forum.Models;

    public class CommentViewModel
    {
        public static Expression<Func<Comment, CommentViewModel>> FromComment
        {
            get
            {
                return
                    comment =>
                        new CommentViewModel
                        {
                            CommentId = comment.Id,
                            CommentContent = comment.Content,
                            Post = comment.Post,
                            User = comment.User
                        };
            }
        }

        public int CommentId { get; set; }

        public string CommentContent { get; set; }

        public virtual Post Post { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}