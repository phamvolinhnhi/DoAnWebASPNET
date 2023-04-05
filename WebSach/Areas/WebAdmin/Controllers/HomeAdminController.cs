using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebSach.Models;

namespace WebSach.Areas.WebAdmin.Controllers
{
    [Route("api/userdata")]

    public class HomeAdminController : Controller
    {
        private Model1 db = new Model1();
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


    }
}