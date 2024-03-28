using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTPShop.Models;
using PagedList;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Text.RegularExpressions;
using TTPShop.Common;
using System.Configuration;
using WebLATA.Common;
using Newtonsoft.Json.Linq;
using System.Web.UI.WebControls;

namespace TTPShop.Controllers
{
    public class HomeController : Controller
    {
        private TTPShopDB db = new TTPShopDB();
        private PasswordCrypt passwordCrypt = new PasswordCrypt();

        //HIỂN THỊ TRANG CHỦ
        public ActionResult Index()
        {
            List<SanPham> sanPhams = new List<SanPham>();
            try
            {
                //LẤY 8 SẢN PHẨM SẮP XẾP GIẢM DẦN THEO THEO NGÀY TẠO HIỂN THỊ LÊN TRANG CHỦ
                sanPhams = db.SanPhams.Where(s => s.SoLuong > 0).OrderByDescending(s => s.NgayTao).Take(8).ToList();
            }catch(Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
            
            return View(sanPhams);
        }

        //CHI TIẾT SẢN PHẨM
        public ActionResult ProductDetails(int? id)
        {
            if(id == null)
            {
                return View();
            }

            SanPham sanPham = new SanPham();
            try
            {
                //LẤY CHI TIẾT SẢN PHẨM
                sanPham = db.SanPhams.SingleOrDefault(sp => sp.MaSP == id);
            }catch(Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
            
            
            if(sanPham == null)
            {
                return View();
            }
            return View(sanPham);
        }

        //ĐĂNG NHẬP
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string DiaChiEmail, string MatKhau)
        {
            //KIỂM TRA
            if (string.IsNullOrEmpty(DiaChiEmail) && string.IsNullOrEmpty(MatKhau))
            {
                ViewBag.Error = "Vui lòng nhập thông tin trước khi đăng nhập!";
            }else if (string.IsNullOrEmpty(DiaChiEmail) || string.IsNullOrEmpty(MatKhau))
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //MÃ HÓA MẬT KHẨU
                    string passhash = passwordCrypt.Base64Encode(MatKhau);
                    try
                    {
                        var user = db.TaiKhoans.SingleOrDefault(u => u.DiaChiEmail.Equals(DiaChiEmail) && u.MatKhau.Equals(passhash));
                        if (user != null)
                        {
                            if (user.TrangThai == 0)
                            {
                                ViewBag.Error = "Tài khoản đã bị khóa";
                                return View();
                            }
                            //LƯU VÀO SESSION
                            Session["HoTen"] = user.HoTen;
                            Session["Email"] = user.DiaChiEmail;
                            Session["MaTK"] = user.MaTK;
                            Session["Perrmission"] = user.VaiTro;
                            if (user.VaiTro.Equals("Administrator"))
                            {
                                return RedirectToAction("Index", "Home", new { area = "Admin" });
                            }
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
                        }
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("ServerError", "Error");
                    }

                }
            }
            return View();
        }

        //ĐĂNG KÝ
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "MaTK, DiaChiEmail, HoTen, MatKhau, VaiTro, GioiTinh, NgaySinh, SoDienThoai")] TaiKhoan taiKhoan)
        {
            try
            {
                //KIỂM TRA TÍNH HỢP LỆ CỦA DỮ LIỆU ĐẦU VÀO
                bool check = ValidRegister(taiKhoan);
                if (check == false)
                {
                    ViewBag.Error = "Vui lòng điền đây đủ thông tin!";
                    return View(taiKhoan);
                }
                else
                {
                    //KIỂM TRA ĐỊA CHỈ EAMIL CÓ TỒN TẠI HAY KHÔNG
                    var user = db.TaiKhoans.SingleOrDefault(u => u.DiaChiEmail.Equals(taiKhoan.DiaChiEmail) || u.SoDienThoai.Equals(taiKhoan.SoDienThoai));
                    if (user == null)
                    {
                        //TẠO MỚI TÀI KHOẢN
                        taiKhoan.VaiTro = "User";
                        taiKhoan.TrangThai = 1;
                        string passhash = passwordCrypt.Base64Encode(taiKhoan.MatKhau);
                        //MÃ HÓA MẬT KHẨU
                        taiKhoan.MatKhau = passhash;
                        //KIỂM TRA TÍNH HỢP LỆ CỦA DỮ LIỆU
                        if (ModelState.IsValid)
                        {
                            if (!string.IsNullOrEmpty(taiKhoan.SoDienThoai) && taiKhoan.MatKhau.Length < 6 )
                            {
                                ViewBag.Error = "Mật khẩu phải có tối thiểu 6 ký tự!";
                                return View(taiKhoan);
                            }
                            if(taiKhoan.NgaySinh > DateTime.Now)
                            {
                                ViewBag.Error = "Ngày sinh không hợp lệ!";
                                return View(taiKhoan);
                            }
                            //THÊM TÀI KHOẢN VÀO DB
                            db.TaiKhoans.Add(taiKhoan);
                            db.SaveChanges();
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            return View(taiKhoan);
                        }
                        
                    }
                    else
                    {
                        ViewBag.Error = "Email hoặc Số điện thoại đã tồn tại!!!";
                        return View(taiKhoan);
                    }
                }
               
            }
            catch (Exception)
            {
                ViewBag.Error = "Đăng ký không thành công vui lòng thử lại sau!";
                return View(taiKhoan);
            }
        }

        //HÀM KIỂM TRẢ TÍNH HỢP LỆ CỦA DỮ LIỆU
        public bool ValidRegister(TaiKhoan taiKhoan)
        {
            if (string.IsNullOrEmpty(taiKhoan.HoTen))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(taiKhoan.MatKhau))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(taiKhoan.DiaChiEmail))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(taiKhoan.GioiTinh))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(taiKhoan.SoDienThoai))
            {
                return false;
            }
            else
            {
                string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (!Regex.IsMatch(taiKhoan.DiaChiEmail , expression))
                {
                    return false;
                }
                string regex = @"^([\+]?61[-]?|[0])?[1-9][0-9]{8}$";

                if (!Regex.IsMatch(taiKhoan.SoDienThoai, regex))
                {
                    return false;
                }
            }
            return true;

        }

        //CHỈNH SỬA THÔNG TIN TÀI KHOẢN
        [HttpGet]
        public ActionResult EditUserInfor(int? id)
        {
            //KIỂM TRA ĐĂNG NHẬP
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            List<string> Genders = new List<string>
                    {
                        "Nam", "Nữ"
                    };
            ViewBag.Genders = Genders;
            if (id == null)
            {
                return RedirectToAction("Login", "Home");
            }
            TaiKhoan taiKhoan = new TaiKhoan();
            try
            {
                 taiKhoan = db.TaiKhoans.Find(id);
            }catch(Exception e)
            {
                return View("ServerError", "Error");
            }
             
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            //GIẢI MÃ MẬT KHẨU
            string passhash = passwordCrypt.Base64Decode(taiKhoan.MatKhau);
            taiKhoan.MatKhau = passhash;
            return View(taiKhoan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult EditUserInfor([Bind(Include = "MaTK,DiaChiEmail,MatKhau,HoTen,VaiTro,GioiTinh,NgaySinh,SoDienThoai")] TaiKhoan taiKhoan)
        {
            try
            {
                List<string> Genders = new List<string>
                {
                    "Nam", "Nữ"
                };
                //KIỂM TRA DỮ LIỆU
                if (ModelState.IsValid)
                {
                    
                    bool check = ValidRegister(taiKhoan);
                    if (check == false)
                    {
                        ViewBag.Genders = Genders;
                        ViewBag.Error = "Vui lòng nhập đầy đủ thôn tin!";
                        return View(taiKhoan);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(taiKhoan.MatKhau) && taiKhoan.MatKhau.Length < 6)
                        {

                            ViewBag.Genders = Genders;
                            ViewBag.Error = "Mật khẩu phải có tối thiểu 6 ký tự!";
                            return View(taiKhoan);
                        }
                        if(taiKhoan.NgaySinh > DateTime.Now)
                        {
                            ViewBag.Genders = Genders;
                            ViewBag.Error = "Ngày sinh không hợp lệ!";
                            return View(taiKhoan);
                        }

                        //MÃ HÓA MẬT KHẨU
                        string passhash = passwordCrypt.Base64Encode(taiKhoan.MatKhau);
                        taiKhoan.MatKhau = passhash;
                        taiKhoan.TrangThai = 1;
                        //CẬP NHẬT THÔNG TIN
                        db.Entry(taiKhoan).State = EntityState.Modified;
                        db.SaveChanges();
                        //LƯU SESSION MỚI
                        Session["HoTen"] = taiKhoan.HoTen;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.Genders = Genders;
                    return View(taiKhoan);
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = "Lỗi trong quá trình cập nhật thông tin vui lòng thử lại sau! " + e.Message;
                return View(taiKhoan);
            }
        }

        //GIỎ HÀNG
        public ActionResult ShopCart()
        {
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            List<CartItem> items = new List<CartItem>();
            int MaTK = Convert.ToInt32(Session["MaTK"]);
            int total = 0;

            items = GetListCartItem(MaTK);
            if(items.Count > 0)
            {
                total = TotalPriceCarts(items);
            }

            ViewData["Total"] = total;
            return View(items);
        }

        //CẬP NHẬT GIỎ HÀNG
        public ActionResult Update_Cart_quantity(FormCollection form)
        {
            int quantity = int.Parse(form["quantity"]);
            int productID = int.Parse(form["productID"]);
            try
            {
                Cart cart = db.Carts.FirstOrDefault(sp => sp.MaSP == productID);
                cart.quantity = quantity;
                db.Carts.AddOrUpdate(cart);
                db.SaveChanges();
            }catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
          
            return RedirectToAction("ShopCart", "Home");
        }

        //SỐ LƯỢNG SẢN PHẨM TRONG GIỎ HÀNG
        public ActionResult Show_Count_Cart()
        {
            if (Session["MaTK"] == null)
            {
                ViewData["TotalCart"] = 0;

                return View();
            }
            List<Cart> carts = new List<Cart>();
            int MaTK = Convert.ToInt32(Session["MaTK"]);
            try
            {
                carts = db.Carts.Where(c => c.MaTK == MaTK).ToList();
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }

            ViewData["TotalCart"] = carts.Count;

            return View();
        }

        //THÊM SẢN PHẨM VÀO GIỎ HÀNG KHI CLICK VÀO ICON TRÊN TRANG CHỦ
        public ActionResult AddItem_icon(int productID)
        {
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            SanPham product = new SanPham();
            try
            {
                product = db.SanPhams.SingleOrDefault(s => s.MaSP == productID);
                int MaTK = Convert.ToInt32(Session["MaTK"]);
                if (product != null && product.SoLuong > 0)
                {
                    Cart cart = new Cart();
                    cart = db.Carts.Where(c => c.MaTK == MaTK).Where(c => c.MaSP == productID).FirstOrDefault();
                    if(cart == null)
                    {
                        Cart c = new Cart();
                        c.MaTK = MaTK;
                        c.quantity = 1;
                        c.MaSP = productID;
                        db.Carts.Add(c);
                        db.SaveChanges();
                        return RedirectToAction("ShopCart", "Home");
                    }
                    else
                    {
                        cart.quantity += 1;
                        db.Carts.AddOrUpdate(cart);
                        db.SaveChanges();
                        return RedirectToAction("ShopCart", "Home");
                    }
                    
                }
            }catch(Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
            
            ViewBag.ErrorSoLuong = "Sản phẩm đã hết hàng";
            return View("ProductDetails", product);
        }

        //THÊM SẢN PHẨM VÀO GIỎ HÀNG Ở TRANG CHI TIẾT SẢN PHẨM
        public ActionResult AddItem(FormCollection data)
        {
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            SanPham product = new SanPham();
            int quantity = Convert.ToInt32(data["soluong"]);
            int productID = Convert.ToInt32(data["productID"]);

            try
            {
                product = db.SanPhams.SingleOrDefault(s => s.MaSP == productID);
                int MaTK = Convert.ToInt32(Session["MaTK"]);
                if (product == null || product.SoLuong < 1)
                {
                    ViewBag.ErrorSoLuong = "Sản phẩm đã hết hàng";
                    return View("ProductDetails", product);
                }
                if (product != null && quantity > 0)
                {
                    Cart cart = new Cart();
                    cart = db.Carts.Where(c => c.MaTK == MaTK).Where(c => c.MaSP == productID).FirstOrDefault();
                    if (cart == null)
                    {
                        Cart c = new Cart();
                        c.MaTK = MaTK;
                        c.quantity = quantity;
                        c.MaSP = productID;
                        db.Carts.Add(c);
                        db.SaveChanges();
                        return RedirectToAction("ShopCart", "Home");
                    }
                    else
                    {
                        cart.quantity += quantity;
                        db.Carts.AddOrUpdate(cart);
                        db.SaveChanges();
                        return RedirectToAction("ShopCart", "Home");
                    }
                }
            }catch(Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }

            
            ViewBag.ErrorSoLuong = "Số lượng sản phẩm nhỏ hơn 0";
            return View("ProductDetails", product);
        }

        //XÓA SẢN PHẨM TRONG GIỎ HÀNG
        public ActionResult Remove_Cart(int productID)
        {
            try
            {
                Cart cart = db.Carts.FirstOrDefault(c => c.MaSP == productID);
                if(cart == null)
                {
                    return RedirectToAction("ShopCart", "Home");
                }
                else
                {
                    db.Carts.Remove(cart);
                    db.SaveChanges();
                }
            }catch(Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
            return RedirectToAction("ShopCart", "Home");
        }

        //THANH TOÁN ĐƠN HÀNG VÀ LƯU DỮ LIỆU
        public ActionResult DonHang(FormCollection form)
        {
            int MaTK = Convert.ToInt32(Session["MaTK"]);
            //KIỂM TRA ĐĂNG NHẬP
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            List<CartItem> items = new List<CartItem>();
            try
            {
                int HinhThucTT = int.Parse(form["HinhThucTT"]);
                DonHang donhang = new DonHang();
                //LẤY DANH SÁCH SẢN PHẨM CÓ TRONG GIỎ HÀNG CỦA KHÁCH HÀNG
                items = GetListCartItem(MaTK);

                donhang.NgayDat = DateTime.Now;
                donhang.DiaChiGH = form["DiaChi"];
                donhang.TongTien = TotalPriceCarts(items);
                donhang.MaTK = MaTK;
                donhang.TinhTrang = 0;
                donhang.HinhThucTT = int.Parse(form["HinhThucTT"]);

                //KIỂM TRA HÌNH THỨC THANH TOÁN ĐỂ XỬ LÝ
                //0 - THANH TOÁN KHI NHẬN HÀNG
                //1 - THANH TOÁN QUA VÍ ĐIỆN TỬ MOMO
                if (HinhThucTT == 0)
                {
                    db.DonHangs.Add(donhang);
                    db.SaveChanges();

                    //THÊM CÁC CHI TIẾT ĐƠN HÀNG VÀO DB 
                    foreach (var item in items)
                    {
                        ChiTietDH chitietHD = new ChiTietDH();
                        chitietHD.SoDH = donhang.SoDH;
                        chitietHD.MaSP = item.Product.MaSP;
                        chitietHD.SoLuong = item.Cart.quantity;
                        db.ChiTietDHs.Add(chitietHD);
                        db.SaveChanges();
                    }

                    //XÓA CÁC CART KHI TẠO XONG ĐƠN HÀNG
                    foreach (var item in items)
                    {
                        db.Carts.Remove(item.Cart);
                        db.SaveChanges();
                    }

                    return RedirectToAction("DanhsachDonhang", "Home");
                }
                else
                {
                    //THIẾT LẬP DỮ LIỆU CHO THANH TOÁN BẰNG MOMO
                    string endpoint = ConfigurationManager.AppSettings["endpoint"].ToString();
                    string partnerCode = ConfigurationManager.AppSettings["partnerCode"].ToString();
                    string accessKey = ConfigurationManager.AppSettings["accessKey"].ToString();
                    string serectKey = ConfigurationManager.AppSettings["serectKey"].ToString();
                    string orderInfo = "DH" + DateTime.Now.ToString();
                    string returnUrl = ConfigurationManager.AppSettings["returnUrl"].ToString();
                    string notifyurl = ConfigurationManager.AppSettings["notifyUrl"].ToString();
                    string amount = "10000";
                    string orderid = Guid.NewGuid().ToString();
                    string requestId = Guid.NewGuid().ToString();
                    string extraData = "";
                    string rawHash = "partnerCode=" + partnerCode + "&accessKey=" + accessKey
                        + "&requestId=" + requestId + "&amount=" + amount + "&orderId=" + orderid
                        + "&orderInfo=" + orderInfo + "&returnUrl=" + returnUrl + "&notifyUrl=" + notifyurl
                        + "&extraData=" + extraData;
                    MoMoSecurity crypto = new MoMoSecurity();
                    string signature = crypto.signSHA256(rawHash, serectKey);
                    JObject message = new JObject
                    {
                        {"partnerCode", partnerCode },
                        { "accessKey", accessKey },
                        { "requestId", requestId },
                        { "amount", amount },
                        { "orderId", orderid },
                        { "orderInfo", orderInfo },
                        { "returnUrl", returnUrl },
                        { "notifyUrl", notifyurl },
                        { "requestType", "captureMoMoWallet" },
                        { "signature", signature }
                    };

                    string responeFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                    JObject jmessage = JObject.Parse(responeFromMomo);
                    Session["donhang"] = donhang;
                    return Redirect(jmessage.GetValue("payUrl").ToString());
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
        }

        //PARTIAL VIEW HIỂN THỊ CHI TIẾT ĐƠN
        public PartialViewResult ChiTietDonHang(int MaTK)
        {
            var taiKhoan = db.TaiKhoans.Find(MaTK);
            return PartialView(taiKhoan);
        }

        //DANH SÁCH ĐƠN HÀNG
        public ActionResult DanhsachDonhang()
        {
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            int MaTK = Convert.ToInt32(Session["MaTK"]);
            List<DonHang> donHangs = new List<DonHang>();
            try
            {
                donHangs = db.DonHangs.Where(dh => dh.MaTK == MaTK).ToList();
            }
            catch(Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
            
            return View(donHangs);
        }

        //HỦY ĐƠN HÀNG
        public ActionResult HuyDon(int id)
        {
            string error = "";
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            DonHang donHang = new DonHang();
            try
            {
                donHang = db.DonHangs.FirstOrDefault(dh => dh.SoDH == id);
                if(donHang != null)
                {
                    if(donHang.TinhTrang != 2)
                    {
                        donHang.TinhTrang = 3;
                        db.DonHangs.AddOrUpdate(donHang);
                        db.SaveChanges();
                    }
                    else
                    {
                        error = "Đơn hàng đã giao không thể hủy";
                    }
                    
                }
            }catch(Exception e)
            {
                error = "Hủy đơn không thành công vui lòng thử lại sau";
            }
            if(error != "")
            {
                TempData["ErrorCancel"] = error;
                return RedirectToAction("DanhsachDonhang", "Home", error);
            }
            
            return RedirectToAction("DanhsachDonhang", "Home");
        }
        
        //CHI TIẾT ĐƠN HÀNG
        public ActionResult ChiTietDon(int? id)
        {
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            List<ChiTietDon> chiTietDons = new List<ChiTietDon>();
            List<ChiTietDH> chiTietDH = new List<ChiTietDH>();
            List<SanPham> sanPhams = new List<SanPham>();

            if (id == null)
            {
                return View();
            }

            try
            {
                chiTietDH = db.ChiTietDHs.Where(c => c.SoDH == id).ToList();
                if(chiTietDH != null)
                {
                    for (int i = 0; i < chiTietDH.Count; i++)
                    {
                        int MaSP = chiTietDH[i].MaSP;
                        SanPham sanPham = db.SanPhams.FirstOrDefault(sp => sp.MaSP == MaSP);
                        sanPhams.Add(sanPham);
                    }
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }

            if (chiTietDH != null && chiTietDH.Count > 0)
            {
                for (int i = 0; i < chiTietDH.Count; i++)
                {

                    chiTietDons.Add(new ChiTietDon
                    {
                        sanPham = sanPhams[i],
                        chiTietDH = chiTietDH[i]
                    });
                }
            }

            return View(chiTietDons);
        }

        //MENU SHOP
        public ActionResult Shop(int? page, int? MaDM, String searchString)
        {
            List<SanPham> sanPhams = new List<SanPham>();

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            try
            {
                if (MaDM == null)
                {
                    sanPhams = db.SanPhams.OrderByDescending(s => s.NgayTao).Select(s => s).ToList();
                }
                else if (MaDM != null)
                {
                    sanPhams = db.SanPhams.Where(s => s.MaDM == MaDM).OrderByDescending(s => s.NgayTao).Select(s => s).ToList();
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    sanPhams = sanPhams.Where(t => t.TenSP.ToLower().Contains(searchString.ToLower())).ToList();
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }
            

            return View(sanPhams.ToPagedList(pageNumber, pageSize));
        }

        //ĐĂNG XUẤT
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        //LIÊN HỆ
        public ActionResult Contact()
        {
            LienHe lienHe = new LienHe();
            try
            {
                lienHe = db.LienHes.FirstOrDefault();
            }
            catch (Exception e)
            {
                return RedirectToAction("ServerError", "Error");
            }


            return View(lienHe);

        }
        //TIẾN HÀNH THANH TOÁN
        public ActionResult Checkout()
        {
            if (Session["MaTK"] == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            List<CartItem> items = new List<CartItem>();
            int MaTK = Convert.ToInt32(Session["MaTK"]);
            int total = 0;

            items = GetListCartItem(MaTK);
            if (items.Count > 0)
            {
                total = TotalPriceCarts(items);
            }

            ViewData["Total"] = total;
            return View(items);
        }

        //HIỂN THỊ KẾT QUẢ SAU KHI THỰC HIỆN THANH TOÁN BẰNG MOMO
        public ActionResult ReturnUrl()
        {
            if (Session["MaTK"] != null && Session["donhang"] != null)
            {
                string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
                param = Server.UrlDecode(param);
                MoMoSecurity crypto = new MoMoSecurity();
                string serectKey = ConfigurationManager.AppSettings["serectKey"].ToString();
                string signature = crypto.signSHA256(param, serectKey);
                if (signature != Request["signature"].ToString())
                {
                    ViewData["Result"] = "Thanh toán thất bại. Vui lòng thử lại sau!";
                    return View();
                }

                if (!Request.QueryString["errorCode"].Equals("0"))
                {
                    ViewData["Result"] = "Thanh toán thất bại. Vui lòng thử lại sau!";
                    return View();
                }

                int MaTK = Convert.ToInt32(Session["MaTK"]);
                DonHang donHang = new DonHang();
                donHang = Session["donhang"] as DonHang;

                List<CartItem> items = new List<CartItem>();
                try
                {
                    items = GetListCartItem(MaTK);
                    db.DonHangs.Add(donHang);
                    db.SaveChanges();

                    //THEM CHI TIET DON HANG
                    foreach (var item in items)
                    {
                        ChiTietDH chitietHD = new ChiTietDH();
                        chitietHD.SoDH = donHang.SoDH;
                        chitietHD.MaSP = item.Product.MaSP;
                        chitietHD.SoLuong = item.Cart.quantity;
                        db.ChiTietDHs.Add(chitietHD);
                        db.SaveChanges();
                    }

                    //XÓA CÁC CART KHI TẠO XONG ĐƠN HÀNG
                    foreach (var item in items)
                    {
                        db.Carts.Remove(item.Cart);
                        db.SaveChanges();
                    }

                    ViewData["Result"] = "Thanh toán thành công";
                    return View();
                }
                catch
                {
                    ViewData["Result"] = "Thanh toán thất bại. Vui lòng thử lại sau!";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }

        //URL CHẠY NGẦM ĐỂ THIẾT LẬP CHO THANH TOÁN MOMO
        public JsonResult NotifyUrl()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }

        //HIỂN THỊ DỮ LIỆU CHO SIDEBAR
        public PartialViewResult shop_sidebar()
        {
            var danhMucs = db.DanhMucs.Select(s => s);

            return PartialView(danhMucs);
        }

        //HIỂN THỊ DỮ LIỆU CHO HEADER
        public PartialViewResult _Header()
        {
            var danhMucs = db.DanhMucs.Select(s => s);

            return PartialView(danhMucs);
        }

        //SẢN PHẨM LIÊN QUAN TRONG TRANG CHI TIẾT SẢN PHẨM
        public PartialViewResult _SanPhamLienQuan(int MaDM)
        {
            var sanPhamLienQuans = db.SanPhams.Where(s => s.MaDM == MaDM && s.SoLuong > 0).Select(s => s);

            return PartialView(sanPhamLienQuans);
        }

        //HÀM TÍNH TỔNG TIỀN CHO GIỎ HÀNG
        public int TotalPriceCarts(List<CartItem> cartItems)
        {
            var total = 0;
            foreach (var item in cartItems)
            {
                int giaKM = 0;
                int giaBan = item.Product.GiaBan;
                if ((item.Product.PhanTramKM != null || item.Product.PhanTramKM.ToString().Trim() != "") && item.Product.PhanTramKM > 0)
                {
                    int phanTramKM = int.Parse(item.Product.PhanTramKM.ToString());
                    giaKM = giaBan - giaBan * phanTramKM / 100;
                }
                else
                {
                    giaKM = giaBan;
                }
                total += giaKM * item.Cart.quantity;
            }

            return (int)total;
        }

        //HÀM LẤY DANH SÁCH CÁC SẢN PHẨM TRONG GIỎ HÀNG
        public List<CartItem> GetListCartItem(int MaTK)
        {
            List<Cart> carts = new List<Cart>();
            List<SanPham> sanPhams = new List<SanPham>();
            List<CartItem> items = new List<CartItem>();

            try
            {
                carts = db.Carts.Where(c => c.MaTK == MaTK).ToList();

                for (int i = 0; i < carts.Count; i++)
                {
                    int MaSP = carts[i].MaSP;
                    SanPham sanPham = db.SanPhams.FirstOrDefault(sp => sp.MaSP == MaSP);
                    sanPhams.Add(sanPham);
                }

            }
            catch (Exception e)
            {

            }

            if (carts.Count > 0)
            {
                for (int i = 0; i < carts.Count; i++)
                {

                    items.Add(new CartItem
                    {
                        Product = sanPhams[i],
                        Cart = carts[i]
                    });
                }
            }

            return items;
        }
    }
}