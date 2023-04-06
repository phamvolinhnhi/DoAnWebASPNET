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

        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return View("Index", "Home");
            }
            Books books = _db.Books.SingleOrDefault(c => c.Book_Id == id);
            if (books == null)
            {
                return View("Index", "Home");
            }
            if (Session["UserName"] != null)
            {
                string name = Session["UserName"].ToString();
                var find = _db.Follow.FirstOrDefault(f => f.bookId == id && f.userName == name);
                if (find == null)
                    books.isFollowing = false;
                else books.isFollowing = true;
            }
            BooksViewModel viewModel = new BooksViewModel
            {
                book = books,
                Chapters = _db.Chapter.Where(c => c.Book_Id == id).ToList(),
            };

            return View(viewModel);

        }

        public List<Books> GetAll() => _db.Books.ToList();
        public List<Books> GetAll(string searchKey)
        {
            searchKey = searchKey ?? "";
            return _db.Books.Where(p => p.Title.Contains(searchKey) || p.Author.Contains(searchKey)).ToList();
        }

        public Books FindBookById(int id)
        {
            return _db.Books.SingleOrDefault(c => c.Book_Id == id);
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
            var viewModel = new BooksViewModel
            {
                book = _db.Books.FirstOrDefault(b => b.Book_Id == bookid),
                Chapters = _db.Chapter.Where(c => c.Book_Id == bookid).ToList(),
                chapter = chapter
            };
            return View(viewModel);
        }

        public void Follow(int bookId)
        {
            string name = Session["UserName"].ToString();
            var find = _db.Follow.FirstOrDefault(f => f.bookId == bookId && f.userName == name);
            if (find == null)
            {
                var follow = new Follow
                {
                    userName = name,
                    bookId = bookId
                };
                _db.Follow.Add(follow);
            }
            else
            {
                _db.Follow.Remove(find);
            }
            _db.SaveChanges();
        }

    }
}