namespace TTPShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cart")]
    public partial class Cart
    {
        [Key]
        public int IDCart { get; set; }

        public int MaTK { get; set; }

        public int quantity { get; set; }

        public int MaSP { get; set; }

        public virtual SanPham SanPham { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
