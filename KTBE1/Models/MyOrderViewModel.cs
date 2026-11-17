using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTBE1.Models
{
    public class OrderGroup
    {
        public string Status { get; set; }
        public List<Order> Orders { get; set; }
        public int Total { get; set; }
        public bool HasMore { get; set; }
    }

    public class MyOrderViewModel
    {
        public string Keyword { get; set; }
        public string Tab { get; set; }
        public bool ShowAll { get; set; }
        public List<OrderGroup> Groups { get; set; }
    }
}