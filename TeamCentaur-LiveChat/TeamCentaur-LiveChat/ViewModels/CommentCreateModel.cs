using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeamCentaur_LiveChat.ViewModels
{
    public class CommentCreateModel
    {
        [Required]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Comment must be between {2} and {1} symbols.")]
        [AllowHtml]
        public string Content { get; set; }
    }
}