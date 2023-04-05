using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSach.Models;

namespace WebSach.Areas.WebAdmin.Controllers
{
    public class AdminChaptersController : Controller
    {
        private Model1 db = new Model1();

        // GET: WebAdmin/AdminChapters
        public async Task<ActionResult> Index()
        {
            var chapter = db.Chapter.Include(c => c.Books);
            return View(await chapter.ToListAsync());
        }

        // GET: WebAdmin/AdminChapters/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapter.FirstOrDefault(c => c.Chapter_Id == id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // GET: WebAdmin/AdminChapters/Create
        public ActionResult Create()
        {
            ViewBag.Book_Id = new SelectList(db.Books, "Book_Id", "Title");
            return View();
        }

        // POST: WebAdmin/AdminChapters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Chapter_Id,Book_Id,Chapter_Name,Content")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Chapter.Add(chapter);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Book_Id = new SelectList(db.Books, "Book_Id", "Title", chapter.Book_Id);
            return View(chapter);
        }


        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/ContentChapter/" + file.FileName));
            return "/Content/ContentChapter/" + file.FileName;
        }


        // GET: WebAdmin/AdminChapters/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapter.FirstOrDefault(c => c.Chapter_Id == id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            ViewBag.Book_Id = new SelectList(db.Books, "Book_Id", "Title", chapter.Book_Id);
            return View(chapter);
        }

        // POST: WebAdmin/AdminChapters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Chapter_Id,Book_Id,Chapter_Name,Content")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chapter).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Book_Id = new SelectList(db.Books, "Book_Id", "Title", chapter.Book_Id);
            return View(chapter);
        }

        // GET: WebAdmin/AdminChapters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapter.FirstOrDefault(c=>c.Chapter_Id== id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // POST: WebAdmin/AdminChapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Chapter chapter = await db.Chapter.FindAsync(id);
            db.Chapter.Remove(chapter);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
