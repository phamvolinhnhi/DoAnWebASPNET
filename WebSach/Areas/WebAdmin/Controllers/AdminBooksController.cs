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
using System.IO;

namespace WebSach.Areas.WebAdmin.Controllers
{
    public class AdminBooksController : Controller
    {
        private Model1 db = new Model1();

        // GET: WebAdmin/AdminBooks
        public async Task<ActionResult> Index()
        {
            var books = db.Books.Include(b => b.Categories).Include(b => b.User);
            return View(await books.ToListAsync());
        }

        // GET: WebAdmin/AdminBooks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = await db.Books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // GET: WebAdmin/AdminBooks/Create
        public ActionResult Create()
        {
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name");
            ViewBag.User_Name = new SelectList(db.User, "User_Name", "Full_Name");
            return View();
        }

        // POST: WebAdmin/AdminBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Book_Id,Title,Category_Id,Author,Create_at,Update_at,Avatar,View,Content,User_Name")] Books books, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    // Get the filename and extension of the uploaded file
                    var fileName = Path.GetFileName(file.FileName);
                    var extension = Path.GetExtension(fileName);

                    // Generate a unique filename for the uploaded file
                    var uniqueFileName = Guid.NewGuid().ToString() + extension;

                    // Save the uploaded file to the Content/img folder
                    var path = Path.Combine(Server.MapPath("~/Content/img"), uniqueFileName);
                    file.SaveAs(path);

                    // Set the Avatar property of the Books model to the filename of the uploaded file
                    books.Avatar = uniqueFileName;
                }


                db.Books.Add(books);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name", books.Category_Id);
            ViewBag.User_Name = new SelectList(db.User, "User_Name", "Full_Name", books.User_Name);
            return View(books);
        }


        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }

        // GET: WebAdmin/AdminBooks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = await db.Books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name", books.Category_Id);
            ViewBag.User_Name = new SelectList(db.User, "User_Name", "Full_Name", books.User_Name);
            return View(books);
        }

        // POST: WebAdmin/AdminBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Book_Id,Title,Category_Id,Author,Create_at,Update_at,Avatar,View,Content,User_Name")] Books books)
        {
            if (ModelState.IsValid)
            {
                db.Entry(books).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Name", books.Category_Id);
            ViewBag.User_Name = new SelectList(db.User, "User_Name", "Full_Name", books.User_Name);
            return View(books);
        }

        // GET: WebAdmin/AdminBooks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = await db.Books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // POST: WebAdmin/AdminBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Books books = await db.Books.FindAsync(id);
            db.Books.Remove(books);
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
