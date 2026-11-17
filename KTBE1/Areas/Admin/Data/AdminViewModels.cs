using System;
using System.Collections.Generic;
using KTBE1.Models;

namespace KTBE1.Areas.Admin.Data
{
    public class OrderListViewModel
    {
        public List<Order> Orders { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string CustomerName { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Sort { get; set; }

        public string OrderDateText
        {
            get { return OrderDate.HasValue ? OrderDate.Value.ToString("yyyy-MM-dd") : null; }
        }
    }

    public class StatusCount
    {
        public string Status { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }

    public class TopProduct
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class DashboardViewModel
    {
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public decimal Revenue { get; set; }
        public List<StatusCount> ByStatus { get; set; }
        public List<TopProduct> TopProducts { get; set; }
    }
}