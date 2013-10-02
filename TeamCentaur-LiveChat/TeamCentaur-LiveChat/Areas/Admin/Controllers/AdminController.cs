using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeamCentaur_LiveChat.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/Admin/
        //[Authorize(Roles="Administrator")]
        public ActionResult Index()
        {
            return View();
        }
	}
}