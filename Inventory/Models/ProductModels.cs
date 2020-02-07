using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class ProductModels
    {
        public class ProductModel
        {
            public int ProductID { get; set; }
            [Required(ErrorMessage ="Please Enter Product Code")]
            public string Code { get; set; }
            [DisplayName("Sort Code")]
            [Required(ErrorMessage ="Please Enter Sort Code")]
            public int SortCode { get; set; }
            [DisplayName("Name")]
            [Required(ErrorMessage ="Please Enter Product Name")]
            public string ProductName { get; set; }
            public string Description { get; set; }
            public int SubMenuID { get; set; }
            [DisplayName("Purchase Price")]
            public double PurchasePrice { get; set; }
            [DisplayName("Sale Price")]
            public double SalePrice { get; set; }
            [DisplayName("Whole Sale Price")]
            public double WholeSalePrice { get; set; }
            public bool isUnit { get; set; }
            public bool isStock { get; set; }
            public byte[] Photo { get; set; }
            [DisplayName("Alert Quantity")]
            public int AlertQuantity { get; set; }
            [DisplayName("Discount(%)")]
            public int DiscountPercent { get; set; }
            public string BarCode { get; set; }
            public string QRCode { get; set; }
            public bool isVariant { get; set; }
            public string SubMenuName { get; set; }
            public string MainMenuName { get; set; }


        }
        public class ProductBranch
        {
            public int ID { get; set; }
            public int ProductID { get; set; }
            public int BranchID { get; set; }
        }
        public class ProductUnit
        {
            public int ID { get; set; }
            public int ProductID { get; set; }
            public int UnitID { get; set; }
            public double PurchasePrice { get; set; }
            public double SalePrice { get; set; }
            public int DiscountPercent { get; set; }
            public string Description { get; set; }
        }
        public class ProductVariant
        {
            public int ID { get; set; }
            public int ProductID { get; set; }
            public string Variant { get; set; }
        }
        public class ProductDetail
        {
            public ProductModel ProductModel { get; set; }
            public ProductBranch ProductBranch { get; set; }
            public ProductUnit ProductUnit { get; set; }
            public ProductVariant ProductVariant { get; set; }
            public List<BranchModels.BranchModel> lstBranch { get; set; }

        }
    }
}