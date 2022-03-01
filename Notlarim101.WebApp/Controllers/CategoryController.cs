using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Notlarim101.BusinessLayer;
using Notlarim101.Entity;
using Notlarim101.WebApp.Models;


namespace Notlarim101.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryManager cm = new CategoryManager();

        
        public ActionResult Index()
        {
            var cat = cm.IndexCat();
            return View(cat);
            //return View(cm.List());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = cm.Find(s=>s.Id==id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                cm.InsertCat(category);
                CacheHelper.RemoveCategoriesFromCache();
                //cm.Insert(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = cm.Find(s=>s.Id==id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                Category cat = cm.Find(s => s.Id == category.Id);
                cat.Title = category.Title;
                cat.Description = category.Description;
                cm.Update(cat);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = cm.Find(s=>s.Id==id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = cm.Find(s=>s.Id==id);

            cm.Delete(category);
            CacheHelper.RemoveCategoriesFromCache();
            return RedirectToAction("Index");
        }

    }
}
