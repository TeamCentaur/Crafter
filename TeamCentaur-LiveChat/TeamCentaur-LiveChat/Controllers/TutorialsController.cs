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

        // GET: /Tutorials/Create
        public ActionResult Create()
        {
            return View();
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

        // GET: /Tutorials/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutorial tutorial = db.Tutorials.Find(id);
            if (tutorial == null)
            {
                return HttpNotFound();
            }
            return View(tutorial);
        }

        // POST: /Tutorials/Edit/5
		// To protect from over posting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		// 
		// Example: public ActionResult Update([Bind(Include="ExampleProperty1,ExampleProperty2")] Model model)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tutorial tutorial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutorial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tutorial);
        }

        // GET: /Tutorials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutorial tutorial = db.Tutorials.Find(id);
            if (tutorial == null)
            {
                return HttpNotFound();
            }
            return View(tutorial);
        }

        // POST: /Tutorials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tutorial tutorial = db.Tutorials.Find(id);
            db.Tutorials.Remove(tutorial);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
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
