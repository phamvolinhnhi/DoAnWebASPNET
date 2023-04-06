using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
        public ActionResult Index(int? page, string search, int? type)
        {
            page = page ?? 1;
            int pageSize = 24;
            if (type == null)
            {
                search = search ?? "";
                ViewBag.Keyword = search;
                var Books = GetAll(search).ToPagedList(page.Value, pageSize);
                return View(Books);
            }
            else
            {
                var Books = GetAll(type).ToPagedList(page.Value, pageSize);
                return View(Books);
            }
        }

        public List<Books> GetAll() => _db.Books.ToList();
        public List<Books> GetAll(string searchKey)
        {
            return _db.Books.Where(p => p.Title.Contains(searchKey)).ToList();
        }
        public List<Books> GetAll(int? type)
        {
            return _db.Books.Where(p => p.Category_Id == type).ToList();
        }

        public List<Books> GetAllOrderByView()
        {
            return _db.Books.OrderBy(c => c.View).ToList();
        }
        public List<Books> GetAllOrderByDate()
        {
            return _db.Books.OrderBy(c => c.View).ToList();
        }


        //public List<Books> GetAll(int? type)
        //{
        //    type = type ?? 0;
        //    if(type == 0)
        //    {
        //        _db.Books.ToList();
        //    }
        //    else if(type == 1)
        //    {
        //        _db.Books.ToList().OrderBy(c=>c.Create_at);
        //    }
        //    else if(type == 1)
        //    {
        //        _db.Books.ToList().OrderBy(c=>c.Create_at);
        //    }
        //}

        public ActionResult SachMoi(int? page)
        {
            page = page ?? 1;
            int pageSize = 24;
            return View(GetAllOrderByDate().ToPagedList(page.Value, pageSize));
        }
        public ActionResult XepHang(int? page)
        {
            page = page ?? 1;
            int pageSize = 24;
            return View(GetAllOrderByView().ToPagedList(page.Value, pageSize));
        }

        public Books FindBookById(int id)
        {
            return _db.Books.FirstOrDefault(p => p.Book_Id == id);
        }

        public ActionResult Category()
        {
            var Category = _db.Categories.ToList();
            return PartialView("_Category", Category);
        }
        public ActionResult TopBooks()
        {
            var topBooks = _db.Books.OrderByDescending(b => b.View).Take(3).ToList();
            return PartialView("_TopBooks", topBooks);
        }
    }
}