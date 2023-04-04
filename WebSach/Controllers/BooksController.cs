using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSach.Models;

namespace WebSach.Controllers
{
    public class BooksController : Controller
    {
        private readonly WebBookDb _db;
        public BooksController()
        {
            _db = new WebBookDb();
        }

        // GET: Books
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Books books = FindBookById(id.Value);
            if (books == null)
            {
                return HttpNotFound();
            }
            var booksviewmodel = new BooksViewModel
            {
                books = FindBookById(id.Value),
                Chapters = _db.Chapter.Where(c => c.Book_Id == id.Value).ToList(),
                Comments = _db.Comment.Where(c => c.Book_Id == id.Value).ToList(),
            };
            return View(booksviewmodel);
        }

        public List<Books> GetAll() => _db.Books.ToList();
        public List<Books> GetAll(string searchKey)
        {
            searchKey = searchKey ?? "";
            return _db.Books.Where(p => p.Title.Contains(searchKey) || p.Author.Contains(searchKey)).ToList();
        }

        public Books FindBookById(int id)
        {
            return _db.Books.FirstOrDefault(p => p.Book_Id == id);
        }
        public ActionResult Chapter(int? bookid, int? id)
        {
            if (id == null || bookid == null)
            {
                return HttpNotFound();
            }
            Chapter chapter = _db.Chapter.Where(c => c.Chapter_Id == id.Value && c.Book_Id == bookid).FirstOrDefault();

            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        public void Comment(BooksViewModel viewModel)
        {

        }
    }
}