using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory.Models;
using System.Data.Entity.Core.Objects;

namespace Inventory.Controllers
{
    public class CustomerController : Controller
    {
        InventoryDBEntities Entities = new InventoryDBEntities();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CustomerList()
        {
            var township = Entities.S_Township.ToList();
            ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");
            GetisMultiBranch();
            if (ViewBag.isMultiBranch == true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }
            return View(GetCustomerList().ToList());
        }
        public ActionResult CreateCustomer()
        {
            var township = Entities.S_Township.ToList();
            ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");
            GetisMultiBranch();           
            if (ViewBag.isMultiBranch==true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }          
            return View();
        }
        public ActionResult InsertCustomer(CustomerModels.CustomerModel customer)
        {
            var township = Entities.S_Township.ToList();
            ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");
            GetisMultiBranch();
            if (ViewBag.isMultiBranch==true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }
            try
            {
                var cCode = Entities.S_Customer.Where(c => c.Code == customer.Code).FirstOrDefault();
                if(cCode!=null)
                {
                    ViewBag.Message = "Customer Code Duplicate...";
                    ViewBag.Type = 2;
                }
                else
                {
                    S_Customer tbl_customer = new S_Customer();
                    tbl_customer.CustomerName = customer.CustomerName;
                    tbl_customer.Code = customer.Code;
                    tbl_customer.Phone = customer.Phone;
                    tbl_customer.Email = customer.Email;
                    tbl_customer.Contact = customer.Contact;
                    tbl_customer.Address = customer.Address;
                    if (customer.BranchID > 0) tbl_customer.BranchID = customer.BranchID;
                    else tbl_customer.BranchID = null;
                    if (customer.TownshipID > 0) tbl_customer.TownshipID = customer.TownshipID;
                    else tbl_customer.TownshipID = null;
                    tbl_customer.IsCredit = customer.IsCredit;
                    tbl_customer.IsDefault = false;
                    Entities.S_Customer.Add(tbl_customer);
                    Entities.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = "New Customer is inserted successful..";
                    ViewBag.Type = 1;
                }                
                return View("CreateCustomer");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please Enter Customer Detail...";
                ViewBag.Type = 2;
                return View("CreateCustomer");
            }
        }
        [HttpPost]
        public JsonResult DeleteCustomer(int id)
        {
            var customer = Entities.S_Customer.Find(id);
            bool result = false;
            if (customer != null)
            {
                if(customer.IsDefault==false)
                {
                    Entities.S_Customer.Remove(customer);
                    Entities.SaveChanges();
                    result = true;
                    ViewBag.Message = "Deleted Successful...";
                }
                else
                {
                    ViewBag.Message = "System Detected for Default Customer";
                }                
            }
            //GetisMultiBranch();
            //return View("CustomerList",GetCustomerList().ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult EditCustomer(int id)
        {
            var township = Entities.S_Township.ToList();
            ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");
            GetisMultiBranch();
            if (ViewBag.isMultiBranch == true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }
            var customer = Entities.S_Customer.Find(id);
            if (customer != null)
            {
                CustomerModels.CustomerModel model = new CustomerModels.CustomerModel();
                model.CustomerID = Convert.ToInt32(customer.CustomerID);
                model.CustomerName = Convert.ToString(customer.CustomerName);
                model.Code = Convert.ToString(customer.Code);
                model.Phone = Convert.ToString(customer.Phone);
                model.Email = Convert.ToString(customer.Email);
                model.Contact = Convert.ToString(customer.Contact);
                model.Address = Convert.ToString(customer.Address);
                model.TownshipID = Convert.ToInt32(customer.TownshipID);
                model.BranchID = Convert.ToInt32(customer.BranchID);
                model.IsCredit = Convert.ToBoolean(customer.IsCredit);               
                ViewBag.formType = 2;
                return View("CreateCustomer", model);
            }
            else
            {
                return View("CreateCustomer");
            }
        }
        public ActionResult UpdateCustomer(CustomerModels.CustomerModel customer)
        {
            var cCode = Entities.S_Customer.Where(c => c.CustomerID != customer.CustomerID && c.Code == customer.Code).FirstOrDefault();
            if(cCode!=null)
            {
                var township = Entities.S_Township.ToList();
                ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");
                GetisMultiBranch();
                if (ViewBag.isMultiBranch == true)
                {
                    var branches = Entities.S_Branch.ToList();
                    ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
                }
                ViewBag.Message = "Customer Code Duplicated....";
                ViewBag.formType = 2;
                return View("CreateCustomer");
            }
            else
            {
                if(customer.BranchID>0)
                {
                    Entities.PrcUpdateCustomerData(customer.CustomerID, customer.Code, customer.CustomerName, customer.TownshipID, customer.Contact, customer.Address, customer.Phone, customer.Email, customer.IsCredit, customer.IsDefault, customer.BranchID);
                }
                else
                {
                    Entities.PrcUpdateCustomerData(customer.CustomerID, customer.Code, customer.CustomerName, customer.TownshipID, customer.Contact, customer.Address, customer.Phone, customer.Email, customer.IsCredit, customer.IsDefault, null);
                }
                ViewBag.Message = "Customer Updated Successful...";
                return View("CustomerList", GetCustomerList().ToList());
            }
        }
        public ActionResult CustomerDetail(int id)
        {
            var customer = (from C in Entities.S_Customer
                            join T in Entities.S_Township on C.TownshipID equals T.TownshipID into township
                            from LTownship in township.DefaultIfEmpty()
                            join B in Entities.S_Branch on C.BranchID equals B.BranchID into branch
                            from LBranch in branch.DefaultIfEmpty()
                            where C.CustomerID == id
                            select new CustomerModels.CustomerModel
                            {
                                CustomerID = C.CustomerID,
                                CustomerName = C.CustomerName,
                                Code = C.Code,
                                Phone = C.Phone,
                                Email = C.Email,
                                Contact = C.Contact,
                                TownshipName = LTownship.TownshipName,
                                BranchName = LBranch.BranchName,
                                Address = C.Address,
                                IsCredit = (Boolean)C.IsCredit                                                         
                         }).ToList().FirstOrDefault();
            GetisMultiBranch();
            return PartialView("CustomerDetail",customer);
        }
        public JsonResult SearchCustomer(string customer,string township,string branch)
        {
            List<CustomerModels.CustomerModel> lstCustomer = new List<CustomerModels.CustomerModel>();
            if (township == "") township = "0";
            if (branch == "") branch = "0";
            lstCustomer = SearchingCustomerData(customer, Convert.ToInt32(township), Convert.ToInt32(branch));
            var data = new { data1 = lstCustomer, data2 = isMultiBranch() };
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        #region Methods
        public void GetisMultiBranch()
        {
            CompanySettingModels cModel = new CompanySettingModels();
            var isMultiBranch = Entities.S_CompanySetting.Select(company => company.IsMultiBranch);
            cModel.IsMultiBranch = isMultiBranch.FirstOrDefault();
            ViewBag.isMultiBranch = cModel.IsMultiBranch;
        }
        public List<CustomerModels.CustomerModel>GetCustomerList()
        {
            List<CustomerModels.CustomerModel> lstCustomer = new List<CustomerModels.CustomerModel>();
            var customer = Entities.PrcRetrieveCustomer();
            foreach (var model in customer)
            {
                CustomerModels.CustomerModel data = new CustomerModels.CustomerModel();
                data.CustomerID = Convert.ToInt32(model.CustomerID);
                data.Code = Convert.ToString(model.Code);
                data.CustomerName = Convert.ToString(model.CustomerName);
                data.TownshipID = Convert.ToInt32(model.TownshipID);
                data.TownshipName = Convert.ToString(model.TownshipName);
                data.BranchID = Convert.ToInt32(model.BranchID);
                data.BranchName = Convert.ToString(model.BranchName);
                data.Contact = Convert.ToString(model.Contact);
                data.Address = Convert.ToString(model.Address);
                data.Phone = Convert.ToString(model.Phone);
                data.Email = Convert.ToString(model.Email);
                data.IsCredit = Convert.ToBoolean(model.IsCredit);
                data.IsDefault = Convert.ToBoolean(model.IsDefault);
                lstCustomer.Add(data);
            }
            return lstCustomer;
        }
        public bool isMultiBranch()
        {
            CompanySettingModels cModel = new CompanySettingModels();
            var isMultiBranch = Entities.S_CompanySetting.Select(company => company.IsMultiBranch);
            cModel.IsMultiBranch = isMultiBranch.FirstOrDefault();
            return Convert.ToBoolean(cModel.IsMultiBranch);
        }
        public List<CustomerModels.CustomerModel> SearchingCustomerData(string customer,int township,int branch)
        {
            List<CustomerModels.CustomerModel> lstCustomer = new List<CustomerModels.CustomerModel>();
            var cData = Entities.PrcSearchCustomerData(customer, township, branch);
            foreach(var item in cData)
            {
                CustomerModels.CustomerModel model = new CustomerModels.CustomerModel();
                model.CustomerID = Convert.ToInt32(item.CustomerID);
                model.Code = Convert.ToString(item.Code);
                model.CustomerName = Convert.ToString(item.CustomerName);
                model.TownshipName = Convert.ToString(item.TownshipName);
                model.BranchName = Convert.ToString(item.BranchName);
                lstCustomer.Add(model);
            }
            return lstCustomer;
        }
        #endregion
    }
}