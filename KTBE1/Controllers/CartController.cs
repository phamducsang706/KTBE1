using System.Collections.Generic;
using System.Web.Mvc;
using KTBE1.Models;

namespace KTBE1.Controllers
{
    public class CartController : Controller
    {
        [HttpPost]
        [SessionAuthorize]
        public ActionResult Add(int productId, string returnUrl)
        {
            var cart = Session["Cart"] as Dictionary<int, int> ?? new Dictionary<int, int>();

            // Thêm với số lượng = 1 (nếu đã có trong giỏ thì cộng thêm 1)
            cart[productId] = cart.ContainsKey(productId) ? cart[productId] + 1 : 1;
            Session["Cart"] = cart;

            TempData["Msg"] = "Đã thêm sản phẩm vào giỏ hàng (số lượng: 1).";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("MyOrder", "Customer");
        }
    }
}