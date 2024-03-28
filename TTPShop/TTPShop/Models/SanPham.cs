namespace TTPShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            Carts = new HashSet<Cart>();
            ChiTietDHs = new HashSet<ChiTietDH>();
        }

        [Key]
        public int MaSP { get; set; }

        public int MaDM { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập tên sản phẩm!")]
        [StringLength(100)]
        public string TenSP { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập giá bán!")]
        public int GiaBan { get; set; }

        [StringLength(100)]
        public string AnhSP { get; set; }

        public string MoTa { get; set; }

        public int? PhanTramKM { get; set; }

        public DateTime NgayTao { get; set; }

        public int? SoLuong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDH> ChiTietDHs { get; set; }

        public virtual DanhMuc DanhMuc { get; set; }

        public virtual DanhMuc DanhMuc1 { get; set; }
    }
}
