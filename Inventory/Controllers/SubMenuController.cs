using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory.Models;

namespace Inventory.Controllers
{
    public class SubMenuController : Controller
    {
        InventoryDBEntities Entities = new InventoryDBEntities();
        // GET: SubMenu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SubMenuList()
        {
            GetDDLMainMenu();
            return View(GetSubMenu());
        }
        public ActionResult SubMenuEntry()
        {
            GetCompanySetting();
            GetDDLMainMenu();
            return View();
        }
        public ActionResult InsertSubMenu(SubMenuModels.SubMenuModel subMenu,HttpPostedFileBase file)
        {
            GetCompanySetting();
            GetDDLMainMenu();
            var mCode = Entities.S_SubMenu.Where(m => m.Code == subMenu.Code).FirstOrDefault();
            var mSortCode = Entities.S_SubMenu.Where(m => m.SortCode == subMenu.SortCode).FirstOrDefault();
            if(mCode==null&&mSortCode==null)
            {
                S_SubMenu tblSubMenu = new S_SubMenu();
                tblSubMenu.SubMenuName = subMenu.SubMenuName;
                tblSubMenu.Code = subMenu.Code;
                tblSubMenu.SortCode = subMenu.SortCode;
                if (subMenu.MainMenuID != 0) tblSubMenu.MainMenuID = subMenu.MainMenuID;
                else tblSubMenu.MainMenuID = null;               
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        subMenu.Photo = new byte[file.ContentLength];
                        file.InputStream.Read(subMenu.Photo, 0, file.ContentLength);
                        tblSubMenu.Photo = subMenu.Photo;
                    }
                }
                else
                {
                    tblSubMenu.Photo = null;
                }
                Entities.S_SubMenu.Add(tblSubMenu);
                Entities.SaveChanges();
                ViewBag.Message = "New Sub Menu Inserted Successful....";
                ViewBag.Type = 1;
                ModelState.Clear();
            }
            else
            {
                if(mCode != null && mSortCode == null) ViewBag.Message = "Sub Menu Code Duplicated";
                else if(mCode == null && mSortCode != null) ViewBag.Message = "Sub Menu Sort Code Duplicated";
                else ViewBag.Message = "Sub Menu Code and Sort Code Duplicated";
                ViewBag.Type = 2;
            }
            return View("SubMenuEntry");
        }
        public JsonResult DeleteSubMenu(int id)
        {
            bool result = false;
            var sMenu = Entities.S_SubMenu.Find(id);
            if(sMenu!=null)
            {
                Entities.S_SubMenu.Remove(sMenu);
                Entities.SaveChanges();
                ViewBag.Message = "Sub Menu Delete Successful..";
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditSubMenu(int id)
        {
            var sMenu = Entities.S_SubMenu.Find(id);
            if (sMenu != null)
            {
                SubMenuModels.SubMenuModel model = new SubMenuModels.SubMenuModel();
                model.SubMenuID = sMenu.SubMenuID;
                model.SubMenuName = sMenu.SubMenuName;
                model.Code = sMenu.Code;
                model.SortCode = Convert.ToInt32(sMenu.SortCode);
                model.MainMenuID = Convert.ToInt32(sMenu.MainMenuID);
                if (sMenu.Photo == null) ViewBag.Photo = false;
                else ViewBag.Photo = true;
                model.Photo = sMenu.Photo;
                ViewBag.formType = 2;
                GetCompanySetting();
                GetDDLMainMenu();
                return View("SubMenuEntry", model);
            }
            else return View("SubMenuList", GetSubMenu());
        }
        public ActionResult UpdateSubMenu(SubMenuModels.SubMenuModel subMenu,HttpPostedFileBase file)
        {
            var sCode = Entities.S_SubMenu.Where(m => m.SubMenuID != subMenu.SubMenuID && m.Code == subMenu.Code).FirstOrDefault();
            var sSortCode = Entities.S_SubMenu.Where(m => m.SubMenuID!= subMenu.SubMenuID && m.SortCode == subMenu.SortCode).FirstOrDefault();
            if(sCode==null && sSortCode==null)
            {
                if(file!=null)
                {
                    if (file.ContentLength > 0)
                    {
                        subMenu.Photo = new byte[file.ContentLength];
                        file.InputStream.Read(subMenu.Photo, 0, file.ContentLength);
                        subMenu.Photo = subMenu.Photo;
                        Entities.PrcUpdateSubMenu(subMenu.SubMenuID, subMenu.SubMenuName, subMenu.Code, subMenu.SortCode, subMenu.Photo, subMenu.MainMenuID, 2);
                    }
                }
                else
                {
                    Entities.PrcUpdateSubMenu(subMenu.SubMenuID, subMenu.SubMenuName, subMenu.Code, subMenu.SortCode, subMenu.Photo, subMenu.MainMenuID, 1);
                }                
                ViewBag.Message = "Sub menu updated successful...";
                return View("SubMenuList", GetSubMenu());
            }
            else
            {
                if (sCode != null && sSortCode != null) ViewBag.Message = "Code and Sort Code Duplicated.";
                else if (sCode != null && sSortCode == null) ViewBag.Message = "Code duplicated.";
                else ViewBag.Message = "SortCode Duplicated...";
                ViewBag.formType = 2;
                GetCompanySetting();
                GetDDLMainMenu();
                return View("SubMenuEntry", subMenu);
            }
        }
        public JsonResult SearchSubMenu(string sMenu,string mMenuID)
        {
            if (mMenuID == "") mMenuID = "0";
            return Json(SearchingSubMenu(sMenu,Convert.ToInt32(mMenuID)),JsonRequestBehavior.AllowGet);
        }
        #region Methods
        public List<SubMenuModels.SubMenuModel>GetSubMenu()
        {
            List<SubMenuModels.SubMenuModel> lstSubMenu = new List<SubMenuModels.SubMenuModel>();
            var sMenu = Entities.PrcRetrieveSubMenu();
            foreach(var item in sMenu)
            {
                SubMenuModels.SubMenuModel model = new SubMenuModels.SubMenuModel();
                model.SubMenuID = item.SubMenuID;
                model.SubMenuName = item.SubMenuName;
                model.SortCode = Convert.ToInt32(item.SortCode);
                model.Code = item.Code;
                if (item.MainMenuName == null) model.MainMenuName = "-";
                else model.MainMenuName = item.MainMenuName;
                lstSubMenu.Add(model);
            }
            return lstSubMenu;
        }
        public List<SubMenuModels.SubMenuModel>SearchingSubMenu(string sMenu,int mMenuID)
        {
            List<SubMenuModels.SubMenuModel> lstSubMenu = new List<SubMenuModels.SubMenuModel>();
            var subMenu = Entities.PrcSearchSubMenu(sMenu, mMenuID);
            foreach(var item in subMenu)
            {
                SubMenuModels.SubMenuModel model = new SubMenuModels.SubMenuModel();
                model.SubMenuID = item.SubMenuID;
                model.SubMenuName = item.SubMenuName;
                model.SortCode = Convert.ToInt32(item.SortCode);
                model.Code = item.Code;
                if (item.MainMenuName == null) model.MainMenuName = "-";
                else model.MainMenuName = item.MainMenuName;
                lstSubMenu.Add(model);
            }
            return lstSubMenu;
        }
        public void GetCompanySetting()
        {
            CompanySettingModels cModel = new CompanySettingModels();
            var isProductPhoto = Entities.S_CompanySetting.Select(company => company.IsProductPhoto);
            cModel.IsProductPhoto = isProductPhoto.FirstOrDefault();
            ViewBag.isProductPhoto = cModel.IsProductPhoto;
        }
        public void GetDDLMainMenu()
        {
            var mMenu = Entities.S_MainMenu.ToList();
            ViewBag.MainMenu = new SelectList(mMenu, "MainMenuID", "MainMenuName");
        }
        #endregion
    }
}