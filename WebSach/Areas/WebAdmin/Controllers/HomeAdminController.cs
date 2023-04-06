//using Microsoft.AspNetCore.Mvc;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebSach.Models;

namespace WebSach.Areas.WebAdmin.Controllers
{
    [Route("api/userdata")]

    public class HomeAdminController : Controller
    {
        private WebBookDb db = new WebBookDb();
        // GET: WebAdmin/HomeAdmin
        public ActionResult Index()
        {
            var userCounts = db.User
                .GroupBy(u => new { Month = u.Create_at.Value.Month, Year = u.Create_at.Value.Year })
                .Select(g => new { MonthYear = g.Key.Year + "/" + g.Key.Month, Count = g.Count() })
                .AsEnumerable()
                .Select(g => new { MonthYear = DateTime.ParseExact(g.MonthYear, "yyyy/M", CultureInfo.InvariantCulture), Count = g.Count })
                .OrderBy(g => g.MonthYear)
                .ToList();

            ViewBag.Months = userCounts.Select(uc => uc.MonthYear.ToString("yyyy/MM")).ToList();
            ViewBag.UserCounts = userCounts.Select(uc => uc.Count).ToList();

            var categories = db.Categories.ToList();
            var bookCounts = categories.Select(c => c.Books.Count()).ToList();
            ViewBag.Categories = categories.Select(c => c.Category_Id).ToList();
            var categoryNames = categories.Select(c => c.Category_Name).ToList();

            ViewBag.Categories = categoryNames;
            ViewBag.BookCounts = bookCounts;


            return View();
        }
        public ActionResult TheLoai(int? page, string search, int? type)
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
        public List<Books> GetAll() => db.Books.ToList();
        public List<Books> GetAll(string searchKey)
        {
            return db.Books.Where(p => p.Title.Contains(searchKey)).ToList();
        }
        public List<Books> GetAll(int? type)
        {
            return db.Books.Where(p => p.Category_Id == type).ToList();
        }

        public ActionResult TopBooks()
        {
            var topBooks = db.Books.OrderByDescending(b => b.View).Take(3).ToList();
            return PartialView("_TopBooks", topBooks);
        }

        public ActionResult Category()
        {
            var Category = db.Categories.ToList();
            return PartialView("_Category", Category);
        }

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

        public List<Books> GetAllOrderByView()
        {
            return db.Books.OrderBy(c => c.View).ToList();
        }
        public List<Books> GetAllOrderByDate()
        {
            return db.Books.OrderBy(c => c.View).ToList();
        }
    }
}