using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TTPShop.Common;
using TTPShop.Models;

namespace TTPShop.Areas.Admin.Controllers
{
    public class TaiKhoansController : BaseController
    {
        private TTPShopDB db = new TTPShopDB();
        private PasswordCrypt passwordCrypt = new PasswordCrypt();

        //TRANG CHỦ TRANG TÀI KHOẢN
        public ActionResult Index(string searchString, string currenFilter, int? page)
        {
            int pageSize = 0;
            int pageNumber = 0;
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
                var taiKhoans = db.TaiKhoans.Select(t => t);
                if (!String.IsNullOrEmpty(searchString))
                {
                    taiKhoans = taiKhoans.Where(t => t.HoTen.Contains(searchString));
                    pageSize = taiKhoans.ToList().Count;
                    if (pageSize == 0)
                    {
                        pageSize = 1;
                    }
                    pageNumber = (page ?? 1);
                    taiKhoans = taiKhoans.OrderBy(dh => dh.HoTen);
                    return View(taiKhoans.ToPagedList(pageNumber, pageSize));
                }
                taiKhoans = taiKhoans.OrderBy(dh => dh.HoTen);
                pageSize = 5;
                pageNumber = (page ?? 1);
                return View(taiKhoans.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
        }

        //KHÓA TÀI KHOẢN
        public ActionResult Block(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            TaiKhoan taiKhoan = new TaiKhoan();
            try
            {
                taiKhoan = db.TaiKhoans.Find(id);
            }
            catch(Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
             
            if (taiKhoan == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            return View(taiKhoan);
        }

        [HttpPost, ActionName("Block")]
        [ValidateAntiForgeryToken]
        public ActionResult BlockConfirmed(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            TaiKhoan taiKhoan = new TaiKhoan();
            try
            {
                taiKhoan = db.TaiKhoans.Find(id);
                if (taiKhoan == null)
                {
                    return HttpNotFound();
                }
                if (taiKhoan.TrangThai == 0)
                {
                    taiKhoan.TrangThai = 1;
                }
                else
                {
                    taiKhoan.TrangThai = 0;
                }

                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();

                var taiKhoans = db.TaiKhoans.OrderBy(t => t.HoTen).Select(t => t);

                int pageSize = 3;
                int pageNumber = 1;

                return View("Index", taiKhoans.ToPagedList(pageNumber, pageSize));
            }
            catch(Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            //return View("Index", 1);
        }

        //CHI TIẾT TÀI KHOẢN
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            TaiKhoan taiKhoan = new TaiKhoan();
            try
            {
                taiKhoan = db.TaiKhoans.Find(id);
            }
            catch(Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
                
            if (taiKhoan == null)
            {
                return View(taiKhoan);
            }
            //GIẢI MÃ MẬT KHẨU
            string passhash = passwordCrypt.Base64Decode(taiKhoan.MatKhau);
            taiKhoan.MatKhau = passhash;
            return View(taiKhoan);
        }

        //THÊM TÀI KHOẢN
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TaiKhoans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTK,DiaChiEmail,MatKhau,HoTen,VaiTro,GioiTinh,NgaySinh,SoDienThoai,TrangThai")] TaiKhoan taiKhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //KIỂM TRA TÍNH HỢP LỆ
                    string error = checkEditTaiKhoan(taiKhoan);
                    if (error.Length == 0)
                    {
                        var user = db.TaiKhoans.SingleOrDefault(u => u.DiaChiEmail.Equals(taiKhoan.DiaChiEmail));
                        if (user == null)
                        {
                            //MÃ HÓA MẬT KHẨU
                            string passhash = passwordCrypt.Base64Encode(taiKhoan.MatKhau);
                            taiKhoan.MatKhau = passhash;
                            db.TaiKhoans.Add(taiKhoan);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.Error = "Email này đã được sử dụng!";
                            return View(taiKhoan);
                        }
                    }
                    else
                    {
                        ViewBag.Error = error;
                        return View(taiKhoan);
                    }
                }
                return View(taiKhoan);
                
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
        }

        //CHỈNH SỬA
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("NotFoundPage", "ErrorAdmin");
            }
            TaiKhoan taiKhoan = new TaiKhoan();
            try
            {
                taiKhoan = db.TaiKhoans.Find(id);
            }
            catch(Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
            if (taiKhoan == null)
            {
                return View(taiKhoan);
            }
            else
            {
                string passhash = passwordCrypt.Base64Decode(taiKhoan.MatKhau);
                taiKhoan.MatKhau = passhash;
            }
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "MaTK,DiaChiEmail,MatKhau,HoTen,VaiTro,GioiTinh,SoDienThoai,NgaySinh,TrangThai")] TaiKhoan taiKhoan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string error = checkEditTaiKhoan(taiKhoan);
                    if (error.Length == 0)
                    {
                        var user = db.TaiKhoans.SingleOrDefault(u => u.DiaChiEmail.Equals(taiKhoan.DiaChiEmail));
                        if (user == null)
                        {
                            string passhash = passwordCrypt.Base64Encode(taiKhoan.MatKhau);
                            taiKhoan.MatKhau = passhash;
                            db.TaiKhoans.AddOrUpdate(taiKhoan);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            if(user.MaTK == taiKhoan.MaTK)
                            {
                                string passhash = passwordCrypt.Base64Encode(taiKhoan.MatKhau);
                                taiKhoan.MatKhau = passhash;
                                if (CompareUser(taiKhoan, user) == true)
                                {
                                    return RedirectToAction("Index");
                                }
                                db.TaiKhoans.AddOrUpdate(taiKhoan);
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
                            ViewBag.Error = "Email này đã được sử dụng!";
                            return View(taiKhoan);
                        }
                    }
                    else
                    {
                        ViewBag.Error = error;
                        return View(taiKhoan);
                    }
                }
                else
                {
                    return View(taiKhoan);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "ErrorAdmin");
            }
        }

        //HÀM SO SÁNH HAI TÀI KHOẢN CÓ TRUNG KHỚP HAY KHÔNG
        public bool CompareUser(TaiKhoan taiKhoan1, TaiKhoan taiKhoan2)
        {
            if(taiKhoan1.DiaChiEmail == taiKhoan2.DiaChiEmail)
            {
                if(taiKhoan1.MatKhau == taiKhoan2.MatKhau)
                {
                    if(taiKhoan1.HoTen == taiKhoan2.HoTen)
                    {
                        if(taiKhoan1.GioiTinh == taiKhoan2.GioiTinh)
                        {
                            if(taiKhoan1.NgaySinh == taiKhoan2.NgaySinh)
                            {
                                if(taiKhoan1.TrangThai == taiKhoan2.TrangThai)
                                {
                                    if(taiKhoan1.VaiTro == taiKhoan2.VaiTro)
                                    {
                                        if(taiKhoan1.SoDienThoai == taiKhoan2.SoDienThoai)
                                        {
                                            return true;
                                        }
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        //XÓA 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            string passhash = passwordCrypt.Base64Decode(taiKhoan.MatKhau);
            taiKhoan.MatKhau = passhash;
            return View(taiKhoan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaiKhoan taiKhoan = new TaiKhoan();
            var session = Session["MaTK"];
            try
            {
                taiKhoan = db.TaiKhoans.Find(id);
                db.TaiKhoans.Remove(taiKhoan);
                db.SaveChanges();
                
                if (int.Parse(session.ToString()) == id)
                {
                    Session.Clear();
                }
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ViewBag.Error = "Tài khoản liên kết với các mục khác vui lòng xóa chúng trước khi xóa tài khoản!";
                return View(taiKhoan);
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

        //HÀM KIỂM TRA TÍNH HỢP LỆ CỦA TÀI KHOẢN
        public string checkEditTaiKhoan(TaiKhoan taiKhoan)
        {
            if(taiKhoan == null)
            {
                return "Vui lòng nhập thông tin trước khi nhấn Lưu!";
            }
            if (string.IsNullOrEmpty(taiKhoan.DiaChiEmail))
            {
                return "Vui lòng nhập email!";
            }
            if (string.IsNullOrEmpty(taiKhoan.MatKhau))
            {
                return "Vui lòng nhập mật khẩu!";
            }
            if (string.IsNullOrEmpty(taiKhoan.HoTen))
            {
                return "Vui lòng nhập họ tên!";
            }
            if (string.IsNullOrEmpty(taiKhoan.VaiTro))
            {
                return "Vui lòng chọn vai trò!";
            }
            if (string.IsNullOrEmpty(taiKhoan.GioiTinh))
            {
                return "Vui lòng chọn giới tính!";
            }
            if (string.IsNullOrEmpty(taiKhoan.SoDienThoai))
            {
                return "Vui lòng nhập số điện thoại!";
            }
            if(taiKhoan.NgaySinh > DateTime.Now)
            {
                return "Ngày sinh không hợp lệ!";
            }
            if (taiKhoan.TrangThai != 0 && taiKhoan.TrangThai != 1)
            {
                return "Trạng thái chỉ có 2 giá trị là 0 và 1!";
            }

            string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (!Regex.IsMatch(taiKhoan.DiaChiEmail, expression))
            {
                return "Vui lòng nhập địa chỉ email đúng định dạng!";
            }
            if(taiKhoan.MatKhau.Length < 6)
            {
                return "Mật khẩu phải có ít nhất 6 ký tự!";
            }
            if(taiKhoan.VaiTro != "Administrator" && taiKhoan.VaiTro != "User")
            {
                return "Vai trò chỉ có 2 giá trị là User và Administrator!";
            }
            string regex = @"^([\+]?61[-]?|[0])?[1-9][0-9]{8}$";

            if (!Regex.IsMatch(taiKhoan.SoDienThoai, regex))
            {
                return "Vui lòng nhập số điện thoại đúng định dạng!";
            }
            return "";
        }
    }
}