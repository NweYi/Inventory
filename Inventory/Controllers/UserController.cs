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

        InventoryDBEntities Entities = new InventoryDBEntities();
        //GET : User Data
        public ActionResult UserList()
        {
            GetisMultiBranch();
            return View(GetUserList().ToList());
        }
        public ActionResult CreateUser()
        {
            GetisMultiBranch();
            if(ViewBag.isMultiBranch==true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }            
            var locations = Entities.S_Location.ToList();
            ViewBag.Locations = new SelectList(locations, "LocationID", "LocationName");
            ViewBag.divLocation =false;
            return View();
        }
        [HttpPost]
        public ActionResult InsertUser(UserModels.UserModel user_model)
        {
            GetisMultiBranch();
            if(ViewBag.isMultiBranch==true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }           
            var locations = Entities.S_Location.ToList();
            ViewBag.Locations = new SelectList(locations, "LocationID", "LocationName");            
            try
            {
                S_User tbl_user = new S_User();
                tbl_user.UserName = user_model.UserName;
                tbl_user.UserPassword = user_model.UserPassword;
                if (user_model.BranchID != 0) tbl_user.BranchID = user_model.BranchID;
                else tbl_user.BranchID = null;
                if (user_model.IsDefaultLocation == true) tbl_user.IsDefaultLocation = true;
                else tbl_user.IsDefaultLocation = false;
                if (user_model.LocationID != 0) tbl_user.LocationID = user_model.LocationID;
                else tbl_user.LocationID = null;
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
        public JsonResult DeleteUser(int id)
        {
            var user = Entities.S_User.Find(id);
            bool result = false;
            if(user!=null)
            {
                Entities.S_User.Remove(user);
                Entities.SaveChanges();
                ViewBag.Message = "User Delete Successful...";
                result= true;
            }
            //GetisMultiBranch();
            //return View("UserList",GetUserList().ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SearchingUser(string username)
        {
            GetisMultiBranch();
            List<UserModels.UserModel> result = new List<UserModels.UserModel>();
            if(username!=null)
            {
                try
                {
                    result = SearchUserData(username);                
                }
                catch(FormatException)
                {
                    Console.WriteLine("{0} is not a data");
                }
                var data = new { data1=result,data2=isMultiBranch()};
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditUser(int id)
        {
            var user = Entities.S_User.Find(id);
            GetisMultiBranch();
            if (user!=null)
            {               
                if(ViewBag.isMultiBranch==true)
                {
                    var branches = Entities.S_Branch.ToList();
                    ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
                }                
                var locations = Entities.S_Location.ToList();
                ViewBag.Locations = new SelectList(locations, "LocationID", "LocationName");

                UserModels.UserModel user_model = new UserModels.UserModel();
                user_model.UserID = Convert.ToInt32(user.UserID);
                user_model.UserName = Convert.ToString(user.UserName);
                user_model.UserPassword = Convert.ToString(user.UserPassword);
                if (user.BranchID != null) user_model.BranchID = Convert.ToInt32(user.BranchID);
                if (user.IsDefaultLocation != null)
                {
                    user_model.IsDefaultLocation = Convert.ToBoolean(user.IsDefaultLocation);
                    ViewBag.divLocation =user.IsDefaultLocation;
                } 
                if (user.LocationID != null) user_model.LocationID = Convert.ToInt32(user.LocationID);
                ViewBag.FormType = 2;
                return View("CreateUser", user_model);
            }
            else
            {
                return View("UserList",GetUserList().ToList());
            }            
        }
        [HttpPost]
        public ActionResult UpdateUser(UserModels.UserModel user_model)
        {
            if(user_model.BranchID==0 && user_model.LocationID==0)
            {
                Entities.PrcUpdateUserData(user_model.UserID, user_model.UserName, user_model.UserPassword,null, user_model.IsDefaultLocation,null);
            }
            else if(user_model.BranchID!=0&&user_model.LocationID==0)
            {
                Entities.PrcUpdateUserData(user_model.UserID, user_model.UserName, user_model.UserPassword, user_model.BranchID, user_model.IsDefaultLocation, null);
            }
            else if(user_model.BranchID == 0 && user_model.LocationID != 0)
            {
                Entities.PrcUpdateUserData(user_model.UserID, user_model.UserName, user_model.UserPassword,null, user_model.IsDefaultLocation, user_model.LocationID);
            }
            else
            {
                Entities.PrcUpdateUserData(user_model.UserID, user_model.UserName, user_model.UserPassword, user_model.BranchID, user_model.IsDefaultLocation, user_model.LocationID);
            }
            ViewBag.Message = "User Updated Successful...";
            GetisMultiBranch();       
            return View("UserList",GetUserList().ToList());
        }
        
        #region Methods
        public void GetisMultiBranch()
        {
            CompanySettingModels cModel = new CompanySettingModels();
            var isMultiBranch = Entities.S_CompanySetting.Select(company => company.IsMultiBranch);
            cModel.IsMultiBranch = isMultiBranch.FirstOrDefault();
            ViewBag.isMultiBranch = cModel.IsMultiBranch;
        }
        public bool isMultiBranch()
        {
            CompanySettingModels cModel = new CompanySettingModels();
            var isMultiBranch = Entities.S_CompanySetting.Select(company => company.IsMultiBranch);
            cModel.IsMultiBranch = isMultiBranch.FirstOrDefault();
            return Convert.ToBoolean(cModel.IsMultiBranch);
        }
        public List<UserModels.UserModel>GetUserList()
        {
            var user_model = (from U in Entities.S_User
                              join B in Entities.S_Branch on U.BranchID equals B.BranchID into LBranch
                              from leftBranch in LBranch.DefaultIfEmpty()
                              join L in Entities.S_Location on U.LocationID equals L.LocationID into LUser
                              from leftUser in LUser.DefaultIfEmpty()
                              select new UserModels.UserModel
                              {
                                  UserID = U.UserID,
                                  UserName = U.UserName,
                                  UserPassword = U.UserPassword,
                                  BranchName = leftBranch.BranchName,
                                  LocationName = leftUser.LocationName
                              }).ToList();
            return user_model.ToList();
        }
        public List<UserModels.UserModel>SearchUserData(string search)
        {
            var user= (from U in Entities.S_User
                       join B in Entities.S_Branch on U.BranchID equals B.BranchID into LBranch
                       from leftBranch in LBranch.DefaultIfEmpty()
                       join L in Entities.S_Location on U.LocationID equals L.LocationID into LUser
                       from leftUser in LUser.DefaultIfEmpty()
                       where U.UserName.Contains(search)
                       select new UserModels.UserModel
                       {
                           UserID = U.UserID,
                           UserName = U.UserName,
                           UserPassword = U.UserPassword,
                           BranchName = leftBranch.BranchName,
                           LocationName = leftUser.LocationName
                       }).ToList();
            return user.ToList();
        }
        #endregion
    }
}