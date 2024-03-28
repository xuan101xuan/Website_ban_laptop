using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TTPShop.Models;

namespace TTPShop.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        private TTPShopDB db = new TTPShopDB();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = Session["MaTK"];
            TaiKhoan u;
            try
            {
                u = db.TaiKhoans.Find(session);
            }catch(Exception e)
            {
                u = new TaiKhoan();
            }
            

            if (session == null || u.VaiTro != "Administrator")
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Home", action = "Login", Area = "" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}