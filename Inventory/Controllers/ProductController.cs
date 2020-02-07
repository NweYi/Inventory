using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inventory.Models;
using System.Data.Entity.Core.Objects;
using System.IO;

namespace Inventory.Controllers
{
    public class ProductController : MyController
    {
        InventoryDBEntities Entities = new InventoryDBEntities();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductList()
        {
            GetisMultiUnit();
            var mainMenu = Entities.S_MainMenu.ToList();
            ViewBag.MainMenu = new SelectList(mainMenu, "MainMenuID", "MainMenuName");
            var subMenu = Entities.S_SubMenu.ToList();
            ViewBag.SubMenu = new SelectList(subMenu, "SubMenuID", "SubMenuName");
            return View(GetProductList().ToList());
        }
        public ActionResult ProductEntry()
        {
            GetSetting();
            ProductModels.ProductDetail detail = new ProductModels.ProductDetail();
            if (ViewBag.isMultiBranchDifProductByBranch == true)
            {
                detail.lstBranch = GetProductAvailableBranch();                
            }
            return View(detail);
        }
        [HttpPost]
        public ActionResult InsertProduct(ProductModels.ProductDetail product,HttpPostedFileBase file,int[]availableBranch)
        {
            //Entities.PrcInsertProduct(product.ProductModel.Code, product.ProductModel.SortCode, product.ProductModel.ProductName, product.ProductModel.Description, product.ProductModel.SubMenuID,1200,1500,1300, product.ProductModel.isUnit, product.ProductModel.isStock, product.ProductModel.Photo, product.ProductModel.AlertQuantity, Convert.ToInt16(product.ProductModel.DiscountPercent),null, product.ProductModel.QRCode, product.ProductModel.isVariant);
            GetSetting();
            ProductModels.ProductDetail detail = new ProductModels.ProductDetail();
            var pCode = Entities.S_Product.Where(p => p.Code == product.ProductModel.Code).FirstOrDefault();
            var pSortCode = Entities.S_Product.Where(p => p.SortCode == product.ProductModel.SortCode).FirstOrDefault();
            if(pCode==null && pSortCode==null)
            {
                S_Product tbl_Product = new S_Product();
                tbl_Product.ProductName = product.ProductModel.ProductName;
                tbl_Product.Code = product.ProductModel.Code;
                tbl_Product.SortCode = product.ProductModel.SortCode;
                tbl_Product.Description = product.ProductModel.Description;
                if (product.ProductModel.SubMenuID == 0) tbl_Product.SubMenuID = null;
                else tbl_Product.SubMenuID = product.ProductModel.SubMenuID;
                if (product.ProductModel.PurchasePrice == 0) tbl_Product.PurPrice = null;
                else tbl_Product.PurPrice = (decimal)product.ProductModel.PurchasePrice;
                if (product.ProductModel.SalePrice == 0) tbl_Product.SalePrice = null;
                else tbl_Product.SalePrice = (decimal)product.ProductModel.SalePrice;
                if (product.ProductModel.WholeSalePrice == 0) tbl_Product.WholeSalePrice = null;
                else tbl_Product.WholeSalePrice = (decimal)product.ProductModel.WholeSalePrice;
                tbl_Product.IsUnit = product.ProductModel.isUnit;
                tbl_Product.IsStock = product.ProductModel.isStock;
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        product.ProductModel.Photo = new byte[file.ContentLength];
                        file.InputStream.Read(product.ProductModel.Photo, 0, file.ContentLength);
                        tbl_Product.Photo = product.ProductModel.Photo;
                    }
                }
                else
                {
                    tbl_Product.Photo = null;
                }
                if (product.ProductModel.AlertQuantity == 0) tbl_Product.AlertQty = null;
                else tbl_Product.AlertQty = product.ProductModel.AlertQuantity;
                if (product.ProductModel.DiscountPercent == 0) tbl_Product.DisPercent = null;
                else tbl_Product.DisPercent = (Int16)product.ProductModel.DiscountPercent;
                tbl_Product.Barcode = product.ProductModel.BarCode;
                tbl_Product.QRcode = product.ProductModel.QRCode;
                tbl_Product.IsVariant = product.ProductModel.isVariant;
                Entities.S_Product.Add(tbl_Product);
                Entities.SaveChanges();

                //Get Recently ProductID
                int product_id = Entities.S_Product.Max(p => p.ProductID);
                //if MultiBranch and difference product by branch
                if (availableBranch != null)
                {
                    for (int i = 0; i < availableBranch.Count(); i++)
                    {
                        S_ProductBranch tbl_ProductBranch = new S_ProductBranch();
                        tbl_ProductBranch.ProductID = product_id;
                        tbl_ProductBranch.BranchID = availableBranch[i];
                        Entities.S_ProductBranch.Add(tbl_ProductBranch);
                        Entities.SaveChanges();
                    }
                }
                //if Product Variant...
                if (lstProductVariant.Count > 0)
                {
                    for (int i = 0; i < lstProductVariant.Count; i++)
                    {
                        S_ProductVariant tbl_ProductVariant = new S_ProductVariant();
                        tbl_ProductVariant.ProductID = product_id;
                        tbl_ProductVariant.Variant = lstProductVariant[i].Variant.ToString();
                        Entities.S_ProductVariant.Add(tbl_ProductVariant);
                        Entities.SaveChanges();
                    }
                    lstProductVariant.Clear();
                }
                ModelState.Clear();
                ViewBag.Message = "Product is Inserted Successful...";
            }
            else
            {
                if (pCode != null && pSortCode == null) ViewBag.Message = "Product Code is Duplicate";
                else if (pCode == null && pSortCode != null) ViewBag.Message = "Product SortCode is Duplicated..";
                else ViewBag.Message = "Product Code and SortCode are duplicated";
                ViewBag.Variant = lstProductVariant;
                if(availableBranch!=null)
                {
                    ViewBag.isCheckedBranch = availableBranch;
                }
            }
            if (ViewBag.isMultiBranchDifProductByBranch == true)
            {
                detail.lstBranch = GetProductAvailableBranch();
            }
            return View("ProductEntry", detail);
        }
        public static List<ProductModels.ProductVariant> lstProductVariant = new List<ProductModels.ProductVariant>();
        public ActionResult InsertProductVariant(ProductModels.ProductDetail product)
        {
            GetSetting();
            ProductModels.ProductVariant variant = new ProductModels.ProductVariant();
            variant.Variant = product.ProductVariant.Variant.ToString();
            lstProductVariant.Add(variant);
            ViewBag.Variant = lstProductVariant;

            ProductModels.ProductDetail detail = new ProductModels.ProductDetail();
            if (ViewBag.isMultiBranchDifProductByBranch == true)
            {
                detail.lstBranch = GetProductAvailableBranch();
            }
            return View("ProductEntry",detail);
        }
        public JsonResult DeleteProduct(int id)
        {
            var product = Entities.S_Product.Find(id);
            bool result = false;
            if(product!=null)
            {
                //Delete Product Data from ProductBranch
                var productBranch = Entities.S_ProductBranch.Where(b=>b.ProductID==id).ToList();
                if(productBranch!=null)
                {
                    Entities.S_ProductBranch.RemoveRange(Entities.S_ProductBranch.Where(pBranch =>pBranch.ProductID == id));
                    Entities.SaveChanges();
                }
                //Delete Product Variant from Product Variant
                var pVariant = Entities.S_ProductVariant.Where(v => v.ProductID == id).ToList();
                if(pVariant!=null)
                {
                    Entities.S_ProductVariant.RemoveRange(Entities.S_ProductVariant.Where(v => v.ProductID == id));
                    Entities.SaveChanges();
                }
                Entities.S_Product.Remove(product);
                Entities.SaveChanges();
                result = true;            
                ViewBag.Message = "Product Deleted Successful...";
            }
            //GetisMultiUnit();
            //return View("ProductList", GetProductList().ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchProduct(string product,string mMenu,string sMenu)
        {
            List<ProductModels.ProductModel> lstProduct = new List<ProductModels.ProductModel>();
            if (mMenu == "") mMenu = "0";
            if (sMenu == "") sMenu = "0";
            lstProduct = SearchingProduct(product, Convert.ToInt32(mMenu), Convert.ToInt32(sMenu));
            var data = new { data1 = lstProduct, data2 = isMultiUnit() };
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        #region Methods
        public List<ProductModels.ProductModel>GetProductList()
        {
            List<ProductModels.ProductModel> lstProduct = new List<ProductModels.ProductModel>();
            var product = Entities.PrcRetrieveProduct();
            foreach(var m in product)
            {
                ProductModels.ProductModel model = new ProductModels.ProductModel();
                model.ProductID = Convert.ToInt32(m.ProductID);
                model.ProductName = m.ProductName.ToString();
                model.Code = m.Code.ToString();
                if (m.SubMenuName == null) model.SubMenuName = "-";
                else model.SubMenuName = m.SubMenuName;
                if (m.MainMenuName == null) model.MainMenuName = "-";
                else model.MainMenuName = m.MainMenuName;
                if (m.SalePrice == null) model.SalePrice =0;
                else model.SalePrice = Convert.ToDouble(m.SalePrice);
                lstProduct.Add(model);
            }
            return lstProduct;
        }
        public void GetisMultiUnit()
        {
            CompanySettingModels cModel = new CompanySettingModels();
            var isMultiUnit = Entities.S_CompanySetting.Select(company => company.IsMultiUnit);
            cModel.IsMultiUnit = isMultiUnit.FirstOrDefault();
            ViewBag.isMultiUnit = cModel.IsMultiUnit;
        }
        public List<BranchModels.BranchModel> GetProductAvailableBranch()
        {
            List<BranchModels.BranchModel> lstBranch = new List<BranchModels.BranchModel>();
            var branch = Entities.S_Branch.ToList();
            foreach (var model in branch)
            {
                BranchModels.BranchModel b = new BranchModels.BranchModel();
                b.BranchID = Convert.ToInt32(model.BranchID);
                b.BranchName = model.BranchName.ToString();
                lstBranch.Add(b);
            }
            return lstBranch;
        }
        public List<ProductModels.ProductModel> SearchingProduct(string product,int mMenu,int sMenu)
        {
            List<ProductModels.ProductModel> lstProduct = new List<ProductModels.ProductModel>();
            var data = Entities.PrcSearchProductData(product, mMenu, sMenu);
            foreach(var item in data)
            {
                ProductModels.ProductModel model = new ProductModels.ProductModel();
                model.ProductID = Convert.ToInt32(item.ProductID);
                model.ProductName = item.ProductName.ToString();
                model.Code = item.Code.ToString();
                if (item.SubMenuName == null) model.SubMenuName = "-";
                else model.SubMenuName = item.SubMenuName;
                if (item.MainMenuName == null) model.MainMenuName = "-";
                else model.MainMenuName = item.MainMenuName;
                if (item.SalePrice == null) model.SalePrice = 0;
                else model.SalePrice = Convert.ToDouble(item.SalePrice);
                lstProduct.Add(model);
            }
            return lstProduct;
        }     
        public void GetSetting()
        {
            CompanySettingModels cModel = new CompanySettingModels();
            var company = Entities.S_CompanySetting.ToList();
            foreach(var item in company)
            {
                cModel.CompanyID = item.CompanyID;
                cModel.CompanyName = item.CompanyName;
                cModel.IsBarcode = item.IsBarcode;
                cModel.IsMultiBranch = item.IsMultiBranch;
                cModel.IsMultiUnit = item.IsMultiUnit;
                cModel.IsProductByBranch = item.IsDifProductByBranch;
                cModel.IsProductPhoto = item.IsProductPhoto;
                cModel.IsProductVariant = item.IsProductVariant;
            }
            ViewBag.isProductPhoto = cModel.IsProductPhoto;
            if (cModel.IsMultiBranch == true && cModel.IsProductByBranch == true) ViewBag.isMultiBranchDifProductByBranch = true;
            else ViewBag.isMultiBranchDifProductByBranch = false;
            ViewBag.isMultiUnit = cModel.IsMultiUnit;
            ViewBag.isProductVariant = cModel.IsProductVariant;
           
        }
        public bool isMultiUnit()
        {
            CompanySettingModels cModel = new CompanySettingModels();
            var company = Entities.S_CompanySetting.Select(c=>c.IsMultiUnit);
            cModel.IsMultiUnit = company.FirstOrDefault();
            return Convert.ToBoolean(cModel.IsMultiUnit);
        }
        #endregion
    }
}