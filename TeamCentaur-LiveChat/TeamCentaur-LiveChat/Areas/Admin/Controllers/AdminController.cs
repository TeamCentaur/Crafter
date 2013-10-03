using Crafter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using TeamCentaur_LiveChat.Areas.Admin.ViewModels;

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

        public JsonResult GetSteps([DataSourceRequest] DataSourceRequest request, int id)
        {
            var tutorial = this.db.Tutorials.Include("Steps").FirstOrDefault(t => t.Id == id);
            var steps = tutorial.Steps.AsQueryable().Select(StepDisplayModel.FromStep);

            return Json(steps.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult DeleteSteps([DataSourceRequest] DataSourceRequest request, StepDisplayModel stepModel)
        //{
        //    var step = this.db.Steps.Include("Images").FirstOrDefault(st => st.Id == stepModel.Id);

        //    if (step != null)
        //    {

        //    }
        //}

        public JsonResult GetTutorials([DataSourceRequest] DataSourceRequest request)
        {
            var tutorials = this.db.Tutorials.Select(TutorialDisplayModel.FromTutorial);            
            return Json(tutorials.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTutorial([DataSourceRequest] DataSourceRequest request, TutorialDisplayModel model)
        {
            var tutorial = this.db.Tutorials.Include("Steps").Include("Comments").Include("Images").FirstOrDefault(t => t.Id == model.Id);

            if (tutorial != null)
            {
                this.db.Images.RemoveRange(tutorial.Images);

                foreach (var step in tutorial.Steps)
                {
                    this.db.Images.RemoveRange(step.Images);
                }

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