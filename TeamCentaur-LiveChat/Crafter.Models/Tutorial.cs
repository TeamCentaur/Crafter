using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafter.Models
{
    public class Tutorial
    {
        public Tutorial()
        {
            this.Steps = new HashSet<Step>();
            this.Comments = new HashSet<Comment>();
            this.Images = new HashSet<Image>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between {2} and {1} symbols.")]
        public string Description { get; set; }

        public virtual ICollection<Step> Steps { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Title must be between {2} and {1} symbols.")]
        public string Title { get; set; }

        public int Likes { get; set; }

        public string EquipmentUsed { get; set; }

        public string CompletionTime { get; set; }

        public string Difficulty { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}
