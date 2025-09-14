using QuanLyKho.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyKho.Controllers
{
    public class AccountController : Controller
    {
        // Danh sách giả lập (sau này sẽ thay bằng DbContext)
        private static List<User> users = new List<User>
        {
            new User { UserId = 1, Username = "admin", Password = "123", Role = "Admin" },
            new User { UserId = 2, Username = "user", Password = "123", Role = "User" }
        };

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                Session["User"] = user;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
            return View();
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                // kiểm tra trùng username
                if (users.Any(u => u.Username == model.Username))
                {
                    ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                    return View(model);
                }

                model.UserId = users.Count + 1;
                users.Add(model);

                ViewBag.Success = "Đăng ký thành công! Hãy đăng nhập.";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // GET: Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
