using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamCentaur_LiveChat.Models;
using Crafter.Models;
using Crafter.Data;

namespace TeamCentaur_LiveChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search()
        {
            var context = new CrafterContext();

            var users = context.Users.Select(UserViewModel.FromUser);
            return PartialView("_Search", users);
        }

        public ActionResult Users()
        {
            var context = new CrafterContext();

            var data = context.Users;

            var users = data.AsQueryable();
            string query = Request.Params["query"];
            if (query != null)
            {
                users = data.Where(u => u.UserName.ToLower().Contains(query.ToLower()));
            }
            var models = users.Select(UserViewModel.FromUser);
            return View("Users", models);
        }
    }

    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Id { get; set; }

        public static System.Linq.Expressions.Expression<Func<ApplicationUser, UserViewModel>> FromUser
        {
            get
            {
                return x => new UserViewModel()
                {
                    UserName = x.UserName,
                    Id = x.Id
                };
            }
        }

        
    }
}