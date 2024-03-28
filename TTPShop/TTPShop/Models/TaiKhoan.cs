namespace TTPShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            Carts = new HashSet<Cart>();
            DonHangs = new HashSet<DonHang>();
            DonHangs1 = new HashSet<DonHang>();
        }

        [Key]
        public int MaTK { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập địa chỉ email!")]
        [StringLength(50, ErrorMessage = "Địa chỉ email tối đa 50 ký tự!")]
        public string DiaChiEmail { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [StringLength(20, ErrorMessage = "Mật khẩu tối đa 20 ký tự!")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên!")]
        [StringLength(40, ErrorMessage = "Họ tên tối đa 40 ký tự!")]
        public string HoTen { get; set; }

        [StringLength(30)]
        public string VaiTro { get; set; }

        [StringLength(4)]
        public string GioiTinh { get; set; }

        public DateTime? NgaySinh { get; set; }

        [StringLength(20)]
        public string SoDienThoai { get; set; }

        public int? TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs1 { get; set; }
    }
}
