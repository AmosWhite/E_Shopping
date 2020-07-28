using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Custom_Security;

namespace WebApp.Areas.Admin.Controllers
{
    [CustomAuthorization(Role = "Admin")]
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}