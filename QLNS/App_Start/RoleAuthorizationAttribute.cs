using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNS.App_Start
{
    public class RoleAuthorizationAttribute :AuthorizeAttribute
    {
        private readonly string[] allowedRoles;

        public RoleAuthorizationAttribute(params string[] roles)
        {
            allowedRoles = roles; // Danh sách các role được phép truy cập
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Kiểm tra nếu session "role" không tồn tại
            var userRole = httpContext.Session["role"] as string;
            if (string.IsNullOrEmpty(userRole))
            {
                return false; // Không cho phép truy cập nếu role không tồn tại
            }

            // Kiểm tra role của người dùng có trong danh sách cho phép
            if (allowedRoles.Length > 0 && !allowedRoles.Contains(userRole))
            {
                return false; // Không cho phép nếu role không nằm trong danh sách
            }

            return true; // Cho phép truy cập nếu hợp lệ
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Chuyển hướng về trang Login nếu không có quyền
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary(
                    new { controller = "Accounts", action = "Login" }
                ));
        }

    }
}