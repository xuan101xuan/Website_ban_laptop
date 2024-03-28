using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTPShop.Models
{
    public class CartItem
    {
        public SanPham Product { get; set; }
        public Cart Cart { get; set; }
    }
}