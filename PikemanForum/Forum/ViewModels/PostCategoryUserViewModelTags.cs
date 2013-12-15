using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    using Forum.Models;

    public class PostCategoryUserViewModelTags
    {
        public static Expression<Func<Post, PostCategoryUserViewModelTags>> FromPost
        {
            get
            {
                return
                    post =>
                        new PostCategoryUserViewModelTags
                        {
                            Id = post.Id,
                            Title = post.Title,
                            PostContent = post.Content,
                            CategoryName = post.Category.Name,
                            CategoryId = post.Category.Id,
                            Creator = post.User.UserName,
                            CreatorId = post.User.Id,
                            TagId = post.Tags.FirstOrDefault().Id,
                            TagName = post.Tags.FirstOrDefault().Name,
                            Tags = post.Tags,
                            Timespan = post.Timespan
                        };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string PostContent { get; set; }

        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public string Creator { get; set; }

        public string CreatorId { get; set; }

        public DateTime Timespan { get; set; }

        public int TagId { get; set; }

        public string TagName { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } 
    }
}