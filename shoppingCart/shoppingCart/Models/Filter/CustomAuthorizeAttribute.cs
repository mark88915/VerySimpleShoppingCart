using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shoppingCart.Models.Filter
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 登入驗證，若Session中的UserId沒有吃到東西就return false並執行HandleUnauthorizedRequest()
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if(httpContext.Session["UserId"] == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 驗證失敗時執行
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Client/LoginPage");
        }
    }
}