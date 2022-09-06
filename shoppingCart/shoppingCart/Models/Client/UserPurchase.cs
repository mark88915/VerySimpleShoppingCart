using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace shoppingCart.Models.Client
{
    public class UserPurchase : Models.Product.Product
    {
        /// <summary>
        /// 購買數量
        /// </summary>
        [Display(Name = "購買數量")]
        public int PurchasedAmount { get; set; }
    }
}