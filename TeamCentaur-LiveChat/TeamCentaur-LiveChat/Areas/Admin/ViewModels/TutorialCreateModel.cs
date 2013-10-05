using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeamCentaur_LiveChat.Areas.Admin.ViewModels
{
    public class TutorialCreateModel
    {
        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between {2} and {1} symbols.")]
        public string Description { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Title must be between {2} and {1} symbols.")]
        public string Title { get; set; }

        public string EquipmentUsed { get; set; }

        public string CompletionTime { get; set; }

        public string Difficulty { get; set; }

        public int CategoryId { get; set; }

        public HttpPostedFileBase Image { get; set; }
    }
}