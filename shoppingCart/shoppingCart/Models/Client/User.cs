using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace shoppingCart.Models.Client
{
    public class User
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "只能輸入英文字母及數字")]
        [Display(Name = "帳號")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "只能輸入英文字母及數字")]
        [Display(Name = "密碼")]
        public string Password { get; set; }

    }
}