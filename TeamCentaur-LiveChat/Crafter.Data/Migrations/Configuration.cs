namespace Crafter.Data.Migrations
{
    using Crafter.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Crafter.Data.CrafterContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Crafter.Data.CrafterContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //PopulateDb(context);
        }




        private void PopulateDb(CrafterContext context)
        {
           var categories = new List<Category>();

           categories.Add(new Category(){ Name = "Babies and Kids" });
           categories.Add(new Category(){ Name = "Bath and Beauty" });
           categories.Add(new Category(){ Name = "Bridal & Weddings" });
           categories.Add(new Category(){ Name = "Ceramics" });
           categories.Add(new Category(){ Name = "Crochet" });
           categories.Add(new Category(){ Name = "Cross-Stitch" });
           categories.Add(new Category(){ Name = "Decorations" });
           categories.Add(new Category(){ Name = "Embroidery" });
           categories.Add(new Category(){ Name = "Fashion" });
           categories.Add(new Category(){ Name = "Fundamental Skills" });
           categories.Add(new Category(){ Name = "Furniture" });
           categories.Add(new Category(){ Name = "Geek Craft" });
           categories.Add(new Category(){ Name = "Homewares" });
           categories.Add(new Category(){ Name = "Jewellery" });
           categories.Add(new Category(){ Name = "Knitting" });
           categories.Add(new Category(){ Name = "Paper Crafts" });
           categories.Add(new Category(){ Name = "Polymer & Air-Dry Clay" });
           categories.Add(new Category(){ Name = "Printmaking" });
           categories.Add(new Category(){ Name = "Repurposing & Upcycling" });
           categories.Add(new Category(){ Name = "Sewing" });
           categories.Add(new Category(){ Name = "String Art" });
           categories.Add(new Category(){ Name = "Sugar craft" });
           categories.Add(new Category() { Name = "Woodwork" });

           //for (int i = 0; i < categories.Count; i++)
           //{
           //    context.Categories.AddOrUpdate(categories[i]);

           //}

           
           for (int i = 0; i < 22; i++)
           {
               var imageForTutorial = new Image()
               {
                   ImagePath = "http://cdn.tutsplus.com/craft.tutsplus.com/uploads/2013/07/preview200-paperflowerbJPG.jpg"
               };

               var imageForStep = new Image()
               {
                   ImagePath = "http://cdn.tutsplus.com/craft.tutsplus.com/uploads/2013/09/Totes-Awesome-Embroidery-Trim-Stabilizer1.jpg"
               };

               var step = new Step()
               {
                   Content = "Step description " + i.ToString(),
               };
               step.Images.Add(imageForStep);

               var commentForTutorial = new Comment()
               {
                   Content = "Ko?Ne!Ko?Ne!Ko?Ne!Ko?Ne!Ko?Ne!Ko?Ne!Ko?Ne!Ko?Ne!Ko?Ne!Ko?Ne!Ko?Ne!",
                   CreatedOn = DateTime.Now,
                   User = context.Users.FirstOrDefault()
               };

               var newTutorial = new Tutorial()
               {
                   Title = "Tutorial " + i.ToString(),
                   Category = categories[i],
                   Difficulty = "Easy",
                   Description = "Description " + i.ToString(),
                   CreatedOn = DateTime.Now,
                   CompletionTime = i.ToString() + "6 hours",
                   EquipmentUsed = "no shit is used in these tutorials",
                   User = context.Users.FirstOrDefault(),
               };
               newTutorial.Images.Add(imageForTutorial);
               newTutorial.Comments.Add(commentForTutorial);
               newTutorial.Steps.Add(step);
               context.Tutorials.AddOrUpdate(newTutorial);
           }
        }






        //private void PopulateDb(CrafterContext context)
        //{
        //   var categories = new List<Category>();

        //   categories.Add(new Category(){ Name = "Babies and Kids" });
        //   categories.Add(new Category(){ Name = "Bath and Beauty" });
        //   categories.Add(new Category(){ Name = "Bridal & Weddings" });
        //   categories.Add(new Category(){ Name = "Ceramics" });
        //   categories.Add(new Category(){ Name = "Crochet" });
        //   categories.Add(new Category(){ Name = "Cross-Stitch" });
        //   categories.Add(new Category(){ Name = "Decorations" });
        //   categories.Add(new Category(){ Name = "Embroidery" });
        //   categories.Add(new Category(){ Name = "Fashion" });
        //   categories.Add(new Category(){ Name = "Fundamental Skills" });
        //   categories.Add(new Category(){ Name = "Furniture" });
        //   categories.Add(new Category(){ Name = "Geek Craft" });
        //   categories.Add(new Category(){ Name = "Homewares" });
        //   categories.Add(new Category(){ Name = "Jewellery" });
        //   categories.Add(new Category(){ Name = "Knitting" });
        //   categories.Add(new Category(){ Name = "Paper Crafts" });
        //   categories.Add(new Category(){ Name = "Polymer & Air-Dry Clay" });
        //   categories.Add(new Category(){ Name = "Printmaking" });
        //   categories.Add(new Category(){ Name = "Repurposing & Upcycling" });
        //   categories.Add(new Category(){ Name = "Sewing" });
        //   categories.Add(new Category(){ Name = "String Art" });
        //   categories.Add(new Category(){ Name = "Sugar craft" });
        //   categories.Add(new Category() { Name = "Woodwork" });

        //   //for (int i = 0; i < categories.Count; i++)
        //   //{
        //   //    context.Categories.AddOrUpdate(categories[i]);

        //   //}

        //   for (int i = 0; i < 22; i++)
        //   {
        //       var newTutorial = new Tutorial()
        //       {
        //           Title = "Tutorial " + i.ToString(),
        //           Category = categories[i],
        //           Difficulty = "Easy",
        //           Description = "Description " + i.ToString(),
        //           CreatedOn = DateTime.Now,
        //           CompletionTime = i.ToString() + "6 hours",
        //           EquipmentUsed = "no shit is used in these tutorials",
        //           User = context.Users.FirstOrDefault()
        //       };

        //       context.Tutorials.AddOrUpdate(newTutorial);
        //   }
        //}
    }
}
