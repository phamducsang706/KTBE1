using System;
using System.Linq;
using System.Web.Mvc;
using KTBE1.Models;

namespace KTBE1.Controllers
{
    public class AccountController : Controller
    {
        private KTBEEntities db = new KTBEEntities();

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(
            string username,
            string password,
            string customerName,
            string customerPhone,
            string customerEmail,
            string customerAddress)
        {
            // Kiểm tra dữ liệu bắt buộc
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(customerName))
            {
                ViewBag.Error = "Vui lòng nhập tên đăng nhập, mật khẩu và họ tên.";
                return View();
            }

            username = username.Trim();
            customerName = customerName.Trim();

            // Kiểm tra tên đăng nhập đã được sử dụng chưa
            bool usernameExists = db.User.Any(u => u.Username == username);
            if (usernameExists)
            {
                ViewBag.Error = "Tên đăng nhập này đã tồn tại.";
                return View();
            }

            // Tạo tài khoản khách hàng
            var user = new User
            {
                Username = username,
                Password = password,
                UserRole = "C" // cột UserRole chỉ dài 1 ký tự
            };

            var customer = new Customer
            {
                Username = username,
                CustomerName = customerName,
                CustomerPhone = customerPhone,
                CustomerEmail = customerEmail,
                CustomerAddress = customerAddress
            };

            db.User.Add(user);
            db.Customer.Add(customer);

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.Error = "Không thể tạo tài khoản. Hãy kiểm tra dữ liệu và thử lại.";
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            var user = db.User.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            string role = (user.UserRole ?? "").Trim();
            bool isAdmin = role.Equals("A", StringComparison.OrdinalIgnoreCase)
                        || role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            Session["Username"] = user.Username;
            Session["IsAdmin"] = isAdmin;

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            if (isAdmin)
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }

    }
}