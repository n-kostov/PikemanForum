using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    using Forum.Models;

    public class PostViewModel
    {
        public static Expression<Func<Post, PostViewModel>> FromPost
        {
            get
            {
                return
                    post =>
                        new PostViewModel
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Content = post.Content,
                            CategoryName = post.Category.Name,
                            CategoryId = post.Category.Id,
                            Creator = post.User.UserName,
                            CreatorId = post.User.Id,
                            Tags = post.Tags.Select(tag => tag.Name),
                            Timespan = post.Timespan
                        };
            }
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Content { get; set; }

        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public string Creator { get; set; }

        public string CreatorId { get; set; }

        public DateTime Timespan { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}