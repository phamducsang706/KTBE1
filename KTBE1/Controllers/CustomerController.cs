using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using KTBE1.Models;

namespace KTBE1.Controllers
{
    [SessionAuthorize]
    public class CustomerController : Controller
    {
        private KTBEEntities db = new KTBEEntities();

        public ActionResult MyOrder(string keyword, string tab, bool more = false)
        {
            string username = (string)Session["Username"];
            var customer = db.Customer.FirstOrDefault(c => c.Username == username);

            var vm = new MyOrderViewModel
            {
                Keyword = keyword,
                Tab = tab ?? "",
                ShowAll = more,
                Groups = new List<OrderGroup>()
            };
            if (customer == null) return View(vm);

            var q = db.Order.Where(o => o.CustomerID == customer.CustomerID);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim();
                int id;
                if (int.TryParse(kw.TrimStart('#'), out id))
                    q = q.Where(o => o.OrderID == id
                                  || o.OrderDetail.Any(d => d.Product.ProductName.Contains(kw)));
                else
                    q = q.Where(o => o.OrderDetail.Any(d => d.Product.ProductName.Contains(kw)));
            }

            string[] statuses = vm.Tab == ""
                ? OrderStatus.All
                : OrderStatus.All.Where(s => s == vm.Tab).ToArray();

            // Mặc định mỗi trạng thái hiện 2 đơn gần nhất; bấm "Xem thêm" (tab cụ thể) thì tối đa 50
            int take = (more && vm.Tab != "") ? 50 : 2;

            foreach (string s in statuses)
            {
                string status = s;
                var sq = q.Where(o => o.PaymentStatus == status);
                int total = sq.Count();
                if (total == 0) continue;

                var list = sq.Include(o => o.OrderDetail.Select(d => d.Product))
                             .OrderByDescending(o => o.OrderDate)
                             .Take(take)
                             .ToList();

                vm.Groups.Add(new OrderGroup
                {
                    Status = status,
                    Orders = list,
                    Total = total,
                    HasMore = take == 2 && total > list.Count
                });
            }

            return View(vm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}