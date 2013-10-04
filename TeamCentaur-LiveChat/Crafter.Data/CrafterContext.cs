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
            //modelBuilder.Entity<ApplicationUser>()
            //    .HasOptional(a => a.Tutorials)
            //    .WithMany()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Tutorial>()
            //    .HasOptional(t => t.Comments)
            //    .WithMany()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Tutorial>()
            //    .HasOptional(t => t.Images)
            //    .WithMany()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Tutorial>()
            //    .HasOptional(t => t.Steps)
            //    .WithMany()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Step>()
            //    .HasOptional(s => s.Images)
            //    .WithMany()
            //    .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}