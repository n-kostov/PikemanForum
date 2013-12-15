using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    using Forum.Models;

    public class PostCategoryUserViewModel
    {
        public static Expression<Func<Post, PostCategoryUserViewModel>> FromPost
        {
            get
            {
                return
                    post =>
                        new PostCategoryUserViewModel
                        {
                            Id = post.Id,
                            Title = post.Title,
                            PostContent = post.Content,
                            CategoryName = post.Category.Name,
                            CategoryId = post.Category.Id,
                            Creator = post.User,
                            CreatorId = post.User.Id,
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

        public ApplicationUser Creator { get; set; }

        public string CreatorId { get; set; }

        public DateTime Timespan { get; set; }

        public ICollection<Tag> Tags { get; set; }

        
    }
}