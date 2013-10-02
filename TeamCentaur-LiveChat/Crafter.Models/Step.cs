using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Crafter.Models
{
    public class Step
    {
        public Step()
        {
            this.Images = new HashSet<Image>();
        }

        public int Id { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        [Required]
        [StringLength(300, MinimumLength= 10, ErrorMessage="Step should be between {2} and {1} symbols.")]
        public string Content { get; set; }

        public virtual Tutorial Tutorial { get; set; }
    }
}
