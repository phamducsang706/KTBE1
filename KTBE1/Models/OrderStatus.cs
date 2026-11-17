using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTBE1.Models
{
    public static class OrderStatus
    {
        public const string Pending = "Chờ thanh toán";
        public const string Processing = "Đang xử lý";
        public const string Shipping = "Đang vận chuyển";
        public const string Delivered = "Đã giao";
        public const string Cancelled = "Đã hủy";

        public static readonly string[] All = { Pending, Processing, Shipping, Delivered, Cancelled };

        public static string BadgeClass(string s)
        {
            switch (s)
            {
                case Delivered: return "success";
                case Cancelled: return "danger";
                case Pending: return "warning text-dark";
                case Processing: return "primary";
                case Shipping: return "info text-dark";
                default: return "secondary";
            }
        }
    }
}