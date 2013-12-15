using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(5000)]
        public string Content { get; set; }

        public DateTime Timespan { get; private set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Tag> Tags { get; set; } 

        public Post()
        {
            Comments = new HashSet<Comment>();
            Tags = new HashSet<Tag>();
            Timespan = DateTime.Now;
        }
    }
}
