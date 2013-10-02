﻿using System;
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

            var users = context.Users.Select(TeamCentaur_LiveChat.ViewModels.SimpleUserViewModel.FromUser);
            return PartialView("_Search", users);
        }

        
    }
}