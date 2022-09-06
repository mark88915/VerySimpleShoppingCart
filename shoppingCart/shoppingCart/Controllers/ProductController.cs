using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shoppingCart.Models.Product;


namespace shoppingCart.Controllers
{
    public class ProductController : Controller
    {
        readonly private ProductService productService = new ProductService();

        /// <summary>
        /// 首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<Product> model = productService.GetAllProduct();

            return View("Index", model);
        }

        /// <summary>
        /// 新增頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 新增資料進DB
        /// </summary>
        /// <param name="newProductData">新增的資料</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Product newProductData)
        {
            ViewBag.IsCreateSuccess = false;

            if (ModelState.IsValid)
            {
                productService.AddProduct(newProductData);
                ModelState.Clear();
                ViewBag.IsCreateSuccess = true;
            }

            return View();
        }

        /// <summary>
        /// 刪除DB中的資料
        /// </summary>
        /// <param name="productName">作為刪除依據的商品名稱</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string productId)
        {
            productService.DeleteProduct(productId);
            return Json(true);
        }

        /// <summary>
        /// 開啟商品更新頁面
        /// </summary>
        /// <param name="productName">作為查詢要更新的商品原始資料的依據</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Update(string productId)
        {
            Product model = productService.GetUpdateProductData(productId);
            return View(model);
        }

        /// <summary>
        /// 更新商品資料
        /// </summary>
        /// <param name="updatedProductData">要更新的資料</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(Product updatedProductData)
        {
            ViewBag.IsUpdateSuccess = false;

            if (ModelState.IsValid)
            {
                productService.UpdateProductData(updatedProductData);
                ViewBag.IsUpdateSuccess = true;
            }

            return View(updatedProductData);
        }
    }
}