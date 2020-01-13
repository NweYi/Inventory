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
            var branch = (from B in Entities.S_Branch
                          select new BranchModels.BranchModel
                          {
                              BranchID = B.BranchID,
                              BranchName = B.BranchName,
                              Code = B.Code
                          }).ToList();
            List<BranchModels.BranchModel> lstBranch = new List<BranchModels.BranchModel>();
            foreach(var item in branch)
            {
                BranchModels.BranchModel model = new BranchModels.BranchModel();
                model.BranchID = Convert.ToInt32(item.BranchID);
                model.BranchName = Convert.ToString(item.BranchName);
                model.Code = Convert.ToString(item.Code);
                lstBranch.Add(model);
            }
            return View(lstBranch);
        }
        public ActionResult CreateBranch()
        {
            return View();
        }
        public ActionResult InsertBranch(BranchModels.BranchModel branch)
        {
            try
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
            }
            catch(Exception ex)
            {
                ViewBag.Message = "Please enter complete branch data";
                ViewBag.Type = 2;
            }
            return View("CreateBranch");
        }
        [HttpPost]
        public ActionResult EditBranch(int id)
        {
            var branch = Entities.S_Branch.Find(id);
            if(branch!=null)
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
            Entities.PrcUpdateBranchData(branch.BranchID, branch.BranchName, branch.ShortName, branch.Description, branch.Code, branch.Phone, branch.Address, branch.Email, branch.Tax, branch.ServiceCharges);
            return RedirectToAction("BranchList");
        }
        [HttpPost]
        public ActionResult DeleteBranch(int id)
        {
            var branch_model = Entities.S_Branch.Find(id);
            if (branch_model != null)
            {
                Entities.S_Branch.Remove(branch_model);
                Entities.SaveChanges();
            }
            var branch = (from B in Entities.S_Branch
                          select new BranchModels.BranchModel
                          {
                              BranchID = B.BranchID,
                              BranchName = B.BranchName,
                              Code = B.Code
                          }).ToList();
            List<BranchModels.BranchModel> lstBranch = new List<BranchModels.BranchModel>();
            foreach (var item in branch)
            {
                BranchModels.BranchModel model = new BranchModels.BranchModel();
                model.BranchID = Convert.ToInt32(item.BranchID);
                model.BranchName = Convert.ToString(item.BranchName);
                model.Code = Convert.ToString(item.Code);
                lstBranch.Add(model);
            }
            return View("BranchList",lstBranch);
        }
        
        public ActionResult BranchDetail(int id)
        {
            BranchModels.BranchModel model = new BranchModels.BranchModel();
            var customer = Entities.S_Branch.Find(id);
            model.BranchName = customer.BranchName.ToString();
            if(customer.ShortName!=null)model.ShortName = customer.ShortName.ToString();
            if(customer.Phone!=null)model.Phone = customer.Phone.ToString();
            if(customer.Description!=null) model.Description = customer.Description.ToString();
            if(customer.Code!=null)model.Code = customer.Code.ToString();
            if(customer.Address!=null)model.Address = customer.Address.ToString();
            if(customer.Email!=null)model.Email = customer.Email.ToString();
            if(customer.Tax!=null)model.Tax = customer.Tax.ToString();
            if(customer.ServiceCharges!=null)model.ServiceCharges = customer.ServiceCharges.ToString();
            return PartialView("BranchDetail",model);
        }
    }

}