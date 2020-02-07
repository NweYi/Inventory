using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory.Models;
using System.Data.Entity.Core.Objects;

namespace Inventory.Controllers
{
    public class LocationController : Controller
    {
        InventoryDBEntities Entities = new InventoryDBEntities();
        // GET: Location
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LocationList()
        {
            GetisMultiBranch();
            if (ViewBag.isMultiBranch == true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }
            return View(GetLocationList().ToList());
        }
        public ActionResult CreateLocation()
        {
            GetisMultiBranch();
            if(ViewBag.isMultiBranch==true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }           
            return View();
        }
        public ActionResult InsertLocation(LocationModels.LocationModel location_model)
        {
            GetisMultiBranch();
            if (ViewBag.isMultiBranch == true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }
            try
            {
                var location_code = Entities.S_Location.Where(location => location.Code == location_model.Code).FirstOrDefault();
                if(location_code!=null)
                {
                    ViewBag.Message = "Location Code Duplicate....";
                    ViewBag.Type = 2;
                    return View("CreateLocation");
                }
                else
                {
                    S_Location tbl_location = new S_Location();
                    tbl_location.LocationName = location_model.LocationName;
                    tbl_location.ShortName = location_model.ShortName;
                    tbl_location.Code = location_model.Code;
                    tbl_location.Description = location_model.Description;
                    tbl_location.Phone = location_model.Phone;
                    tbl_location.Address = location_model.Address;
                    tbl_location.Email = location_model.Email;
                    if (location_model.BranchID > 0) tbl_location.BranchID = location_model.BranchID;
                    else tbl_location.BranchID = null;
                    Entities.S_Location.Add(tbl_location);
                    Entities.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = "New Location is inserted successful..";
                    ViewBag.Type = 1;
                    return View("CreateLocation");
                }                
            }
            catch(Exception ex)
            {
                ViewBag.Message = "Please Enter Location Detail...";
                ViewBag.Type = 2;
                return View("CreateLocation");
            }
        }
        [HttpPost]
        public JsonResult DeleteLocation(int id)
        {
            GetisMultiBranch();
            bool result = false;
            var location_model = Entities.S_Location.Find(id);
            if(location_model!=null)
            {
                Entities.S_Location.Remove(location_model);
                Entities.SaveChanges();
                result = true;
                ViewBag.Message = "Location Deleted Successful...";
            }
            //return View("LocationList",GetLocationList().ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchLocation(string search,string branch)
        {
            List<LocationModels.LocationModel> lstLocation = new List<LocationModels.LocationModel>();
            if (branch == "") branch ="0";
            lstLocation = SearchingLocation(search,Convert.ToInt32(branch));
            var data = new { data1 = lstLocation, data2 = isMultiBranch() };
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditLocation(int id)
        {
            GetisMultiBranch();
            if (ViewBag.isMultiBranch == true)
            {
                var branches = Entities.S_Branch.ToList();
                ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            }
            var location = Entities.S_Location.Find(id);
            if (location!=null)
            {                
                LocationModels.LocationModel location_model = new LocationModels.LocationModel();
                location_model.LocationID = Convert.ToInt32(location.LocationID);
                location_model.LocationName = Convert.ToString(location.LocationName);
                location_model.ShortName = Convert.ToString(location.ShortName);
                location_model.Code = Convert.ToString(location.Code);
                location_model.Description = Convert.ToString(location.Description);
                location_model.Phone = Convert.ToString(location.Phone);
                location_model.Email = Convert.ToString(location.Email);
                location_model.Address = Convert.ToString(location.Address);
                location_model.BranchID = Convert.ToInt32(location.BranchID);
                ViewBag.Formtype = 2;
                return View("CreateLocation", location_model);
            }
            else
            {
                return View("CreateLocation");
            }
        }
        [HttpPost]
        public ActionResult UpdateLocation(LocationModels.LocationModel location)
        {
            GetisMultiBranch();
            var locCode = Entities.S_Location.Where(loc => loc.LocationID != location.LocationID && loc.Code == location.Code).FirstOrDefault();
            if(locCode!=null)
            {
                ViewBag.Message = "Location Code Duplicated....";
                ViewBag.formType = 2;
                if (ViewBag.isMultiBranch == true)
                {
                    var branches = Entities.S_Branch.ToList();
                    ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
                }
                return View("CreateLocation");
            }
            else
            {
                if (location.BranchID > 0)
                {
                    Entities.PrcUpdateLocationData(location.LocationID, location.LocationName, location.ShortName, location.Description, location.Code, location.Phone, location.Address, location.Email, location.BranchID);
                }
                else
                {
                    Entities.PrcUpdateLocationData(location.LocationID, location.LocationName, location.ShortName, location.Description, location.Code, location.Phone, location.Address, location.Email,null);
                }
                ViewBag.Message = "Location Updated Successful...";
                return View("LocationList", GetLocationList().ToList());
            }
        }
        
        public ActionResult LocationDetails(int id)
        {
            GetisMultiBranch();
            LocationModels.LocationModel model = new LocationModels.LocationModel();
            var location = Entities.S_Location.Find(id);
            if(location!=null)
            {
                model.LocationName = location.LocationName.ToString();
                model.ShortName = location.ShortName.ToString();
                if(location.Description!=null)model.Description = location.Description.ToString();
                if(location.Code!=null)model.Code = location.Code.ToString();
                if (location.Phone != null) model.Phone = location.Phone.ToString();
                if (location.Email != null) model.Email = location.Email.ToString();
                if (location.Address != null) model.Address = location.Address.ToString();
            }            
            return PartialView("ShowLocation",model);
        }
        #region Methods
        public List<LocationModels.LocationModel>GetLocationList()
        {
            List<LocationModels.LocationModel> lstLocation = new List<LocationModels.LocationModel>();
            var location = Entities.PrcRetrieveLocationData();
            foreach (var item in location)
            {
                LocationModels.LocationModel model = new LocationModels.LocationModel();
                model.LocationID = Convert.ToInt32(item.LocationID);
                model.LocationName = Convert.ToString(item.LocationName);
                model.ShortName = Convert.ToString(item.ShortName);
                model.Description = Convert.ToString(item.Description);
                model.Code = Convert.ToString(item.Code);
                model.Phone = Convert.ToString(item.Phone);
                model.Address = Convert.ToString(item.Address);
                model.Email = Convert.ToString(item.Email);
                model.BranchID = Convert.ToInt32(item.BranchID);
                model.BranchName = Convert.ToString(item.BranchName);
                lstLocation.Add(model);
            }
            return lstLocation;
        }
        public List<LocationModels.LocationModel> SearchingLocation(string search,int branchID)
        {
            List<LocationModels.LocationModel> lstLocation = new List<LocationModels.LocationModel>();
            var location =Entities.PrcSearchLocationData(search,branchID);
            foreach(var item in location)
            {
                LocationModels.LocationModel model = new LocationModels.LocationModel();
                model.LocationID = Convert.ToInt32(item.LocationID);
                model.LocationName = Convert.ToString(item.LocationName);
                model.Code = item.Code.ToString();             
                model.BranchName = Convert.ToString(item.BranchName);
                lstLocation.Add(model);
            }
            return lstLocation;
        }
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
        #endregion
    }
}