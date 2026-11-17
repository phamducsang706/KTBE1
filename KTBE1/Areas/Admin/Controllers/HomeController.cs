using System.Linq;
using System.Web.Mvc;
using KTBE1.Areas.Admin.Data;
using KTBE1.Models;

namespace KTBE1.Areas.Admin.Controllers
{
    [SessionAuthorize(AdminOnly = true)]
    public class HomeController : Controller
    {
        private KTBEEntities db = new KTBEEntities();

        public ActionResult Index()
        {
            var vm = new DashboardViewModel();
            vm.TotalOrders = db.Order.Count();
            vm.TotalCustomers = db.Customer.Count();
            vm.TotalProducts = db.Product.Count();

            // Doanh thu = tổng tiền các đơn đã giao
            vm.Revenue = db.Order.Where(o => o.PaymentStatus == OrderStatus.Delivered)
                                 .Select(o => (decimal?)o.TotalAmount).Sum() ?? 0;

            vm.ByStatus = db.Order.GroupBy(o => o.PaymentStatus)
                .Select(g => new StatusCount
                {
                    Status = g.Key,
                    Count = g.Count(),
                    Amount = g.Sum(x => x.TotalAmount)
                }).ToList();

            vm.TopProducts = db.OrderDetail.GroupBy(d => d.Product.ProductName)
                .Select(g => new TopProduct { Name = g.Key, Quantity = g.Sum(x => x.Quantity) })
                .OrderByDescending(t => t.Quantity)
                .Take(5)
                .ToList();

            return View(vm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}