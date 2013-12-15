using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    using Forum.Models;

    public class CategoryViewModel
    {
        public static Expression<Func<Category, CategoryViewModel>> FromCategory
        {
            get
            {
                return
                    category =>
                        new CategoryViewModel
                        {
                            CategoryId = category.Id,
                            CategoryName = category.Name,
                            Posts = category.Posts,
                        };
            }
        }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}