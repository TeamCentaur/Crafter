using Crafter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TeamCentaur_LiveChat.Areas.Admin.ViewModels
{
    public class TutorialDisplayModel
    {
        public static Expression<Func<Tutorial, TutorialDisplayModel>> FromTutorial
        {
            get
            {
                return tutorial => new TutorialDisplayModel
                {
                    Id = tutorial.Id,
                    Description = tutorial.Description,
                    Title = tutorial.Title,
                    Likes = tutorial.Likes,
                    EquipmentUsed = tutorial.EquipmentUsed,
                    CompletionTime = tutorial.CompletionTime,
                    Difficulty = tutorial.Difficulty,
                    CreatedOn = tutorial.CreatedOn,
                    User = tutorial.User.UserName,
                    Category = tutorial.Category.Name,
                    CategoryId = tutorial.Category.Id,
                    Steps =tutorial.Steps.AsQueryable().Select(StepDisplayModel.FromStep).ToList(),
                    //Image = new ImageDisplayModel() { Id = tutorial.Image.Id, ImagePath = tutorial.Image.ImagePath}
                };
            }
        }

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between {2} and {1} symbols.")]
        public string Description { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Title must be between {2} and {1} symbols.")]
        public string Title { get; set; }

        [ScaffoldColumn(false)]
        public int Likes { get; set; }

        public string EquipmentUsed { get; set; }

        public string CompletionTime { get; set; }

        public string Difficulty { get; set; }

        public DateTime CreatedOn { get; set; }

        public string User { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }

        public string Image { get; set; }

        public ICollection<StepDisplayModel> Steps { get; set; }
    }
}