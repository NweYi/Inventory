using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Controllers
{
    public class SettingController : Controller
    {
        // GET: Setting
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Startup()
        {
            return View();
        }
    }
}