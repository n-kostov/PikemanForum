using Forum.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Forum.ViewModels
{
    public class TagViewModel
    {
        public static Expression<Func<Tag, TagViewModel>> FromTag
        {
            get
            {
                return tag => new TagViewModel
                {
                    TagId = tag.Id,
                    TagName = tag.Name,
                    PostsCount = tag.Posts.Count,
                };
            }
        }

        public int TagId { get; set; }

        public int PostsCount { get; set; }

        [Required]
        [StringLength(50)]
        public string TagName { get; set; }
    }
}