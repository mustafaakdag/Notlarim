using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Notlarim101.BusinessLayer;
using Notlarim101.Entity;
using Notlarim101.WebApp.Models;


namespace Notlarim101.WebApp.Controllers
{
    public class NoteController : Controller
    {
        private NoteManager nm = new NoteManager();
        private CategoryManager cm = new CategoryManager();
        private LikedManager lm = new LikedManager();


        // GET: Note
        public ActionResult Index()
        {
             List<Note> notes = nm.QList().Include("Category").Include("Owner").Where(
                x => x.Owner.Id ==CurrentSession.User.Id ).OrderByDescending(
                x => x.ModifiedOn).ToList();

             //List<Note> nots = nm.List(x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(x => x.ModifiedOn).ToList();

            return View(notes);
        }

        public ActionResult MyLikedNotes()
        {

            Stopwatch sw = new Stopwatch();
             sw.Start();
             
                var notes1 = lm.List(x => x.LikedUser.Id == CurrentSession.User.Id).Select(x => x.Note);
            
            
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string eTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            string aaa = eTime;
            
            sw.Start();
            var notes4 = lm.List(x => x.LikedUser.Id == CurrentSession.User.Id).Select(x => x.Note).ToList();
            sw.Stop();
            TimeSpan ts111 = sw.Elapsed;
            string eTime111 = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts111.Hours, ts111.Minutes, ts111.Seconds,
                ts111.Milliseconds / 10);

            string aaa111 = eTime111;


            sw.Start();
            var notes3= lm.QList(s => s.LikedUser.Id == CurrentSession.User.Id).Include("LikedUser").Include("Note").Select(x => x.Note).Include("Category")
                .Include("Owner").OrderByDescending(s => s.ModifiedOn);
            sw.Stop();
            TimeSpan ts1 = sw.Elapsed;
            string eTime1 = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts1.Hours, ts1.Minutes, ts1.Seconds,
                ts1.Milliseconds / 10);

            string aaa1 = eTime1;


            sw.Start();
            var notes = lm.QList().Include("LikedUser").Include("Note")
                .Where(s => s.LikedUser.Id == CurrentSession.User.Id).Select(x => x.Note).Include("Category")
                .Include("Owner").OrderByDescending(s => s.ModifiedOn);
            sw.Stop();
            TimeSpan ts11 = sw.Elapsed;
            string eTime11 = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts11.Hours, ts11.Minutes, ts11.Seconds,
                ts11.Milliseconds / 10);

            string aaa11 = eTime11;

            return View("Index", notes4);
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(s=>s.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Note/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                nm.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(s=>s.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Note dbNote = nm.Find(s => s.Id == note.Id);
                dbNote.IsDraft = note.IsDraft;
                dbNote.CategoryId = note.CategoryId;
                dbNote.Text = note.Text;
                dbNote.Title = note.Title;
               
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(s=>s.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = nm.Find(s=>s.Id==id);
            nm.Delete(note);
            return RedirectToAction("Index");
        }
    }
}
