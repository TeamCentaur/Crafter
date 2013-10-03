using Crafter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TeamCentaur_LiveChat.ViewModels
{
    public class UserViewModel : Crafter.Models.ApplicationUser
    {
        public ICollection<Tutorial> TutorialCollection { get; set; }
    }

    public class SimpleUserViewModel
    {
        public string UserName { get; set; }

        public string Id { get; set; }


        public static Expression<Func<ApplicationUser, SimpleUserViewModel>> FromUser
        {
            get
            {
                return x => new SimpleUserViewModel()
                {
                    UserName = x.UserName,
                    Id = x.Id
                };
            }
        }

    }
}