using Crafter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TeamCentaur_LiveChat.ViewModels;

namespace TeamCentaur_LiveChat.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetProfile(string userName)
        {
            var context = new CrafterContext();

            if (string.IsNullOrEmpty(userName))
            {
                userName = User.Identity.Name;
            }

            var user = context.Users.Include(u => u.Tutorials).FirstOrDefault(u => u.UserName == userName);

            return this.View(user);
        }
    }
}