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
        public DbSet<Category> Categories { get; set; }

        public DbSet<Tutorial> Tutorials { get; set; }

        public DbSet<Step> Steps { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}