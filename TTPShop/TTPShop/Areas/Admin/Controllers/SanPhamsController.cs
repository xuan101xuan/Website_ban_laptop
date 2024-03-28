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
    public class SanPhamsController : BaseController
    {
        private TTPShopDB db = new TTPShopDB();

        //TRANG CHỦ TRANG SẢN PHẨM (TÌM KIẾM THEO TÊN SP, SẮP XẾP THEO TÊN SP, TÊN DANH MỤC)
        public ActionResult Index(string sortOder, string searchString, string currenFilter, int? page)
        {
            int pageSize = 0;
            int pageNumber = 0;
            ViewBag.CurrentSort = sortOder;
            ViewBag.SapTheoTen = string.IsNullOrEmpty(sortOder) ? "name_desc" : "";
            ViewBag.SapTheoDanhMuc = sortOder == "TenDM" ? "dm_desc" : "TenDM";
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
                var sanPhams = db.SanPhams.Select(t => t);
                if (!String.IsNullOrEmpty(searchString))
                {
                    sanPhams = sanPhams.Where(t => t.TenSP.Contains(searchString));
                }
                switch (sortOder)
                {
                    case "name_desc":
                        sanPhams = sanPhams.OrderByDescending(t => t.TenSP);
                        break;

                    case "TenDM":
                        sanPhams = sanPhams.OrderBy(t => t.DanhMuc.TenDM);
                        break;

                    case "dm_desc":
                        sanPhams = sanPhams.OrderByDescending(t => t.DanhMuc.TenDM);
                        break;

                    default:
                        sanPhams = sanPhams.OrderBy(t => t.TenSP);
                        break;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    pageSize = sanPhams.ToList().Count;
                    if (pageSize == 0)
                    {
                        pageSize = 1;
                    }
                    pageNumber = (page ?? 1);
                    return View(sanPhams.ToPagedList(pageNumber, pageSize));
                }
                pageSize = 5;
                pageNumber = (page ?? 1);
                return View(sanPhams.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }  
        }

        //CHI TIẾT SẢN PHẨM
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            SanPham sanPham = new SanPham();
            try
            {
                sanPham = db.SanPhams.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            } 
            if (sanPham == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(sanPham);
        }

        //THÊM SẢN PHẨM
        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "MaSP,MaDM,TenSP,GiaBan,AnhSP,MoTa,PhanTramKM,SoLuong")] SanPham sanPham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sanPham.AnhSP = "";
                    var f = Request.Files["ImageFile"];
                    if (string.IsNullOrEmpty(sanPham.MoTa))
                    {
                        sanPham.MoTa = "";
                    }
                    if (sanPham.SoLuong == null)
                    {
                        sanPham.SoLuong = 0;
                    }
                    if (sanPham.PhanTramKM == null)
                    {
                        sanPham.PhanTramKM = 0;
                    }
                    if (sanPham.SoLuong < 0 || sanPham.PhanTramKM < 0 || sanPham.GiaBan < 0 || sanPham.PhanTramKM > 100)
                    {
                        ViewBag.Error = "Giá sản phẩm, khuyến mãi, giá bán phải lớn hơn 0 và khuyến mãi nhỏ hơn 100!";
                        ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
                        return View(sanPham);
                    }
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Areas/Admin/Assets/images/" + FileName);
                        f.SaveAs(UploadPath);
                        sanPham.AnhSP = FileName;
                    }
                    sanPham.NgayTao = DateTime.Now;
                    db.SanPhams.Add(sanPham);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
                    return View(sanPham);
                }
                
            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi thêm: " + e.Message;
                ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
                return View(sanPham);
            }
        }

        //CHỈNH SỬA
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            SanPham sanPham = new SanPham();
            try
            {
                sanPham = db.SanPhams.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
                
            if (sanPham == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "MaSP,MaDM,TenSP,GiaBan,AnhSP,MoTa,PhanTramKM,SoLuong,NgayTao")] SanPham sanPham)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(sanPham.MoTa))
                    {
                        sanPham.MoTa = "";
                    }
                    if (sanPham.SoLuong == null)
                    {
                        sanPham.SoLuong = 0;
                    }
                    if (sanPham.PhanTramKM == null)
                    {
                        sanPham.PhanTramKM = 0;
                    }
                    if (sanPham.SoLuong < 0 || sanPham.PhanTramKM < 0 || sanPham.GiaBan < 0)
                    {
                        ViewBag.Error = "Giá sản phẩm, Khuyến mãi, giá bán phải lớn hơn 0!";
                        ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
                        return View(sanPham);
                    }

                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Areas/Admin/Assets/images/" + FileName);
                        f.SaveAs(UploadPath);
                        sanPham.AnhSP = FileName;
                    }
                    else
                    {
                        sanPham.AnhSP = Request["oldImage"];
                    }
                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
                    return View(sanPham);
                }
                
            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi: " + e.Message;
                ViewBag.MaDM = new SelectList(db.DanhMucs, "MaDM", "TenDM", sanPham.MaDM);
                return View(sanPham);
            }
        }

        //XÓA
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            SanPham sanPham = new SanPham();
            try
            {
                sanPham = db.SanPhams.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (sanPham == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(sanPham);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = new SanPham();
            try
            {
                sanPham = db.SanPhams.Find(id);
                if (sanPham == null)
                {
                    return RedirectToAction("NotFoundPage", "ErrorAdmin");
                }
                db.SanPhams.Remove(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Sản phẩm liên kết với các mục khác vui lòng xóa chúng trước khi xóa sản phẩm!";
                DanhMuc danhMuc = db.DanhMucs.Find(sanPham.MaDM);
                ViewData["MaDM"] = danhMuc.TenDM;
                return View(sanPham);
            }
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