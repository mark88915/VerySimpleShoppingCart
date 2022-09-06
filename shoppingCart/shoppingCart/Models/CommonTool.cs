using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shoppingCart.Models
{
    public class CommonTool
    {
        public string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
        }
    }
}