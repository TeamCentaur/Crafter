using Crafter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TeamCentaur_LiveChat.ViewModels
{
    public class TutorialViewModel
    {

        public int Id { get; set; }
        public string Title { get; set; }

        public static Expression<Func<Tutorial, TutorialViewModel>> FromTutorial
        {
            get
            {
                return (x => new TutorialViewModel() { Id = x.Id, Title = x.Title });
            }
        }
    }
}