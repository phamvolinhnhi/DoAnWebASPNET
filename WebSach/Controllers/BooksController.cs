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
            Books books = _db.Books.Where(c => c.Book_Id == id).FirstOrDefault();
            if (books == null)
            {
                return View("Index", "Home");
            }
            BooksViewModel viewModel = new BooksViewModel();
            if (Session["UserName"] != null)
            {
                string name = Session["UserName"].ToString();
                var find = _db.Follow.FirstOrDefault(f => f.bookId == id && f.userName == name);
                if (find == null)
                    books.isFollowing = false;
                else books.isFollowing = true;
                var findi = _db.ReadHistory.FirstOrDefault(c=>c.UserName == name && c.BookId == id);
                if (findi != null)
                    viewModel.chapterid = findi.ChapId;
            }
            viewModel.book = books;
            viewModel.Chapters = _db.Chapter.Where(c => c.Book_Id == id).ToList();
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
                return View("Index", "Home");
            }

            Chapter chapter = _db.Chapter.Where(c => c.Chapter_Id == id.Value && c.Book_Id == bookid).FirstOrDefault();
            if (chapter == null)
            {
                return HttpNotFound();
            }
            var bookf = _db.Books.FirstOrDefault(b => b.Book_Id == bookid);
            bookf.View += 1;
            var viewModel = new BooksViewModel
            {
                book = bookf,
                Chapters = _db.Chapter.Where(c => c.Book_Id == bookid).ToList(),
                chapter = chapter
            };
            if (Session["UserName"] != null)
            {
                string name = Session["UserName"].ToString();
                var find = _db.ReadHistory.FirstOrDefault(f => f.BookId == bookid && f.UserName == name);
                if (find == null)
                {
                    find = new ReadHistory
                    {
                        BookId = bookid.Value,
                        UserName = Session["UserName"].ToString(),
                        Time = DateTime.Now,
                        ChapId = id.Value,
                    };
                    _db.ReadHistory.Add(find);
                }
                else
                {
                    find.Time = DateTime.Now;
                    find.ChapId = id.Value;
                }
                _db.SaveChanges();
            }
            return View(viewModel);
        }

        public void Follow(int? bookId)
        {
            var book = new Books();
            if (Session["UserName"] != null)
            {
                string name = Session["UserName"].ToString();
                var find = _db.Follow.FirstOrDefault(f => f.bookId == bookId && f.userName == name);
                if (find == null)
                {
                    find = new Follow
                    {
                        userName = name,
                        bookId = bookId.Value
                    };
                    _db.Follow.Add(find);
                }
                else
                {
                    _db.Follow.Remove(find);
                }
                _db.SaveChanges();
            }
        }

    }
}