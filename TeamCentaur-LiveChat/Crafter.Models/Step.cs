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
        }

        public int Id { get; set; }

        [Required]
        [StringLength(300, MinimumLength= 10, ErrorMessage="Step content should be between {2} and {1} symbols.")]
        public string Content { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Step title should be between {2} and {1} symbols.")]
        public string Title { get; set; }

        public virtual Tutorial Tutorial { get; set; }

        public string Image { get; set; }
    }
}
