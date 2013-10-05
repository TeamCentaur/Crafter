using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Crafter.Models;
using Crafter.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using TeamCentaur_LiveChat.Areas.Admin.ViewModels;
using System.IO;
using System.Text.RegularExpressions;

namespace TeamCentaur_LiveChat.Controllers
{
    public class TutorialsController : Controller
    {
        private CrafterContext db = new CrafterContext();

        // GET: /Tutorials/
        public ActionResult Index()
        {
            return View(db.Tutorials.ToList());
        }

        public ActionResult MyTutorials()
        {
            return View();
        }

        [Authorize]
        public JsonResult GetMyTutorials([DataSourceRequest] DataSourceRequest request)
        {
            var user = this.db.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            if (user == null)
            {
                return Json(new List<TutorialDisplayModel>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }

            var tutorials = this.db.Tutorials.Include("Steps").Where(t => t.User.UserName == user.UserName).Select(TutorialDisplayModel.FromTutorial);

            return Json(tutorials.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategories([DataSourceRequest] DataSourceRequest request)
        {
            var categories = this.db.Categories.Select(CategoryDisplayModel.FromCategory).ToList();

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        // GET: /Tutorials/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutorial tutorial = db.Tutorials.Include("Steps").Include("Comments").FirstOrDefault(t => t.Id == id);
            if (tutorial == null)
            {
                return HttpNotFound();
            }
            return View(tutorial);
        }

        public JsonResult CreateTutorial([DataSourceRequest] DataSourceRequest request, TutorialCreateModel tutorialModel)
        {
            var category = this.db.Categories.FirstOrDefault(c => c.Id == tutorialModel.CategoryId);
            var user = this.db.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            TutorialDisplayModel result = new TutorialDisplayModel();

            if (tutorialModel != null && user != null)
            {
                Tutorial newTutorial = new Tutorial();
                newTutorial.Category = category;
                newTutorial.CompletionTime = tutorialModel.CompletionTime;
                newTutorial.Description = tutorialModel.Description;
                newTutorial.Difficulty = tutorialModel.Difficulty;
                newTutorial.EquipmentUsed = tutorialModel.EquipmentUsed;
                newTutorial.Title = tutorialModel.Title;
                newTutorial.User = user;
                newTutorial.CreatedOn = DateTime.Now;
                newTutorial.Image = this.GetTutorialImage(newTutorial.Title);

                this.db.Tutorials.Add(newTutorial);
                this.db.SaveChanges();

                result.Category = category.Name;
                result.CategoryId = category.Id;
                result.CompletionTime = newTutorial.CompletionTime;
                result.Id = newTutorial.Id;
                result.CreatedOn = newTutorial.CreatedOn;
                result.Description = newTutorial.Description;
                result.Difficulty = newTutorial.Difficulty;
                result.EquipmentUsed = newTutorial.EquipmentUsed;
                result.Title = newTutorial.Title;
                result.User = user.UserName;
            }

            return Json(new[] { result }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateStep([DataSourceRequest] DataSourceRequest request, int tutorialId, StepDisplayModel stepModel)
        {

            var tutorial = this.db.Tutorials.Find(tutorialId);
            var user = this.db.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            if (user != tutorial.User)
            {
                ModelState.AddModelError("Permissions", "You don't have permissions");
            }

            if (tutorial != null && user != null && tutorial.User == user && ModelState.IsValid)
            {
                Step step = new Step();
                step.Content = stepModel.Content;
                step.Title = stepModel.Title;

                string newImage = this.GetStepImage(tutorial.Title, step.Title);

                if (newImage != null)
                {
                    step.Image = newImage;
                }

                tutorial.Steps.Add(step);
                db.SaveChanges();
            }

            return Json(new[] { stepModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSteps([DataSourceRequest] DataSourceRequest request, int id)
        {
            var tutorial = this.db.Tutorials.Include("Steps").FirstOrDefault(t => t.Id == id);
            var steps = tutorial.Steps.AsQueryable().Select(StepDisplayModel.FromStep);

            return Json(steps.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteStep([DataSourceRequest] DataSourceRequest request, int id, StepDisplayModel stepModel)
        {
            var step = this.db.Steps.Include("Tutorial").FirstOrDefault(st => st.Id == stepModel.Id);
            var tutorial = step.Tutorial;
            var user = tutorial.User;


            if (user.UserName != HttpContext.User.Identity.Name)
            {
                ModelState.AddModelError("Permissions", "You don't have permissions");
                return Json(new[] { stepModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            }

            if (step != null)
            {
                this.db.Steps.Remove(step);
                this.db.SaveChanges();
            }

            return Json(new[] { stepModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStep([DataSourceRequest] DataSourceRequest request, StepDisplayModel stepModel)
        {
            var step = this.db.Steps.FirstOrDefault(st => st.Id == stepModel.Id);

            if (step != null && ModelState.IsValid)
            {
                step.Title = stepModel.Title;
                step.Content = stepModel.Content;
                var tutorial = step.Tutorial;

                string newImage = this.GetStepImage(tutorial.Title, step.Title);

                if (newImage != null)
                {
                    step.Image = newImage;
                }

                this.db.SaveChanges();
            }

            return Json(new[] { stepModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveTutorialImage(IEnumerable<HttpPostedFileBase> files)
        {
            string imageLocation = string.Empty;

            foreach (var file in files)
            {
                string destinationFolder = Server.MapPath("~/Uploaded_Files/Users/" + User.Identity.Name + "/Tutorials/Temp/");
                string extension = Path.GetExtension(file.FileName);

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                var destinationPath = destinationFolder + "tutorial-image" + extension;

                file.SaveAs(destinationPath);
            }

            return Content("");
        }

        public ActionResult SaveStepImage(IEnumerable<HttpPostedFileBase> files)
        {
            string imageLocation = string.Empty;

            foreach (var file in files)
            {
                string destinationFolder = Server.MapPath("~/Uploaded_Files/Users/" + User.Identity.Name + "/Steps/Temp/");
                string extension = Path.GetExtension(file.FileName);

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                var destinationPath = destinationFolder + "step-image" + extension;

                file.SaveAs(destinationPath);
            }

            return Content("");
        }

        private string GetTutorialImage(string tutorialTitle)
        {
            string searchFolder = Server.MapPath("~/Uploaded_Files/Users/" + User.Identity.Name + "/Tutorials/Temp/");

            if (!Directory.Exists(searchFolder))
            {
                return null;
            }

            var relativePath = "~/Uploaded_Files/Users/" + User.Identity.Name + "/Tutorials/" + tutorialTitle + "/";

            string destinationFolder = Server.MapPath(relativePath);

            if (Directory.Exists(destinationFolder))
            {
                Directory.Delete(destinationFolder);
            }

            Directory.Move(searchFolder, destinationFolder);
            var file = Directory.GetFiles(destinationFolder).FirstOrDefault();

            if (file == null)
            {
                return null;
            }

            var imageFileName = Regex.Match(file, @"[^\\]+$").Value;
            var result = relativePath.Substring(1, relativePath.Length - 1) + imageFileName;
            return result;
        }

        private string GetStepImage(string tutorialTitle, string StepTitle)
        {
            string searchFolder = Server.MapPath("~/Uploaded_Files/Users/" + User.Identity.Name + "/Steps/Temp/");

            if (!Directory.Exists(searchFolder))
            {
                return null;
            }

            string stepsPath = "~/Uploaded_Files/Users/" + User.Identity.Name + "/Tutorials/" + tutorialTitle + "/Steps/";
            string relativePath = stepsPath  + StepTitle + "/";
            string destinationFolder = Server.MapPath(relativePath);

            if (Directory.Exists(destinationFolder))
            {
                Directory.Delete(destinationFolder);
            }

            if (!Directory.Exists(stepsPath))
            {
                Directory.CreateDirectory(Server.MapPath(stepsPath));
            }

            Directory.Move(searchFolder, destinationFolder);
            var file = Directory.GetFiles(destinationFolder).FirstOrDefault();

            if (file == null)
            {
                return null;
            }

            var imageFileName = Regex.Match(file, @"[^\\]+$").Value;
            var result = relativePath.Substring(1, relativePath.Length - 1) + imageFileName;
            return result;
        }
    }
}
