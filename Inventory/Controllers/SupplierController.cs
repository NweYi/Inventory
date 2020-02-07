using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory.Models;

namespace Inventory.Controllers
{
    public class SupplierController : Controller
    {
        InventoryDBEntities Entities = new InventoryDBEntities();
        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SupplierList()
        {
            var township = Entities.S_Township.ToList();
            ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");
            GetCompanySetting();
            if (ViewBag.isMultiBranch == true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }
            return View(GetSupplier());
        }
        public ActionResult SupplierEntry()
        {
            var township = Entities.S_Township.ToList();
            ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");
            GetCompanySetting();
            if (ViewBag.isMultiBranch == true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }
            return View();
        }
        public ActionResult InsertSupplier(SupplierModels.Supplier supplier)
        {
            var township = Entities.S_Township.ToList();
            ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");
            GetCompanySetting();
            if (ViewBag.isMultiBranch == true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }
            var sCode = Entities.S_Supplier.Where(s => s.Code == supplier.Code).FirstOrDefault();
            if (sCode != null)
            {
                ViewBag.Message = "Supplier Code Duplicate...";
                ViewBag.Type = 2;
            }
            else
            {
                S_Supplier tbl_supplier = new S_Supplier();
                tbl_supplier.SupplierName = supplier.SupplierName;
                tbl_supplier.Code = supplier.Code;
                tbl_supplier.Phone = supplier.Phone;
                tbl_supplier.Email = supplier.Email;
                tbl_supplier.Contact = supplier.Contact;
                tbl_supplier.Address = supplier.Address;
                if (supplier.BranchID != 0) tbl_supplier.BranchID = supplier.BranchID;
                else tbl_supplier.BranchID = null;               
                tbl_supplier.TownshipID = supplier.TownshipID;
                tbl_supplier.IsCredit = supplier.isCredit;
                tbl_supplier.IsDefault = false;
                Entities.S_Supplier.Add(tbl_supplier);
                Entities.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = "New Supplier is inserted successful..";
                ViewBag.Type = 1;
            }
            return View("SupplierEntry");
        }
        public JsonResult DeleteSupplier(int id)
        {
            bool result = false;
            var supplier = Entities.S_Supplier.Find(id);
            if(supplier!=null)
            {
                if(supplier.IsDefault==false)
                {
                    Entities.S_Supplier.Remove(supplier);
                    Entities.SaveChanges();
                    ViewBag.Message = "Deleted Successful...";
                    result = true;
                }
                else
                {
                    ViewBag.Message = "System Detected for Default Customer";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditSupplier(int id)
        {
            GetCompanySetting();
            var supplier = Entities.S_Supplier.Find(id);
            if(supplier!=null)
            {
                var township = Entities.S_Township.ToList();
                ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");               
                if (ViewBag.isMultiBranch == true)
                {
                    var branches = Entities.S_Branch.ToList();
                    ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
                }
                SupplierModels.Supplier model = new SupplierModels.Supplier();
                model.SupplierID = supplier.SupplierID;
                model.SupplierName = supplier.SupplierName;
                model.Code = supplier.Code;
                model.Phone = supplier.Phone;
                model.Email = supplier.Email;
                model.Contact = supplier.Contact;
                model.TownshipID = Convert.ToInt32(supplier.TownshipID);
                if (supplier.BranchID != null)model.BranchID = Convert.ToInt32(supplier.BranchID);
                model.Address = supplier.Address;
                model.isCredit = Convert.ToBoolean(supplier.IsCredit);
                ViewBag.formType = 2;
                return View("SupplierEntry", model);
            }
            return View("SupplierList",GetSupplier());
        }
        public ActionResult UpdateSupplier(SupplierModels.Supplier supplier)
        {
            GetCompanySetting();
            var sCode = Entities.S_Supplier.Where(s => s.SupplierID != supplier.SupplierID && s.Code == supplier.Code).FirstOrDefault();
            if (sCode != null)
            {
                var township = Entities.S_Township.ToList();
                ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");
                if (ViewBag.isMultiBranch == true)
                {
                    var branches = Entities.S_Branch.ToList();
                    ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
                }
                ViewBag.Message = "Supplier Code Duplicated....";
                ViewBag.formType = 2;
                return View("SupplierEntry");
            }
            else
            {
                if(supplier.BranchID>0)
                {
                    Entities.PrcUpdateSupplier(supplier.SupplierID, supplier.Code, supplier.SupplierName, supplier.TownshipID, supplier.Contact, supplier.Address, supplier.Phone, supplier.Email, supplier.isCredit, supplier.BranchID);
                }
                else
                {
                    Entities.PrcUpdateSupplier(supplier.SupplierID, supplier.Code, supplier.SupplierName, supplier.TownshipID, supplier.Contact, supplier.Address, supplier.Phone, supplier.Email, supplier.isCredit, null);
                }
                ViewBag.Message = "Supplier Updated Successful...";
                return View("SupplierList", GetSupplier());
            }            
        }
        public ActionResult SupplierDetail(int id)
        {
            var supplier = (from S in Entities.S_Supplier
                            join T in Entities.S_Township on S.TownshipID equals T.TownshipID into township
                            from LTownship in township.DefaultIfEmpty()
                            join B in Entities.S_Branch on S.BranchID equals B.BranchID into branch
                            from LBranch in branch.DefaultIfEmpty()
                            where S.SupplierID == id
                            select new SupplierModels.Supplier
                            {
                                SupplierID = S.SupplierID,
                                SupplierName = S.SupplierName,
                                Code = S.Code,
                                Phone = S.Phone,
                                Email = S.Email,
                                Contact = S.Contact,
                                TownshipName = LTownship.TownshipName,
                                BranchName = LBranch.BranchName,
                                Address = S.Address,
                                isCredit = (Boolean)S.IsCredit
                            }).ToList().FirstOrDefault() ;
            GetCompanySetting();
            return PartialView(supplier);
        } 
        public JsonResult SearchSupplier(string supplier,string township,string branch)
        {
            List<SupplierModels.Supplier> lstSupplier = new List<SupplierModels.Supplier>();
            if (township == "") township = "0";
            if (branch == "") branch = "0";
            lstSupplier = SearchingSupplierData(supplier, Convert.ToInt32(township), Convert.ToInt32(branch));
            var data = new { data1 = lstSupplier, data2 = isMultiBranch() };
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        #region Methods
        public List<SupplierModels.Supplier>GetSupplier()
        {
            List<SupplierModels.Supplier> lstSupplier = new List<SupplierModels.Supplier>();
            var supplier = Entities.PrcRetrieveSupplier();
            foreach(var item in supplier)
            {
                SupplierModels.Supplier model = new SupplierModels.Supplier();
                model.SupplierID = item.SupplierID;
                model.SupplierName = item.SupplierName;
                model.Code = item.Code;
                if (item.TownshipName == null) model.TownshipName = "-";
                else model.TownshipName = item.TownshipName;
                if (item.BranchName == null) model.BranchName = "-";
                else model.BranchName = item.BranchName;
                lstSupplier.Add(model);               
            }
            return lstSupplier;
        }
        public List<SupplierModels.Supplier>SearchingSupplierData(string supplier,int township,int branch)
        {
            List<SupplierModels.Supplier> lstSupplier = new List<SupplierModels.Supplier>();
            var data = Entities.PrcSearchSupplierData(supplier, township, branch);
            foreach(var item in data)
            {
                SupplierModels.Supplier model = new SupplierModels.Supplier();
                model.SupplierID = item.SupplierID;
                model.SupplierName = item.SupplierName;
                model.Code = item.Code;
                model.TownshipName = item.TownshipName;
                model.BranchName = item.BranchName;
                lstSupplier.Add(model);
            }
            return lstSupplier;
        }
        public void GetCompanySetting()
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
        #endregion
    }
}