using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shoppingCart.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 選擇何種登入方式
        /// Admin or Client
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}