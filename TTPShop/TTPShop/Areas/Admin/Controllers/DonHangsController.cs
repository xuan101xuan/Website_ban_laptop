using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTPShop.Models;

namespace TTPShop.Areas.Admin.Controllers
{
    public class DonHangsController : BaseController
    {
        private TTPShopDB db = new TTPShopDB();

        //TRẢ CHỦ ĐƠN HÀNG
        public ActionResult Index(string searchString, string currenFilter, int? page)
        {
            int pageSize = 0;
            int pageNumber = 0;
            //KIỂM TRA XEM CÓ TÌM KIẾM THEO TÊN KHÁCH HÀNG HAY KHÔNG
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currenFilter;
            }
            ViewBag.CurrentFilter = searchString;
            try
            {
                var donHangs = db.DonHangs.Select(t => t);
                if (!String.IsNullOrEmpty(searchString))
                {
                    donHangs = donHangs.Where(t => t.TaiKhoan.HoTen.Contains(searchString));
                    pageSize = donHangs.ToList().Count;
                    if(pageSize == 0)
                    {
                        pageSize = 1;
                    }
                    pageNumber = (page ?? 1);
                    donHangs = donHangs.OrderBy(dh => dh.TaiKhoan.HoTen);
                    return View(donHangs.ToPagedList(pageNumber, pageSize));
                }
                donHangs = donHangs.OrderBy(dh => dh.TaiKhoan.HoTen);
                //PHÂN TRANG
                pageSize = 5;
                pageNumber = (page ?? 1);
                return View(donHangs.ToPagedList(pageNumber, pageSize));
            }
            catch(Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
        }

        //CHI TIẾT ĐƠN HÀNG
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            DonHang donHang = new DonHang();
            try
            {
                donHang = db.DonHangs.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (donHang == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(donHang);
        }

        // TẠO ĐƠN HÀNG
        public ActionResult Create()
        {
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "DiaChiEmail");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoDH,NgayDat,DiaChiGH,TongTien,MaTK,TinhTrang,HinhThucTT")] DonHang donHang)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string check = validDonHang(donHang);
                    if (check.Length > 0)
                    {
                        ViewBag.Error = check;
                        ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "DiaChiEmail", donHang.MaTK);
                        return View(donHang);
                    }
                    donHang.NgayDat = DateTime.Now;
                    db.DonHangs.Add(donHang);
                    db.SaveChanges();
                    if(donHang.TinhTrang == 2)
                    {
                        //CẬP NHẬT SỐ LƯỢNG SẢN PHẨM KHI ĐƠN HÀNG ĐƯỢC GIAO
                        updateSoLuongSP(donHang.SoDH);
                    }
                    return RedirectToAction("Index");
                }

                ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "DiaChiEmail", donHang.MaTK);
                return View(donHang);
            }
            catch(Exception e)
            {
                ViewBag.Error = "Lỗi thêm: " + e.Message;
                ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "DiaChiEmail", donHang.MaTK);
                return View(donHang);
            }
            
        }

        //HÀM CẬP NHẬT SỐ LƯỢNG SẢN PHẨM KHI ĐƠN HÀNG ĐƯỢC GIAO
        public void updateSoLuongSP(int SoDH)
        {
            List<ChiTietDH> chiTietDHs = new List<ChiTietDH>();
            chiTietDHs = db.ChiTietDHs.Where(c => c.SoDH == SoDH).ToList();
            if(chiTietDHs.Count > 0)
            {
                foreach(ChiTietDH c in chiTietDHs)
                {
                    SanPham sanPham = db.SanPhams.FirstOrDefault(s => s.MaSP == c.MaSP);
                    if(sanPham != null)
                    {
                        if (sanPham.SoLuong > 1)
                        {
                            sanPham.SoLuong -= c.SoLuong;
                            db.Entry(sanPham).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        //HÀM KIỂM TRA TÍNH HỢP LỆ CỦA ĐƠN HÀNG
        public string validDonHang(DonHang donHang)
        {
            if (string.IsNullOrEmpty(donHang.DiaChiGH))
            {
                return "Vui lòng nhập địa chỉ giao hàng!";
            }
            if (donHang.TongTien < 0)
            {
                return "Tổng tiền không hợp lệ!";
            }
            if(donHang.TinhTrang < 0 || donHang.TinhTrang > 3)
            {
                return "Tình trạng đơn hàng không hợp lệ!";
            }
            if(donHang.HinhThucTT < 0 || donHang.HinhThucTT > 2)
            {
                return "Hình thức thanh toán không hợp lệ!";
            }

            return "";
        }

        //CHỈNH SỬA ĐƠN HÀNG
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            DonHang donHang = new DonHang();
            try
            {
                donHang = db.DonHangs.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (donHang == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "DiaChiEmail", donHang.MaTK);
            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoDH,NgayDat,DiaChiGH,TongTien,MaTK,TinhTrang,HinhThucTT")] DonHang donHang)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string check = validDonHang(donHang);
                    if(check.Length != 0)
                    {
                        ViewBag.Error = check;
                        ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "DiaChiEmail", donHang.MaTK);
                        return View(donHang);
                    }
                    donHang.NgayDat = DateTime.Now;
                    db.Entry(donHang).State = EntityState.Modified;
                    db.SaveChanges();
                    if (donHang.TinhTrang == 2)
                    {
                        //CẬP NHẬT SỐ LƯỢNG SẢN PHẨM
                        updateSoLuongSP(donHang.SoDH);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "DiaChiEmail", donHang.MaTK);
                    return View(donHang);
                }
            }
            catch(Exception e)
            {
                ViewBag.Error = "Lỗi thêm: " + e.Message;
                ViewBag.MaTK = new SelectList(db.TaiKhoans, "MaTK", "DiaChiEmail", donHang.MaTK);
                return View(donHang);
            }
            
            
        }

        //XÓA ĐƠN HÀNG
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            DonHang donHang = new DonHang();
            try
            {
                donHang = db.DonHangs.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (donHang == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(donHang);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List<ChiTietDH> chiTietDonHangs = new List<ChiTietDH>();
            try
            {
                chiTietDonHangs = db.ChiTietDHs.Where(ct => ct.SoDH == id).ToList();
                if(chiTietDonHangs.Count > 0)
                {
                    foreach(var ct in chiTietDonHangs)
                    {
                        db.ChiTietDHs.Remove(ct);
                    }
                    DonHang donHang = db.DonHangs.Find(id);
                    db.DonHangs.Remove(donHang);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    DonHang donHang = db.DonHangs.Find(id);
                    db.DonHangs.Remove(donHang);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            } 
        }

        //TRANG CHI TIẾT CÁC SẢN PHẨM CÓ TRONG ĐƠN
        public ActionResult ChiTietDonHang(int? id)
        {
            List<ChiTietDH> chiTietDHs = new List<ChiTietDH>();
            if(id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            try
            {
                chiTietDHs = db.ChiTietDHs.Where(ct => ct.SoDH == id).ToList();
            }catch(Exception e)
            {

            }
            //var chiTietDHs = db.ChiTietDHs.Include(c => c.DonHang).Include(c => c.SanPham);
            ViewData["id"] = id;
            return View(chiTietDHs.ToList());
        }

        //THÊM SẢN PHẨM VÀO ĐƠN
        public ActionResult CreateChiTietDonHang(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP");
            ViewData["id"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateChiTietDonHang([Bind(Include = "SoDH,MaSP,SoLuong")] ChiTietDH chiTietDH)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string check = validChiTietDH(chiTietDH);
                    if(check.Length > 0)
                    {
                        ViewBag.Error = check;
                        ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", chiTietDH.MaSP);
                        return View(chiTietDH);
                    }
                    if (string.IsNullOrEmpty(chiTietDH.SoDH.ToString()))
                    {
                        return RedirectToAction("NotFoundPage", "ErrorAdmin");
                    }
                    db.ChiTietDHs.Add(chiTietDH);
                    db.SaveChanges();
                    //CẬP NHẬT LẠI SỐ TIỀN Ở ĐƠN HÀNG
                    UpdatePriceTotal(chiTietDH.SoDH);
                    return RedirectToAction("ChiTietDonHang", new { id = chiTietDH.SoDH });
                }

                ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", chiTietDH.MaSP);
                return View(chiTietDH);
            }
            catch(Exception e)
            {
                ViewBag.Error = "Lỗi thêm: " + e.Message;
                ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", chiTietDH.MaSP);
                return View(chiTietDH);
            }
            
        }

        //CHỈNH SỬA CHI TIẾT CỦA ĐƠN
        public ActionResult EditChiTietDonHang(int? id, int? SoDH)
        {
            if (id == null || SoDH == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }

            ChiTietDH chiTietDH = new ChiTietDH();
            try
            {
                chiTietDH = db.ChiTietDHs.Find(id);
                if (chiTietDH == null)
                {
                    return RedirectToAction("NotFoundPage", "ErrorAdmin");
                }
                ViewData["SoDH"] = SoDH;
                ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", chiTietDH.MaSP);
                return View(chiTietDH);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            } 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditChiTietDonHang([Bind(Include = "id,SoDH,MaSP,SoLuong")] ChiTietDH chiTietDH)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string check = validChiTietDH(chiTietDH);
                    if (check.Length > 0)
                    {
                        ViewBag.Error = check;
                        ViewData["SoDH"] = chiTietDH.SoDH;
                        ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", chiTietDH.MaSP);
                        return View(chiTietDH);
                    }
                    if (string.IsNullOrEmpty(chiTietDH.SoDH.ToString()) || string.IsNullOrEmpty(chiTietDH.id.ToString()))
                    {
                        return RedirectToAction("NotFoundPage", "ErrorAdmin");
                    }
                    db.Entry(chiTietDH).State = EntityState.Modified;
                    db.SaveChanges();
                    //CẬP NHẬT LẠI SỐ TIỀN
                    UpdatePriceTotal(chiTietDH.SoDH);
                    return RedirectToAction("ChiTietDonHang", new { id = chiTietDH.SoDH });
                }

                ViewData["SoDH"] = chiTietDH.SoDH;
                ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", chiTietDH.MaSP);
                return View(chiTietDH);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi thêm: " + e.Message;
                ViewData["SoDH"] = chiTietDH.SoDH;
                ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", chiTietDH.MaSP);
                return View(chiTietDH);
            }

        }

        //XÓA CHI TIẾT ĐƠN
        public ActionResult DeleteChiTietDonHang(int? id, int? SoDH)
        {
            if (id == null || SoDH == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }

            ChiTietDH chiTietDH = new ChiTietDH();
            try
            {
                chiTietDH = db.ChiTietDHs.Find(id);
                if (chiTietDH == null)
                {
                    return RedirectToAction("NotFoundPage", "ErrorAdmin");
                }

                ViewData["SoDH"] = SoDH;
                ViewBag.MaSP = new SelectList(db.SanPhams, "MaSP", "TenSP", chiTietDH.MaSP);
                return View(chiTietDH);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteChiTietDonHang(int id, int SoDH)
        {
            try
            {
                var chitietDH = db.ChiTietDHs.Find(id);
                if(chitietDH == null)
                {
                    return RedirectToAction("NotFoundPage", "ErrorAdmin");
                }
                db.ChiTietDHs.Remove(chitietDH);
                db.SaveChanges();
                //CẬP NHẬT LẠI SỐ TIỀN CỦA ĐƠN
                UpdatePriceTotal((int)SoDH);
                return RedirectToAction("ChiTietDonHang", new { id = SoDH });
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }

        }

        //HÀM CẬP NHẬT LẠI SỐ TIỀN CỦA ĐƠN
        public void UpdatePriceTotal(int SoDH)
        {
            List<ChiTietDH> chiTietDHs = new List<ChiTietDH>();
            chiTietDHs = db.ChiTietDHs.Where(ct => ct.SoDH == SoDH).ToList();
            int total = 0;
            if(chiTietDHs.Count > 0)
            {
                foreach(ChiTietDH c in chiTietDHs)
                {
                    SanPham sanPham = (SanPham)db.SanPhams.FirstOrDefault(sp => sp.MaSP == c.MaSP);
                    if(sanPham != null)
                    {
                        if ((sanPham.PhanTramKM != null || sanPham.PhanTramKM.ToString().Trim() != "") && sanPham.PhanTramKM > 0)
                        {
                            int km = int.Parse(sanPham.PhanTramKM.ToString());
                            int giaKM = (sanPham.GiaBan - sanPham.GiaBan * km / 100) * c.SoLuong;
                            total += giaKM;
                        }
                        else
                        {
                            total += sanPham.GiaBan * c.SoLuong;
                        }
                    }
                }
            }

            DonHang donHang = db.DonHangs.Find(SoDH);
            if(donHang != null)
            {
                donHang.TongTien = total;
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
            }

        }

        //HÀM KIỂM TRA TÍNH HỢP LỆ CỦA DỮ LIỆU
        public string validChiTietDH(ChiTietDH chiTietDH)
        {
            if (string.IsNullOrEmpty(chiTietDH.MaSP.ToString()))
            {
                return "Vui lòng chọn sản phẩm!";
            }
            if (string.IsNullOrEmpty(chiTietDH.SoLuong.ToString()))
            {
                return "Vui lòng nhập số lượng!";
            }
            if(chiTietDH.SoLuong <= 0)
            {
                return "Số lượng sản phẩm phải lớn hơn 0!";
            }
            return "";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}