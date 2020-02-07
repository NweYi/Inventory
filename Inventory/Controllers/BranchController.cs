
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory.Models;
using System.Data.Entity.Core.Objects;

namespace Inventory.Controllers
{
    public class BranchController : MyController
    {
        InventoryDBEntities Entities = new InventoryDBEntities();
        // GET: Branch
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BranchList()
        {
            return View(GetBranchList().ToList());
        }
        public ActionResult CreateBranch()
        {
            return View();
        }
        public ActionResult InsertBranch(BranchModels.BranchModel branch)
        {
            try
            {
                var branch_code = Entities.S_Branch.Where(b => b.Code == branch.Code).FirstOrDefault();
                if (branch_code != null)
                {
                    ViewBag.Message = "Code Duplicated....";
                    ViewBag.Type = 2;
                    return View("CreateBranch");
                }
                else
                {
                    S_Branch tbl_branch = new S_Branch();
                    tbl_branch.BranchName = branch.BranchName;
                    tbl_branch.ShortName = branch.ShortName;
                    tbl_branch.Description = branch.Description;
                    tbl_branch.Code = branch.Code;
                    tbl_branch.Phone = branch.Phone;
                    tbl_branch.Email = branch.Email;
                    tbl_branch.Address = branch.Address;
                    tbl_branch.Tax = branch.Tax.ToString();
                    tbl_branch.ServiceCharges = branch.ServiceCharges.ToString();
                    Entities.S_Branch.Add(tbl_branch);
                    Entities.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = "New Branch is inserted successful..";
                    ViewBag.Type = 1;
                    return View("CreateBranch");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please enter complete branch data";
                ViewBag.Type = 2;
            }
            return View("CreateBranch");
        }
        
        public ActionResult EditBranch(int id)
        {
            var branch = Entities.S_Branch.Find(id);
            if (branch != null)
            {
                BranchModels.BranchModel branch_model = new BranchModels.BranchModel();
                branch_model.BranchID = Convert.ToInt32(branch.BranchID);
                branch_model.BranchName = Convert.ToString(branch.BranchName);
                branch_model.ShortName = Convert.ToString(branch.ShortName);
                branch_model.Description = Convert.ToString(branch.Description);
                branch_model.Code = Convert.ToString(branch.Code);
                branch_model.Phone = Convert.ToString(branch.Phone);
                branch_model.Email = Convert.ToString(branch.Email);
                branch_model.Address = Convert.ToString(branch.Address);
                branch_model.Tax = Convert.ToString(branch.Tax);
                branch_model.ServiceCharges = Convert.ToString(branch.ServiceCharges);
                ViewBag.formType = 2;
                return View("CreateBranch", branch_model);
            }
            else
            {
                return View("CreateBranch");
            }
        }
        public ActionResult UpdateBranch(BranchModels.BranchModel branch)
        {
            var branch_code = Entities.S_Branch.Where(b => b.BranchID != branch.BranchID && b.Code == branch.Code).FirstOrDefault();
            if (branch_code != null)
            {
                ViewBag.Message = "Code Duplicated....";
                ViewBag.formType = 2;
                return View("CreateBranch");
            }
            else
            {
                Entities.PrcUpdateBranchData(branch.BranchID, branch.BranchName, branch.ShortName, branch.Description, branch.Code, branch.Phone, branch.Address, branch.Email, branch.Tax, branch.ServiceCharges);
                ViewBag.Message = "Branch Updated Successful...";
                return View("BranchList", GetBranchList().ToList());
            }
        }
        public JsonResult SearchingBranch(string search)
        {
            List<BranchModels.BranchModel> lstBranch = new List<BranchModels.BranchModel>();
            if(search!=null)
            {
                lstBranch = SearchBranch(search);
            }
            return Json(lstBranch, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteBranch(int id)
        {
            var branch_model = Entities.S_Branch.Find(id);
            bool result = false;
            if (branch_model != null)
            {
                Entities.S_Branch.Remove(branch_model);
                Entities.SaveChanges();
                ViewBag.Message = "Branch Deleted successful...";
                result = true;
            }
            //return View("BranchList", GetBranchList().ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BranchDetail(int id)
        {
            BranchModels.BranchModel model = new BranchModels.BranchModel();
            var branch = Entities.S_Branch.Find(id);
            if (branch != null)
            {
                model.BranchName = branch.BranchName.ToString();
                if (branch.ShortName != null) model.ShortName = branch.ShortName.ToString();
                if (branch.Description != null) model.Description = branch.Description.ToString();
                if (branch.Code != null) model.Code = branch.Code.ToString();
                if (branch.Phone != null) model.Phone = branch.Phone.ToString();
                if (branch.Email != null) model.Email = branch.Email.ToString();
                if (branch.Address != null) model.Address = branch.Address.ToString();
                if (branch.Tax != null) model.Tax = branch.Tax.ToString();
                if (branch.ServiceCharges != null) model.ServiceCharges = branch.ServiceCharges.ToString();
            }
            return PartialView("BranchDetail", model);
        }
        #region Methods
        public List<BranchModels.BranchModel> GetBranchList()
        {
            List<BranchModels.BranchModel> lstBranch = new List<BranchModels.BranchModel>();
            var branch = (from B in Entities.S_Branch
                          select new BranchModels.BranchModel
                          {
                              BranchID = B.BranchID,
                              BranchName = B.BranchName,
                              Code = B.Code
                          }).ToList();
            foreach (var item in branch)
            {
                BranchModels.BranchModel model = new BranchModels.BranchModel();
                model.BranchID = Convert.ToInt32(item.BranchID);
                model.BranchName = Convert.ToString(item.BranchName);
                model.Code = Convert.ToString(item.Code);
                lstBranch.Add(model);
            }
            return lstBranch;
        }
        public List<BranchModels.BranchModel> SearchBranch(string search)
        {           
            var branch = (from B in Entities.S_Branch
                          where B.BranchName.Contains(search)||B.Code.Contains(search)
                          select new BranchModels.BranchModel
                          {
                              BranchID = B.BranchID,
                              BranchName = B.BranchName,
                              Code = B.Code
                          }).ToList();
            return branch;
        }
        #endregion
    }
}