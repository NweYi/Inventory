using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory.Models;

namespace Inventory.Controllers
{
    public class MainMenuController : MyController
    {
        InventoryDBEntities Entities = new InventoryDBEntities();
        // GET: MainMenu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MainMenuList()
        {            
            return View(GetMainMenu().ToList());
        }
        public ActionResult MainMenuEntry()
        {
            GetCompanySetting();
            ViewBag.Photo = false;
            return View();
        }
        [HttpPost]
        public ActionResult InsertMainMenu(MainMenuModels.MainMenuModel model,HttpPostedFileBase file)
        {
            GetCompanySetting();
            var menuCode = Entities.S_MainMenu.Where(m => m.Code == model.Code).FirstOrDefault();
            var mSortCode = Entities.S_MainMenu.Where(m => m.SortCode == model.SortCode).FirstOrDefault();
            if (menuCode == null&&mSortCode==null)
            {
                S_MainMenu tblMainMenu = new S_MainMenu();
                tblMainMenu.MainMenuName = model.MainMenuName;
                tblMainMenu.Code = model.Code;
                tblMainMenu.SortCode = model.SortCode;
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        model.Photo = new byte[file.ContentLength];
                        file.InputStream.Read(model.Photo, 0, file.ContentLength);
                        tblMainMenu.Photo = model.Photo;
                    }
                }
                else
                {
                    tblMainMenu.Photo = null;
                }
                Entities.S_MainMenu.Add(tblMainMenu);
                Entities.SaveChanges();
                ViewBag.Message = "New Main Menu Inserted Successful....";
                ViewBag.Type = 1;
                ModelState.Clear();
            }
            else
            {
                if(mSortCode==null&&menuCode!=null)
                {
                    ViewBag.Message = "Code Duplicated....";
                }
                else if(mSortCode!=null&&menuCode==null)
                {
                    ViewBag.Message = "Sort Code Duplicated....";
                }
                else
                {
                    ViewBag.Message = "Code and Sort Code Duplicated....";
                }                
                ViewBag.Type = 2;                                
            }                      
            return View("MainMenuEntry");
        }
        public JsonResult DeleteMainMenu(int id)
        {
            bool result = false;
            var mMenu = Entities.S_MainMenu.Find(id);
            if(mMenu!=null)
            {
                Entities.S_MainMenu.Remove(mMenu);
                Entities.SaveChanges();
                ViewBag.Message = "Main Menu Deleted successful";
                result = true;
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditMainMenu(int id)
        {
            GetCompanySetting();
            var menu = Entities.S_MainMenu.Find(id);
            if(menu!=null)
            {
                MainMenuModels.MainMenuModel model = new MainMenuModels.MainMenuModel();
                model.MainMenuID = menu.MainMenuID;
                model.MainMenuName = menu.MainMenuName;
                model.Code = menu.Code;
                model.SortCode =Convert.ToInt32(menu.SortCode);
                if (menu.Photo == null) ViewBag.Photo = false;
                else ViewBag.Photo = true;
                model.Photo = menu.Photo;
                ViewBag.formType = 2;
                return View("MainMenuEntry", model);
            }
            return View("MainMenuEntry");
        }
        public ActionResult UpdateMainMenu(MainMenuModels.MainMenuModel menu,HttpPostedFileBase file)
        {
            var mCode = Entities.S_MainMenu.Where(m=>m.MainMenuID!=menu.MainMenuID && m.Code==menu.Code).FirstOrDefault();
            var mSortCode = Entities.S_MainMenu.Where(m=>m.MainMenuID!=menu.MainMenuID && m.SortCode==menu.SortCode).FirstOrDefault();
            if(mCode==null && mSortCode==null)
            {
                if(file!=null)
                {
                    if (file.ContentLength > 0)
                    {
                        menu.Photo = new byte[file.ContentLength];
                        file.InputStream.Read(menu.Photo, 0, file.ContentLength);
                        menu.Photo = menu.Photo;
                        Entities.PrcUpdateMainMenu(menu.MainMenuID, menu.MainMenuName, menu.Code, menu.SortCode, menu.Photo, 2);
                    }
                }
                else
                {
                    Entities.PrcUpdateMainMenu(menu.MainMenuID, menu.MainMenuName, menu.Code, menu.SortCode, menu.Photo,1);
                }
                
                ViewBag.Message = "Main menu updated successful...";
                return View("MainMenuList", GetMainMenu());
            }
            else
            {
                if (mCode != null && mSortCode != null) ViewBag.Message = "Code and Sort Code Duplicated.";
                else if (mCode != null && mSortCode == null) ViewBag.Message = "Code duplicated.";
                else ViewBag.Message = "SortCode Duplicated...";
                GetCompanySetting();
                ViewBag.formType = 2;
                return View("MainMenuEntry", menu);
            }            
        }
        public JsonResult SearchMainMenu(string menu)
        {
            List<MainMenuModels.MainMenuModel> lstMainMenu = new List<MainMenuModels.MainMenuModel>();
            if(menu!="")
            {
                lstMainMenu = SearchingMainMenu(menu);
            }
            return Json(lstMainMenu,JsonRequestBehavior.AllowGet);
        }
        #region Methods
        public List<MainMenuModels.MainMenuModel>GetMainMenu()
        {
            List<MainMenuModels.MainMenuModel> lstMainMenu = new List<MainMenuModels.MainMenuModel>();
            var menu = Entities.S_MainMenu.ToList();
            foreach (var model in menu)
            {
                MainMenuModels.MainMenuModel mMenu = new MainMenuModels.MainMenuModel();
                mMenu.MainMenuID = model.MainMenuID;
                mMenu.MainMenuName = model.MainMenuName;
                mMenu.Code = model.Code;
                mMenu.SortCode = (Int32)model.SortCode;
                lstMainMenu.Add(mMenu);
            }
            return lstMainMenu;
        }
        public List<MainMenuModels.MainMenuModel>SearchingMainMenu(string menu)
        {
            List<MainMenuModels.MainMenuModel> lstMainMenu = new List<MainMenuModels.MainMenuModel>();
            var mMenu = Entities.S_MainMenu.Where(m => m.MainMenuName.Contains(menu) || m.Code == menu).ToList();
            foreach(var item in mMenu)
            {
                MainMenuModels.MainMenuModel model = new MainMenuModels.MainMenuModel();
                model.MainMenuID = item.MainMenuID;
                model.MainMenuName = item.MainMenuName;
                model.Code = item.Code;
                model.SortCode = Convert.ToInt32(item.SortCode);
                lstMainMenu.Add(model);
            }
            return lstMainMenu;
        }
        public void GetCompanySetting()
        {
            CompanySettingModels cModel = new CompanySettingModels();
            var isProductPhoto = Entities.S_CompanySetting.Select(company => company.IsProductPhoto);
            cModel.IsProductPhoto = isProductPhoto.FirstOrDefault();
            ViewBag.isProductPhoto = cModel.IsProductPhoto;
        }
       
        #endregion
    }
}