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
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Windows;
using System.Windows.Forms;

namespace WebSach.Areas.WebAdmin.Controllers
{
    public class AdminUsersController : Controller
    {
        private WebBookDb db = new WebBookDb();

        // GET: WebAdmin/AdminUsers
        public async Task<ActionResult> Index()
        {
            if (Session["Admin"] == null)
                return View("Login");
            return View(await db.User.ToListAsync());
        }

        // GET: WebAdmin/AdminUsers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (Session["Admin"] == null)
                return View("Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: WebAdmin/AdminUsers/Create
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
                return View("Login");
            return View();
        }

        // POST: WebAdmin/AdminUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "User_Name,Full_Name,Email,Password,Create_at,Last_Login,Status,Permission_Id")] User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: WebAdmin/AdminUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (Session["Admin"] == null)
                return View("Login");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: WebAdmin/AdminUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "User_Name,Full_Name,Email,Password,Create_at,Last_Login,Status,Permission_Id")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: WebAdmin/AdminUsers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (Session["Admin"] == null)
                return View("Login", "AdminUsers");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: WebAdmin/AdminUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            User user = await db.User.FindAsync(id);
            var find = db.Books.Where(f => f.User_Name == user.User_Name);
            if (find != null)
            {
                MessageBox.Show("Người dùng có liên kết với những dữ liệu khác. Hãy xóa dữ liệu của người dùng trước","Alert");
            }
            else
            {
                db.User.Remove(user);
                await db.SaveChangesAsync();
            }
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

        // LOGIN
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User _user)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(_user.
                    Password);
                var data = db.User.Where(s => s.User_Name.Equals(_user.User_Name) && s.Password.Equals(f_password) && s.Permission_Id == true).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["Admin"] = data.FirstOrDefault().User_Name;
                    data.FirstOrDefault().Last_Login = DateTime.Now;
                    return RedirectToAction("Index", "HomeAdmin");
                }
                else
                {
                    ViewBag.error = "Invalid username or password!";
                    return View("Login");
                }
            }
            else
            {
                ViewBag.error = "Cannot login";
                return View();

            }
        }
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index", "Home");
        }



        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}
