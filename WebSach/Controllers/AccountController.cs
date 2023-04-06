using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebSach.Models;

namespace WebSach.Controllers
{
    public class AccountController : Controller
    {
        private WebBookDb _db = new WebBookDb();

        // GET: Account
        public ActionResult Index(string id)
        {
            if (Session["UserName"] == null)
                return View("Login");
            if (String.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }
            User find = _db.User.FirstOrDefault(p => p.User_Name == id);
            if (find == null)
                return HttpNotFound();
            var listfollow = _db.Follow.Where(f=>f.userName== id);
            var listbook = new List<Books>();
            foreach(var item in listfollow)
            {
                var book = _db.Books.FirstOrDefault(b => b.Book_Id == item.bookId);
                listbook.Add(book);
            }
            var viewModel = new UserViewModel
            {
                user = find,
                follows = listbook
            };
            return View(viewModel);
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
                var data = _db.User.Where(s => s.User_Name.Equals(_user.User_Name) && s.Password.Equals(f_password) && s.Permission_Id == false).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["UserName"] = data.FirstOrDefault().User_Name;
                    data.FirstOrDefault().Last_Login = DateTime.Now;
                    return RedirectToAction("Index", "Home");
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

        //GET: Register
        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                if (_user.Password == form["confirmpassword"])
                {
                    var check = _db.User.FirstOrDefault(s => s.User_Name == _user.User_Name);
                    if (check == null)
                    {
                        User newUser = new User
                        {
                            User_Name = _user.User_Name,
                            Full_Name = _user.Full_Name,
                            Email = _user.Email,
                            Password = GetMD5(_user.Password),
                            Create_at = DateTime.Now,
                            Permission_Id = false,
                            Avatar = "/Images/Users/default-avatar.png"
                        };

                        _db.Configuration.ValidateOnSaveEnabled = false;
                        _db.User.Add(newUser);
                        _db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.error = "Username already exists";
                        return View();
                    }
                }
                else
                {
                    ViewBag.error = "Confirm password doesn't match!";
                    return View();
                }
            }
            return View();
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Images/Users/" + file.FileName));
            return "~/Images/Users/" + file.FileName;
        }

        public ActionResult Follow(int? page)
        {
            page = page ?? 1;
            int pageSize = 24;
            if (Session["UserName"]  == null)
            {
                return View("Login");
            }
            string name = Session["UserName"].ToString();

            var list = _db.Follow.Where(L => L.userName == name).ToList();
            var books = new List<Books>();
            foreach (var item in list) {
                var find = _db.Books.FirstOrDefault(L => L.Book_Id == item.bookId);
                books.Add(find);
            }
            return View(books.ToPagedList(page.Value, pageSize));
        }
    }
}