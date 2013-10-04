using Crafter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TeamCentaur_LiveChat.ViewModels;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

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


        [ActionName("profile")]
        public ActionResult Profile(string userName)
        {
            var context = new CrafterContext();

            if (string.IsNullOrEmpty(userName))
            {
                userName = User.Identity.Name;
            }

            var user = context.Users.Include(u => u.Tutorials).FirstOrDefault(u => u.UserName == userName);

            var userVm = new UserViewModel(user);
            return this.View(userVm);
        }

        public ActionResult All()
        {
            ViewBag.Query = Request.Params["query"];
            return View(ViewBag);
        }

        public ActionResult GetUsers([DataSourceRequest]DataSourceRequest request)
        {
            var context = new CrafterContext();

            var data = context.Users;

            var users = data.AsQueryable();
            string query = Request.Params["query"];
            if (query != null)
            {
                users = data.Where(u => u.UserName.ToLower().Contains(query.ToLower()));
            }
            var models = users.Select(TeamCentaur_LiveChat.ViewModels.SimpleUserViewModel.FromUser);


            DataSourceResult result = models.ToDataSourceResult(request);

            return Json(result);
        }
    }
}