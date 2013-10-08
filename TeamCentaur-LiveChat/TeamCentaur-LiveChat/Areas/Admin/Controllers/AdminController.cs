using Crafter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using TeamCentaur_LiveChat.Areas.Admin.ViewModels;
using Crafter.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace TeamCentaur_LiveChat.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private CrafterContext db;

        public AdminController()
        {
            this.db = new CrafterContext();
        }
        //
        // GET: /Admin/Admin/
        [Authorize(Roles="Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCategories([DataSourceRequest] DataSourceRequest request)
        {
            var categories = this.db.Categories.Select(CategoryDisplayModel.FromCategory).ToList();

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MakeAdmin(string userId)
        {

            return Content("");
        }

        public ActionResult Users()
        {
            return View();
        }

        public JsonResult GetUsers([DataSourceRequest] DataSourceRequest request)
        {
            var users = this.db.Users.Select(usr =>
                new ApplicationUserViewModel 
                {
                    Age = usr.Age,
                    City = usr.City,
                    Id = usr.Id,
                    UserName = usr.UserName
                });

            return Json(users.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComments([DataSourceRequest] DataSourceRequest request, int id)
        {
            var tutorial = this.db.Tutorials.Include("Comments").FirstOrDefault(t => t.Id == id);
            var comments = tutorial.Comments.AsQueryable().Select(CommentDisplayModel.FromComment);

            return Json(comments.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteComment([DataSourceRequest] DataSourceRequest request, CommentDisplayModel commentModel)
        {
            var comment = this.db.Comments.Find(commentModel.Id);

            if (comment != null)
            {
                this.db.Comments.Remove(comment);
                this.db.SaveChanges();
            }

            return Json(new[] { commentModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSteps([DataSourceRequest] DataSourceRequest request, int id)
        {
            var tutorial = this.db.Tutorials.Include("Steps").FirstOrDefault(t => t.Id == id);
            var steps = tutorial.Steps.AsQueryable().Select(StepDisplayModel.FromStep);

            return Json(steps.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteStep([DataSourceRequest] DataSourceRequest request, StepDisplayModel stepModel)
        {
            var step = this.db.Steps.FirstOrDefault(st => st.Id == stepModel.Id);

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

        public JsonResult GetTutorials([DataSourceRequest] DataSourceRequest request)
        {
            var tutorials = this.db.Tutorials.Select(TutorialDisplayModel.FromTutorial);

            return Json(tutorials.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateTutorial([DataSourceRequest] DataSourceRequest request, TutorialCreateModel tutorialModel)
        {
            var category = this.db.Categories.FirstOrDefault(c => c.Id == tutorialModel.CategoryId);
            var user = this.db.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            TutorialDisplayModel result = new TutorialDisplayModel();

            if (tutorialModel != null &&user != null)
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

        public JsonResult DeleteTutorial([DataSourceRequest] DataSourceRequest request, TutorialDisplayModel model)
        {
            var tutorial = this.db.Tutorials.Include("Steps").Include("Comments").Include("Images").FirstOrDefault(t => t.Id == model.Id);

            if (tutorial != null)
            {
                this.db.Steps.RemoveRange(tutorial.Steps);
                this.db.Comments.RemoveRange(tutorial.Comments);
                this.db.Tutorials.Remove(tutorial);
                this.db.SaveChanges();
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTutorial([DataSourceRequest] DataSourceRequest request, TutorialDisplayModel model)
        {
            var tutorial = this.db.Tutorials.Find(model.Id);

            if (tutorial != null && ModelState.IsValid)
            {
                var category = this.db.Categories.Find(model.CategoryId);

                tutorial.CompletionTime = model.CompletionTime;
                tutorial.Description = model.Description;
                tutorial.Difficulty = model.Difficulty;
                tutorial.EquipmentUsed = model.EquipmentUsed;
                tutorial.Likes = model.Likes;
                tutorial.Title = model.Title;

                var newImage = this.GetTutorialImage(tutorial.Title);

                if (newImage != null)
                {
                    tutorial.Image = newImage;
                }

                tutorial.Category = category;

                this.db.SaveChanges();

                model.Category = category.Name;
                model.CategoryId = category.Id;
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
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

            var relativePath = "~/Uploaded_Files/Users/" + User.Identity.Name + "/Tutorials/" + tutorialTitle + "/Steps/" + StepTitle + "/";
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
    }
}