using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shoppingCart.Models.Client;
using shoppingCart.Models.Product;
using shoppingCart.Models.Filter;

namespace shoppingCart.Controllers
{
    public class ClientController : Controller
    {
        readonly private UserService userService = new UserService();
        readonly private ProductService productService = new ProductService();
        readonly private UserProductService userProductService = new UserProductService();

        /// <summary>
        /// 登入畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginPage()
        {
            // 如果Session中有UserId就把頁面導向產品清單畫面
            if(Session["UserId"] != null)
            {
                return RedirectToAction("ProductList");
            }

            return View();
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="userData">使用者輸入的帳號密碼</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoginPage(User userData)
        {
            // 如果DB中有成對的帳號密碼則在Session中加入UserId並導向產品清單畫面
            if(ModelState.IsValid && userService.IsAccountExist(userService.GetAllUser(), userData))
            {
                Session["UserId"] = userData.UserId;
                return RedirectToAction("ProductList");
            }

            ModelState.AddModelError("Password", "輸入有誤，請重新確認帳號密碼");
            ModelState.Remove("Password");
            return View(userData);
        }

        /// <summary>
        /// 註冊畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="userData">使用者輸入的帳號密碼</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(User userData)
        {
            ViewBag.IsCreateSuccess = false;

            if (ModelState.IsValid)
            {
                userService.AddUser(userData);
                ModelState.Clear();
                ViewBag.IsCreateSuccess = true;
            }

            return View();
        }

        /// <summary>
        /// 商品清單畫面(登入成功後)
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult ProductList()
        {
            List<Product> productList = this.productService.GetAllProduct();
            return View(productList);
        }

        /// <summary>
        /// 新增商品至購物車
        /// </summary>
        /// <param name="productId">產品Id</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public ActionResult AddProductToShoppingCart(string productId)
        {
            string userId = Session["UserId"].ToString();

            // 若商品尚未加入購物車才可以加進去
            if (!userProductService.IsThisProductInThisUserShoppingCart(userId, productId))
            {
                userProductService.AddProductToShoppingCart(userId, productId);
                return Json(new { IsAddToShoppingCartSuccess = true });

            }

            return Json(new { IsAddToShoppingCartSuccess = false, ThisProductExistInShoppingCart = true });
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public ActionResult Logout()
        {
            Session.Remove("UserId");
            return RedirectToAction("LoginPage");
        }

        /// <summary>
        /// 使用者的購物車
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult ShoppingCart()
        {
            string userId = Session["UserId"].ToString();
            List<Product> shoppingCartList = userProductService.GetUserShoppingCart(userId);

            return View(shoppingCartList);
        }

        /// <summary>
        /// 將商品移出購物車
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public JsonResult RemoveFromShoppingCart(string productId)
        {
            string userId = Session["UserId"].ToString();
            userProductService.RemoveFromShoppingCart(userId, productId);

            return Json(new { IsRemoveSuccess = true });
        }

        /// <summary>
        /// 商品購買畫面
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult Purchase(string productId)
        {
            UserPurchase purchasedProductData = userProductService.GetPurchasedProductData(productId);
            return View(purchasedProductData);
        }

        /// <summary>
        /// 購買
        /// </summary>
        /// <param name="purchasedData">購買之產品資訊</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public ActionResult Purchase(UserPurchase purchasedData)
        {
            string userId = Session["UserId"].ToString();
            userProductService.Purchase(purchasedData);
            userProductService.AddPurchaseHistory(purchasedData, userId);

            ViewBag.IsPurchaseSuccess = true;

            return RedirectToAction("ProductList");
        }

        /// <summary>
        /// 購買歷史畫面
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult PurchaseHistory()
        {
            string userId = Session["UserId"].ToString();
            List<PurchaseHistory> purchasHistoryList = userProductService.GetUserPurchaseHistory(userId);
            return View(purchasHistoryList);
        }
    }
}