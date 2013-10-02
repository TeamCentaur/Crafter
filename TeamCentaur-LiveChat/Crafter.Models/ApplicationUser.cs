using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafter.Models
{
    public class ApplicationUser : User
    {
        public ApplicationUser()
            : base()
        {
            this.Tutorials = new HashSet<Tutorial>();
        }

        public string Email { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public int Age { get; set; }

        public string City { get; set; }

        public virtual ICollection<Tutorial> Tutorials { get; set; }


    }
}
