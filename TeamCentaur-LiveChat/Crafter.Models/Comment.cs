using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crafter.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500, MinimumLength=2, ErrorMessage = "Comment must be between {2} and {1} symbols.")]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Tutorial Tutorial { get; set; }
    }
}