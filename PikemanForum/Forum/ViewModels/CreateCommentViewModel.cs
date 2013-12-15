using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum.ViewModels
{
    public class CreateCommentViewModel
    {
        [Required]
        public string Content { get; set; }

        public int PostId { get; set; }
    }
}