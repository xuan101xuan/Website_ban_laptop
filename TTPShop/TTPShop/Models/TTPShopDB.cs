namespace TTPShop.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TTPShopDB : DbContext
    {
        public TTPShopDB()
            : base("name=TTPShopDB")
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<ChiTietDH> ChiTietDHs { get; set; }
        public virtual DbSet<DanhMuc> DanhMucs { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<LienHe> LienHes { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DanhMuc>()
                .HasMany(e => e.SanPhams)
                .WithRequired(e => e.DanhMuc)
                .HasForeignKey(e => e.MaDM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DanhMuc>()
                .HasMany(e => e.SanPhams1)
                .WithRequired(e => e.DanhMuc1)
                .HasForeignKey(e => e.MaDM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DonHang>()
                .HasMany(e => e.ChiTietDHs)
                .WithRequired(e => e.DonHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LienHe>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<LienHe>()
                .Property(e => e.DienThoai)
                .IsUnicode(false);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.SanPham)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SanPham>()
                .HasMany(e => e.ChiTietDHs)
                .WithRequired(e => e.SanPham)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.VaiTro)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.Carts)
                .WithRequired(e => e.TaiKhoan)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.DonHangs)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.MaTK);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.DonHangs1)
                .WithOptional(e => e.TaiKhoan1)
                .HasForeignKey(e => e.MaTK);
        }
    }
}
