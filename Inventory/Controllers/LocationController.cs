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
            List<LocationModels.LocationModel> lstLocation = new List<LocationModels.LocationModel>();
            var branches = Entities.S_Branch.ToList();
            ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
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
            return View(lstLocation);
        }
        public ActionResult CreateLocation()
        {
            var branches = Entities.S_Branch.ToList();
            ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            return View();
        }
        public ActionResult InsertLocation(LocationModels.LocationModel location_model)
        {
            var branches = Entities.S_Branch.ToList();
            ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
            try
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
            }
            catch(Exception ex)
            {
                ViewBag.Message = "Please Enter Location Detail...";
                ViewBag.Type = 2;
                return View("CreateLocation");
            }
            return View("CreateLocation");
        }
        [HttpPost]
        public ActionResult DeleteLocation(int id)
        {
            var location_model = Entities.S_Location.Find(id);
            if(location_model!=null)
            {
                Entities.S_Location.Remove(location_model);
                Entities.SaveChanges();
            }

            List<LocationModels.LocationModel> lstLocation = new List<LocationModels.LocationModel>();
            var branches = Entities.S_Branch.ToList();
            ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
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
            return View("LocationList",lstLocation);
        }
        [HttpPost]
        public ActionResult EditLocation(int id)
        {
            var location = Entities.S_Location.Find(id);
            var branches = Entities.S_Branch.ToList();
            ViewBag.Branches = new SelectList(branches, "BranchID", "BranchName");
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
            Entities.PrcUpdateLocationData(location.LocationID,location.LocationName,location.ShortName,location.Description,location.Code,location.Phone,location.Address,location.Email,location.BranchID);
            return RedirectToAction("LocationList");
        }
        [HttpPost]
        public ActionResult Details(int id)
        {
            LocationModels.LocationModel model = new LocationModels.LocationModel();
            var location = Entities.S_Location.Find(id);
            model.LocationName = location.LocationName.ToString();
            model.ShortName = location.ShortName.ToString();
            model.Phone = location.Phone.ToString();
            return PartialView("ShowLocation",model);
        }
    }
}