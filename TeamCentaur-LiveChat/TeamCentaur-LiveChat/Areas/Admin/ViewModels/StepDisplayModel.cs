using Crafter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TeamCentaur_LiveChat.Areas.Admin.ViewModels
{
    public class StepDisplayModel
    {
        public static Expression<Func<Step, StepDisplayModel>> FromStep
        {
            get
            {
                return step => new StepDisplayModel
                {
                    Content = step.Content,
                    Id = step.Id,
                    Image = step.Image,
                    Title = step.Title
                };
            }
        }

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Step title should be between {2} and {1} symbols.")]
        public string Title { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "Step content should be between {2} and {1} symbols.")]
        public string Content { get; set; }

        public string Image { get; set; }
    }
}