using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.App_Start
{
    public class RoleAuthorizationAttribute :AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Kiểm tra nếu session role có tồn tại
            if (httpContext.Session["role"] == null)
            {
                return false; // Không cho phép truy cập
            }
            return true; // Cho phép truy cập
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Nếu không được phép, chuyển hướng về trang Login
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary(
                    new { controller = "Accounts", action = "Login" }
                ));
        }
    }
}