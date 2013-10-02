using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafter.Models
{
    public class Category
    {
        public Category()
        {
            this.Tutorials = new HashSet<Tutorial>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Category should be between {2} and {1} symbols.")]
        public string Name { get; set; }

        public virtual ICollection<Tutorial> Tutorials { get; set; }
    }
}
