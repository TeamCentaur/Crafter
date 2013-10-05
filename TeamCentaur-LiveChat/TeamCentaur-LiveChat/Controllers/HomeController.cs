using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamCentaur_LiveChat.Models;
using Crafter.Models;
using Crafter.Data;

namespace TeamCentaurLiveChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Tutorials");
        }
    }
}