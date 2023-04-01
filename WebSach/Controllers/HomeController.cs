using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSach.Models;

namespace WebSach.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebBookDb _db;
        public HomeController()
        {
            _db = new WebBookDb();
        }
        public ActionResult Index(int? page)
        {
            page = page ?? 1;
            int pageSize = 4;
            return View(GetAll().ToPagedList(page.Value, pageSize));
        }

        public List<Books> GetAll() => _db.Books.ToList();
        public List<Books> GetAll(string searchKey)
        {
            return _db.Books.Where(p => p.Title.Contains(searchKey) || p.Author.Contains(searchKey)).ToList();
        }

        public Books FindBookById(int id)
        {
            return _db.Books.FirstOrDefault(p => p.Book_Id == id);
        }
    }
}