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
using System.Windows.Forms;
using System.Xml.Linq;
using WebSach.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WebSach.Controllers
{
    public class AccountController : Controller
    {
        private WebBookDb _db = new WebBookDb();

        // GET: Account
        public ActionResult Index(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return View("Login");
            }
            var find = _db.User.FirstOrDefault(p => p.User_Name == id);
            if (find == null)
                return HttpNotFound();
            var listfollow = _db.Follow.Where(f => f.userName == id);
            var listbook = new List<Books>();
            foreach (var item in listfollow)
            {
                var book = _db.Books.FirstOrDefault(b => b.Book_Id == item.bookId);
                listbook.Add(book);
            }
            var list = _db.ReadHistory.Where(L => L.UserName == id).OrderBy(l => l.Time).ToList();
            var books = new List<Books>();
            foreach (var item in list)
            {
                var findbook = _db.Books.FirstOrDefault(L => L.Book_Id == item.BookId);
                books.Add(findbook);
            }
            var viewModel = new UserViewModel
            {
                user = find,
                follows = listbook,
                histories = books
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
                var f_password = GetMD5(_user.Password);
                var data = _db.User.FirstOrDefault(s => s.User_Name.Equals(_user.User_Name) && s.Password.Equals(f_password) && s.Permission_Id == false);
                if (data != null)
                {
                    //add session
                    Session["UserName"] = data.User_Name;
                    data.Last_Login = DateTime.Now;
                    var login = new User_Login
                    {
                        UserName = _user.User_Name,
                        LoginTime = DateTime.Now,
                    };
                    _db.User_Login.Add(login);
                    _db.SaveChanges();
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
        public ActionResult Register(User _user, System.Web.Mvc.FormCollection form)
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
            return "/Images/Users/" + file.FileName;
        }

        public ActionResult Follow(int? page)
        {
            if (Session["UserName"] == null)
                return View("Login");
            page = page ?? 1;
            string name = Session["UserName"].ToString();
            int pageSize = 24;
            if (Session["UserName"] == null)
            {
                return View("Login");
            }
            var list = _db.Follow.Where(L => L.userName == name).ToList();
            var books = new List<Books>();
            foreach (var item in list)
            {
                var find = _db.Books.FirstOrDefault(L => L.Book_Id == item.bookId);
                books.Add(find);
            }
            return View(books.ToPagedList(page.Value, pageSize));
        }

        public ActionResult History(int? page)
        {
            if (Session["UserName"] == null)
                return View("Login");
            string name = Session["UserName"].ToString();
            page = page ?? 1;
            int pageSize = 24;
            var list = _db.ReadHistory.Where(L => L.UserName == name).OrderBy(l => l.Time).ToList();
            var books = new List<Books>();
            foreach (var item in list)
            {
                var find = _db.Books.FirstOrDefault(L => L.Book_Id == item.BookId);
                books.Add(find);
            }
            return View(books.ToPagedList(page.Value, pageSize));
        }

        public ActionResult Edit(string name)
        {
            if(name == null)
                return HttpNotFound();
            var find = _db.User.FirstOrDefault(f=>f.User_Name==name);
            if (find == null)
                return HttpNotFound();
            return View(find);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            var find = _db.User.FirstOrDefault(f => f.User_Name == user.User_Name);
            find.Full_Name = user.Full_Name;
            find.Avatar = user.Avatar;
            find.Email = user.Email;
            _db.SaveChanges();
            MessageBox.Show("Chỉnh sửa thành công");
            return View();
        }
    }
}