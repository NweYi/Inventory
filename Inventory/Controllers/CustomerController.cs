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
            ViewBag.BranchCount=Convert.ToInt32(Entities.S_Branch.Count());
            List<CustomerModels.CustomerModel> lstCustomer = new List<CustomerModels.CustomerModel>();
            var customer = Entities.PrcRetrieveCustomer();
            foreach(var model in customer)
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
            return View(lstCustomer);
        }
        public ActionResult CreateCustomer()
        {
            var township = Entities.S_Township.ToList();
            ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");
            if(Convert.ToInt32(Entities.S_Branch.Count())>0)
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
            if (Convert.ToInt32(Entities.S_Branch.Count()) > 0)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }
            try
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

                Entities.S_Customer.Add(tbl_customer);
                Entities.SaveChanges();

                ModelState.Clear();
                ViewBag.Message = "New Customer is inserted successful..";
                ViewBag.Type = 1;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please Enter Customer Detail...";
                ViewBag.Type = 2;
                return View("CreateCustomer");
            }
            return View("CreateCustomer");
        }
        [HttpPost]
        public ActionResult DeleteCustomer(int id)
        {
            var customer = Entities.S_Customer.Find(id);
            if (customer != null)
            {
                Entities.S_Customer.Remove(customer);
                Entities.SaveChanges();
            }
            return RedirectToAction("CustomerList","Customer");
        }
        [HttpPost]
        public ActionResult EditCustomer(int id)
        {
            var township = Entities.S_Township.ToList();
            ViewBag.Township = new SelectList(township, "TownshipID", "TownshipName");
            if (Convert.ToInt32(Entities.S_Branch.Count()) > 0)
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
            Entities.PrcUpdateCustomerData(customer.CustomerID, customer.Code, customer.CustomerName, customer.TownshipID, customer.Contact, customer.Address, customer.Phone, customer.Email, customer.IsCredit, customer.IsDefault, customer.BranchID);
            return RedirectToAction("CustomerList");
        }
    }
}