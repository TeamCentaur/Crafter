using Crafter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamCentaur_LiveChat.ViewModels
{
    public class UserViewModel : Crafter.Models.ApplicationUser
    {
        public ICollection<Tutorial> TutorialCollection { get; set; }
    }
}