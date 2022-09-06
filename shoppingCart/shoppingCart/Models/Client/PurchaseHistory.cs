using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace shoppingCart.Models.Client
{
    public class PurchaseHistory
    {
        [Display(Name = "使用者Id")]
        public string UserId { get; set; }

        [Display(Name = "商品名稱")]
        public string ProductName { get; set; }

        [Display(Name = "購買數量")]
        public int PurchasedAmount { get; set; }

        [Display(Name = "購買日期")]
        public DateTime PurchaseDate { get; set; }
    }
}