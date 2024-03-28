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
    public class DanhMucsController : BaseController
    {
        private TTPShopDB db = new TTPShopDB();

        //TRANG CHỦ CỦA TRANG QUẢN LÝ DANH MỤC
        public ActionResult Index(string searchString, string currenFilter, int? page)
        {
            int pageSize = 0;
            int pageNumber = 0;
            //KIỂM TRA XEM CÓ TÌM KIẾM THEO TÊN DANH MỤC HAY KHÔNG
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
                var danhMucs = db.DanhMucs.Select(t => t);
                if (!String.IsNullOrEmpty(searchString))
                {
                    danhMucs = danhMucs.Where(t => t.TenDM.Contains(searchString));
                    pageSize = danhMucs.ToList().Count;
                    if (pageSize == 0)
                    {
                        pageSize = 1;
                    }
                    //PHÂN TRANG BẰNG 1 NẾU CÓ TÌM KIẾM
                    pageNumber = (page ?? 1);
                    danhMucs = danhMucs.OrderBy(dh => dh.TenDM);
                    return View(danhMucs.ToPagedList(pageNumber, pageSize));
                }
                danhMucs = danhMucs.OrderBy(dh => dh.TenDM);
                //PHÂN TRANG BẰNG 5
                pageSize = 5;
                pageNumber = (page ?? 1);
                return View(danhMucs.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
        }

        //CHI TIẾT DANH MỤC
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            DanhMuc danhMuc = new DanhMuc();
            try
            {
                danhMuc = db.DanhMucs.Find(id);
            }
            catch(Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
                
            if (danhMuc == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(danhMuc);
        }

        //TẠO DNAH MỤC
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDM,TenDM,AnhDM")] DanhMuc danhMuc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(danhMuc.TenDM))
                    {
                        ViewBag.Error = "Vui lòng nhập tên danh mục!";
                        return View(danhMuc);
                    }
                    danhMuc.AnhDM = "";
                    var f = Request.Files["ImageFile"];

                    if (f != null && f.ContentLength > 0)
                    {
                        //LẤY FILE ẢNH VÀ LƯU VÀO THƯ MỤC TRONG PROJECT
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Areas/Admin/Assets/images/" + FileName);
                        f.SaveAs(UploadPath);
                        danhMuc.AnhDM = FileName;
                    }
                    else
                    {
                        string FileName = "";
                        danhMuc.AnhDM = FileName;
                    }
                    db.DanhMucs.Add(danhMuc);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(danhMuc);
                }
                
            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi thêm: " + e.Message;
                return View(danhMuc);
            }
        }

        //CHỈNH SỬA
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            DanhMuc danhMuc = new DanhMuc();
            try
            {
                danhMuc = db.DanhMucs.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
                
            if (danhMuc == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(danhMuc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDM,TenDM,AnhDM")] DanhMuc danhMuc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(danhMuc.TenDM))
                    {
                        ViewBag.Error = "Vui lòng nhập tên danh mục!";
                        return View(danhMuc);
                    }
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        //LẤY FILE ẢNH VÀ LƯU VÀO THƯ MỤC TRONG PROJECT
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Areas/Admin/Assets/images/" + FileName);
                        f.SaveAs(UploadPath);
                        danhMuc.AnhDM = FileName;
                    }
                    else
                    {
                        danhMuc.AnhDM = Request["oldImage"];
                    }
                    db.Entry(danhMuc).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(danhMuc);
                }
                
            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi: " + e.Message;
                return View(danhMuc);
            }
        }

        //XÓA DANH MỤC
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            DanhMuc danhMuc = new DanhMuc();
            try
            {
                danhMuc = db.DanhMucs.Find(id);
            }
            catch
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (danhMuc == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(danhMuc);
        }

        // POST: Admin/DanhMucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DanhMuc danhMuc = new DanhMuc();
            try
            {
                danhMuc = db.DanhMucs.Find(id);
                if(danhMuc == null)
                {
                    return RedirectToAction("Index");
                }
                db.DanhMucs.Remove(danhMuc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["Error"] = "Danh mục liên kết với các mục khác vui lòng xóa chúng trước khi xóa danh mục này!";
                return View(danhMuc);
            }
            
        }

        //GIẢI PHÓNG CÁC TÀI NGUYÊN KHÔNG CẦN THIẾT
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