using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            using (InventoryDBEntities entity = new InventoryDBEntities())
            {
                var fromDatabaseEF = new SelectList(entity.S_User.ToList(), "UserID", "UserName");
                ViewData["UserList"] = fromDatabaseEF;
            }
            return View();
        }
    }
}