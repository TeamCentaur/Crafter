using Crafter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TeamCentaur_LiveChat.Areas.Admin.ViewModels
{
    public class CommentDisplayModel
    {
        public static Expression<Func<Comment, CommentDisplayModel>> FromComment
        {
            get
            {
                return comment => new CommentDisplayModel
                {
                    Content = comment.Content,
                    CreatedOn = comment.CreatedOn,
                    Id = comment.Id,
                    User = comment.User.UserName,
                    UserId = comment.User.Id
                };
            }
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }

        public string User { get; set; }
    }
}