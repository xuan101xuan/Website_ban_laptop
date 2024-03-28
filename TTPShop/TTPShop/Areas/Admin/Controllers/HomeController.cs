using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTPShop.Models;

namespace TTPShop.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private TTPShopDB db = new TTPShopDB();
        public ActionResult Index()
        {
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            List<TaiKhoan> taiKhoans = new List<TaiKhoan>();
            List<SanPham> sanPhams = new List<SanPham>();
            List<DanhMuc> danhMucs = new List<DanhMuc>();
            List<DonHang> donHangs = new List<DonHang>();
            try
            {
                taiKhoans = db.TaiKhoans.ToList();
                sanPhams = db.SanPhams.ToList();
                danhMucs = db.DanhMucs.ToList();
                donHangs = db.DonHangs.ToList();
            }catch(Exception e)
            {
                return View("ServerError", "Error");
            }
            ViewData["danhmuc"] = danhMucs.Count;
            ViewData["sanpham"] = sanPhams.Count;
            ViewData["taikhoan"] = taiKhoans.Count;
            ViewData["donhang"] = donHangs.Count;
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home", new { area = "" });
        }
    }
}