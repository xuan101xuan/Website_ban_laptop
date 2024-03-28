namespace TTPShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonHang")]
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            ChiTietDHs = new HashSet<ChiTietDH>();
        }

        [Key]
        public int SoDH { get; set; }

        public DateTime NgayDat { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập địa chỉ giao hàng!")]
        [StringLength(70)]
        public string DiaChiGH { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tổng tiền!")]
        public int TongTien { get; set; }
        
        public int? MaTK { get; set; }

        public int? TinhTrang { get; set; }

        public int? HinhThucTT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDH> ChiTietDHs { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        public virtual TaiKhoan TaiKhoan1 { get; set; }
    }
}
