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

namespace TeamCentaur_LiveChat.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private CrafterContext db;

        public AdminController()
        {
            this.db = new CrafterContext();
        }
        //
        // GET: /Admin/Admin/
        //[Authorize(Roles="Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCategories([DataSourceRequest] DataSourceRequest request)
        {
            var categories = this.db.Categories.Select(CategoryDisplayModel.FromCategory).ToList();

            return Json(categories, JsonRequestBehavior.AllowGet);
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
            var step = this.db.Steps.Include("Images").FirstOrDefault(st => st.Id == stepModel.Id);

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
                step.Content = stepModel.Content;
                //step.Images.Clear();

                //HashSet<Image> images = this.AddOrUpdateImages(stepModel.Images);

                //foreach (var image in images)
                //{
                //    step.Images.Add(image);
                //}

                this.db.SaveChanges();
            }

            return Json(new[] { stepModel }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTutorials([DataSourceRequest] DataSourceRequest request)
        {
            var tutorials = this.db.Tutorials.Select(TutorialDisplayModel.FromTutorial);    
        
            return Json(tutorials.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<string> GetFileInfo(IEnumerable<HttpPostedFileBase> files)
        {
            return
                from a in files
                where a != null
                select string.Format("{0} ({1} bytes)", Path.GetFileName(a.FileName), a.ContentLength);
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> attachments)
        {
            System.Diagnostics.Trace.WriteLine("SavinG FiLe HeRe");
            // The Name of the Upload component is "attachments"
            string imageLocation = string.Empty;
            foreach (var file in attachments)
            {
                // Some browsers send file names with full path. We only care about the file name.
                var fileName = Path.GetFileName(file.FileName);
                var destinationPath = Path.Combine(Server.MapPath("~/Uploaded_Files"), fileName);
                imageLocation = "/Uploaded_Files/" + fileName;
                file.SaveAs(destinationPath);
            }

            var context = new CrafterContext();
            var user = context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            user.ImageUrl = imageLocation;
            context.SaveChanges();

            // Return an empty string to signify success
            return Content("");
        }



        public JsonResult CreateTutorial([DataSourceRequest] DataSourceRequest request, TutorialCreateModel tutorialModel)
        {
            var category = this.db.Categories.FirstOrDefault(c => c.Id == tutorialModel.CategoryId);
            var user = this.db.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

            //var files = tutorialModel.Image != null ? GetFileInfo(tutorialModel.Image) : null;

            TutorialDisplayModel result = new TutorialDisplayModel();

            if (tutorialModel != null)//&&user != null)
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

                //string imageLocation = string.Empty;
                //foreach (var file in tutorialModel.attachments)
                //{
                //    // Some browsers send file names with full path. We only care about the file name.
                //    var fileName = Path.GetFileName(file.FileName);
                //    var destinationPath = Path.Combine(Server.MapPath("~/Uploaded_Files/"), fileName);
                //    imageLocation = "/Uploaded_Files/" + fileName;
                //    file.SaveAs(destinationPath);
                //}

                //newTutorial.Image = imageLocation;

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
                tutorial.Category = category;
                this.db.SaveChanges();

                model.Category = category.Name;
                model.CategoryId = category.Id;
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }
	}
}