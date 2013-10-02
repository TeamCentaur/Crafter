using Crafter.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafter.Data
{
    public class CrafterContext : IdentityDbContextWithCustomUser<ApplicationUser>
    {
        public IDbSet<Category> Categories { get; set; }

        public IDbSet<Tutorial> Tutorials { get; set; }

        public IDbSet<Step> Steps { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<Image> Images { get; set; }
    }
}
