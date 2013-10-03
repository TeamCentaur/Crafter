using Crafter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;

namespace TeamCentaur_LiveChat.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {

        }

        public UserViewModel(ApplicationUser user)
        {
            this.UserName = user.UserName;
            this.Age = user.Age;
            this.City = user.City;
            this.Description = user.Description;
            this.Email = user.Email;
            this.ImageUrl = user.ImageUrl;
            this.Tutorials = user.Tutorials.AsQueryable().Select(TutorialViewModel.FromTutorial);
        }

        public string UserName { get; set; }
        public string Email { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public int Age { get; set; }

        public string City { get; set; }

        public virtual IQueryable<TutorialViewModel> Tutorials { get; set; }

        public static Expression<Func<ApplicationUser, UserViewModel>> FromUser
        {
            get
            {
                return x => new UserViewModel()
                {
                    UserName = x.UserName,
                    Age = x.Age,
                    City = x.City,
                    Description = x.Description,
                    Email = x.Email,
                    ImageUrl = x.ImageUrl,
                    Tutorials = x.Tutorials.AsQueryable().Select(TutorialViewModel.FromTutorial)
                };
            }
        }
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