using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using KTBE1.Areas.Admin.Data;
using KTBE1.Models;

namespace KTBE1.Areas.Admin.Controllers
{
    [SessionAuthorize(AdminOnly = true)]
    public class OrdersController : Controller
    {
        private const int PageSize = 15;
        private KTBEEntities db = new KTBEEntities();

        // Danh sách: tìm kiếm + sắp xếp + phân trang 15 đơn/trang
        public ActionResult Index(string customerName, string paymentStatus, DateTime? orderDate, string sort, int page = 1)
        {
            IQueryable<Order> q = db.Order.Include(o => o.Customer);

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                string name = customerName.Trim();
                q = q.Where(o => o.Customer.CustomerName.Contains(name));
            }
            if (!string.IsNullOrEmpty(paymentStatus))
                q = q.Where(o => o.PaymentStatus == paymentStatus);
            if (orderDate.HasValue)
            {
                DateTime from = orderDate.Value.Date;
                DateTime to = from.AddDays(1);
                q = q.Where(o => o.OrderDate >= from && o.OrderDate < to);
            }

            sort = string.IsNullOrEmpty(sort) ? "date_desc" : sort;
            switch (sort)
            {
                case "date_asc": q = q.OrderBy(o => o.OrderDate).ThenBy(o => o.OrderID); break;
                case "name_asc": q = q.OrderBy(o => o.Customer.CustomerName).ThenBy(o => o.OrderID); break;
                case "name_desc": q = q.OrderByDescending(o => o.Customer.CustomerName).ThenBy(o => o.OrderID); break;
                case "total_asc": q = q.OrderBy(o => o.TotalAmount).ThenBy(o => o.OrderID); break;
                case "total_desc": q = q.OrderByDescending(o => o.TotalAmount).ThenBy(o => o.OrderID); break;
                default:
                    sort = "date_desc";
                    q = q.OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.OrderID);
                    break;
            }

            int total = q.Count();
            int totalPages = (int)Math.Ceiling(total / (double)PageSize);
            if (page < 1) page = 1;
            if (totalPages > 0 && page > totalPages) page = totalPages;

            var vm = new OrderListViewModel
            {
                Orders = q.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                Page = page,
                TotalPages = totalPages,
                TotalItems = total,
                CustomerName = customerName,
                PaymentStatus = paymentStatus,
                OrderDate = orderDate,
                Sort = sort
            };
            return View(vm);
        }

        // Chi tiết đơn hàng
        public ActionResult DetailedOrder(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var order = db.Order
                .Include(o => o.Customer)
                .Include(o => o.OrderDetail.Select(d => d.Product))
                .FirstOrDefault(o => o.OrderID == id);
            if (order == null) return HttpNotFound();
            return View(order);
        }

        [HttpGet]
        public ActionResult UpdateStatus(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var order = db.Order.Include(o => o.Customer).FirstOrDefault(o => o.OrderID == id);
            if (order == null) return HttpNotFound();
            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, string paymentStatus)
        {
            var order = db.Order.Include(o => o.Customer).FirstOrDefault(o => o.OrderID == id);
            if (order == null) return HttpNotFound();

            if (!OrderStatus.All.Contains(paymentStatus))
            {
                ModelState.AddModelError("", "Trạng thái không hợp lệ.");
                return View(order);
            }

            order.PaymentStatus = paymentStatus;
            db.SaveChanges();
            TempData["Msg"] = "Cập nhật trạng thái đơn hàng thành công.";
            return RedirectToAction("DetailedOrder", new { id = id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}