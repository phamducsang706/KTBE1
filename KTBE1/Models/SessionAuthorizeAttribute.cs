using System;
using System.Web;
using System.Web.Mvc;

namespace KTBE1.Models
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        public bool AdminOnly { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var username = httpContext.Session["Username"] as string;
            if (string.IsNullOrEmpty(username)) return false;
            if (AdminOnly) return Convert.ToBoolean(httpContext.Session["IsAdmin"] ?? false);
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            string returnUrl = HttpUtility.UrlEncode(filterContext.HttpContext.Request.RawUrl);
            filterContext.Result = new RedirectResult("~/Account/Login?returnUrl=" + returnUrl);
        }
    }
}