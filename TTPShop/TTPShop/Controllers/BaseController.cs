using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TTPShop.Models;

namespace TTPShop.Controllers
{
    public class BaseController : Controller
    {
        private TTPShopDB db = new TTPShopDB();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = Session["idUser"];

            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Home", action = "Login", Area = "Client" }));
            }
            base.OnActionExecuting(filterContext);
            //
        }
    }
}