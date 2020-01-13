using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory.Models;
using System.Data.Entity.Core.Objects;

namespace Inventory.Controllers
{
    public class UserController : MyController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeLanguage(string lang)
        {
            new LanguageMang().SetLanguage(lang);
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult Login()
        {
            InventoryDBEntities entity = new InventoryDBEntities();
            UserModels.LoginUserModel model = new UserModels.LoginUserModel();
            CompanySettingModels cModel = new CompanySettingModels();

            var isMultiBranch = entity.S_CompanySetting.Select(c => c.IsMultiBranch);
            cModel.IsMultiBranch = isMultiBranch.FirstOrDefault();
            ViewBag.IsMultiBranch = cModel.IsMultiBranch;

            int? firstBranchId = 0;

            if (cModel.IsMultiBranch == true)
            {
                foreach (var branch in entity.S_Branch)
                {
                    model.Branches.Add(new SelectListItem { Text = branch.BranchName, Value = branch.BranchID.ToString() });
                }
                for (int i = 0; i < model.Branches.Count(); i++)
                {
                    firstBranchId = Convert.ToInt32(model.Branches[i].Value);
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
            }else
            {
                var users = (from user in entity.S_User select user).ToList();
                foreach (var user in users)
                {
                    model.Users.Add(new SelectListItem { Text = user.UserName, Value = user.UserID.ToString() });
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(int? branchId, int userId, string userPassword, string userName, string branchName, bool clickedLogin)
        {
            InventoryDBEntities entity = new InventoryDBEntities();
            UserModels.LoginUserModel model = new UserModels.LoginUserModel();
            CompanySettingModels cModel = new CompanySettingModels();

            var isMultiBranch = entity.S_CompanySetting.Select(c => c.IsMultiBranch);
            cModel.IsMultiBranch = isMultiBranch.FirstOrDefault();
            ViewBag.IsMultiBranch = cModel.IsMultiBranch;

            if (clickedLogin)
            {
                int? result = Convert.ToInt32(entity.PrcValidateUser(userId, userPassword).FirstOrDefault());

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

            if (cModel.IsMultiBranch == true)
            {
                foreach (var branch in entity.S_Branch)
                {
                    model.Branches.Add(new SelectListItem { Text = branch.BranchName, Value = branch.BranchID.ToString() });
                }
                if (branchId.HasValue)
                {
                    var users = (from user in entity.S_User where user.BranchID == branchId.Value select user).ToList();
                    foreach (var user in users)
                    {
                        model.Users.Add(new SelectListItem { Text = user.UserName, Value = user.UserID.ToString() });
                    }
                }
            }else
            {
                var users = (from user in entity.S_User select user).ToList();
                foreach (var user in users)
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
            List<UserModels.UserModel> lstUser = new List<UserModels.UserModel>();
            //user_model.lstUser = Entities.Database.SqlQuery<UserModels.UserModel>("exec PrcRetrieveUser").ToList();
            foreach (var user in Entities.PrcRetrieveUser())
            {
                UserModels.UserModel model = new UserModels.UserModel();
                model.UserID = Convert.ToInt32(user.UserID);
                model.UserName = Convert.ToString(user.UserName);
                model.UserPassword = Convert.ToString(user.UserPassword);
                model.BranchID = Convert.ToInt32(user.BranchID);
                model.BranchName = Convert.ToString(user.BranchName);
                model.IsDefaultLocation = Convert.ToBoolean(user.IsDefaultLocation);
                model.LocationID = Convert.ToInt32(user.LocationID);
                model.LocationName = Convert.ToString(user.LocationName);
                lstUser.Add(model);
            }
            return View(lstUser);
        }
        public ActionResult CreateUser()
        {
            InventoryDBEntities Entities = new InventoryDBEntities();
            var branches = Entities.S_Branch.ToList();
            ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            var locations = Entities.S_Location.ToList();
            ViewBag.Locations = new SelectList(locations, "LocationID", "LocationName");
            return View();
        }
        [HttpPost]
        public ActionResult InsertUser(UserModels.UserModel user_model)
        {
            InventoryDBEntities Entities = new InventoryDBEntities();
            var branches = Entities.S_Branch.ToList();
            ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            var locations = Entities.S_Location.ToList();
            ViewBag.Locations = new SelectList(locations, "LocationID", "LocationName");
            try
            {
                S_User tbl_user = new S_User();
                tbl_user.UserName = user_model.UserName;
                tbl_user.UserPassword = user_model.UserPassword;
                tbl_user.BranchID = user_model.BranchID;
                if (user_model.IsDefaultLocation == true) tbl_user.IsDefaultLocation = true;
                else tbl_user.IsDefaultLocation = false;
                if (user_model.LocationID == 0) tbl_user.LocationID = null;
                else tbl_user.LocationID = user_model.LocationID;
                Entities.S_User.Add(tbl_user);
                Entities.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = "New User Inserted Successful...";
                ViewBag.AlertType = "1";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please Enter UserName and Password.";
                ViewBag.AlertType = "2";
                ModelState.Clear();
                return View("CreateUser");
            }
            return View("CreateUser");
        }
        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            InventoryDBEntities Entities = new InventoryDBEntities();
            var user = Entities.S_User.Find(id);
            if(user!=null)
            {
                Entities.S_User.Remove(user);
                Entities.SaveChanges();
            }
            return RedirectToAction("UserList");
        }
        public ActionResult EditUser(int id)
        {
            InventoryDBEntities Entities = new InventoryDBEntities();
            var user = Entities.S_User.Find(id);
            if(user!=null)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
                var locations = Entities.S_Location.ToList();
                ViewBag.Locations = new SelectList(locations, "LocationID", "LocationName");

                UserModels.UserModel user_model = new UserModels.UserModel();
                user_model.UserID = Convert.ToInt32(user.UserID);
                user_model.UserName = Convert.ToString(user.UserName);
                user_model.UserPassword = Convert.ToString(user.UserPassword);
                if (user.BranchID != null) user_model.BranchID = Convert.ToInt32(user.BranchID);
                if (user.IsDefaultLocation != null) user_model.IsDefaultLocation = Convert.ToBoolean(user.IsDefaultLocation);
                if (user.LocationID != null) user_model.LocationID = Convert.ToInt32(user.LocationID);
                ViewBag.FormType = 2;
                return View("CreateUser", user_model);
            }
            else
            {
                return View("UserList");
            }            
        }
        [HttpPost]
        public ActionResult UpdateUser(UserModels.UserModel user_model)
        {
            InventoryDBEntities Entities = new InventoryDBEntities();
            Entities.PrcUpdateUserData(user_model.UserID, user_model.UserName, user_model.UserPassword, user_model.BranchID, user_model.IsDefaultLocation, user_model.LocationID);
            return RedirectToAction("UserList");
        }
    }
}