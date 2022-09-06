using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace shoppingCart.Models.Product
{
    public class Product
    {
        /// <summary>
        /// 商品編號
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [Display(Name = "商品名稱")]
        [Required(ErrorMessage = "請輸入商品名稱")]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品費用
        /// </summary>
        [Display(Name = "商品價格")]
        [Required(ErrorMessage = "請輸入商品價格")]
        [RegularExpression("^([1-9][0-9]*)$", ErrorMessage = "商品價格只能輸入正數")]
        public int ProductPrice { get; set; }

        /// <summary>
        /// 庫存數量
        /// </summary>
        [Display(Name = "庫存數量")]
        [Required(ErrorMessage = "請輸入庫存數量")]
        [RegularExpression("^([1-9][0-9]*)$", ErrorMessage = "庫存數量只能輸入正數")]
        public int ProductInStock { get; set; }
    }
}