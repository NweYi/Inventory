using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory.Models;
using System.Data.Entity.Core.Objects;

namespace Inventory.Controllers
{
    public class UserController : Controller
    { 
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            //using (InventoryDBEntities entity = new InventoryDBEntities())
            //{
            //    var branches = new SelectList(entity.S_Branch.ToList(), "BranchID", "Name");
            //    ViewData["BranchList"] = branches;

            //    var users = new SelectList(entity.S_User.ToList(),"UserID","UserName");
            //    ViewData["UserList"] = users;
            //}
            InventoryDBEntities entity = new InventoryDBEntities();
            UserModels.LoginUserModel model = new UserModels.LoginUserModel();
            int? firstBranchId = 0;
            foreach (var branch in entity.S_Branch)
            {
                model.Branches.Add(new SelectListItem { Text = branch.BranchName, Value = branch.BranchID.ToString() });
            }
            for(int i = 0; i < model.Branches.Count();i++)
            {
                firstBranchId =Convert.ToInt32(model.Branches[i].Value);
                break;
            }
            if (firstBranchId.HasValue)
            {
                var users = (from user in entity.S_User where user.BranchID == firstBranchId.Value select user).ToList();
                foreach (var user in users)
                {
                    model.Users.Add(new SelectListItem { Text = user.UserName, Value = user.UserID.ToString() });
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(int? branchId,int userId,string userPassword,string userName,string branchName,bool clickedLogin)
        {
            InventoryDBEntities entity = new InventoryDBEntities();
            UserModels.LoginUserModel model = new UserModels.LoginUserModel();

            if (clickedLogin)
            {
                int? result =Convert.ToInt32(entity.PrcValidateUser(userId, userPassword).FirstOrDefault());

                switch (result.Value)
                {
                    case 1:
                        Session["LoginUserID"] = userId;
                        Session["LoginUserName"] = userName;
                        Session["LoginBranchID"] = branchId;
                        Session["LoginBranchName"] = branchName;
                        return RedirectToAction("Index", "Home");

                    case -1:
                        ViewBag.Message = "Password is incorrect.";
                        break;
                 
                    default: break;
                }
                
            }
           
            foreach (var branch in entity.S_Branch)
            {
                model.Branches.Add(new SelectListItem { Text = branch.BranchName, Value = branch.BranchID.ToString() });
            }
            if (branchId.HasValue)
            {
                var users = (from user in entity.S_User where user.BranchID == branchId.Value select user).ToList();
                foreach(var user in users)
                {
                    model.Users.Add(new SelectListItem { Text = user.UserName, Value = user.UserID.ToString() });
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AdminLogin(string adminName, string adminPassword)
        {
            InventoryDBEntities entity = new InventoryDBEntities();

            int? result = Convert.ToInt32(entity.PrcValidateAdmin(adminName, adminPassword).FirstOrDefault());

            switch (result.Value)
            {
                case 1:
                    Session["LoginUserName"] = adminName;
                    return RedirectToAction("Index", "Home");
                case -1:
                    ViewBag.Message = "Invalid Admin.";
                    break;

                default: break;
            }

            return View();
        }

        //GET : User Data
        public ActionResult UserList()
        {
            InventoryDBEntities Entities = new InventoryDBEntities();
            UserModels user_model = new UserModels();
            user_model.lstUser = Entities.Database.SqlQuery<UserModels.UserModel>("exec PrcRetrieveUser").ToList();
            return View(user_model);
        }
    }
}