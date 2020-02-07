using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class SupplierModels
    {
        public class Supplier
        {
            public int SupplierID { get; set; }
            [DisplayName("Name")]
            [Required(ErrorMessage ="Please Enter Supplier Name")]
            public string SupplierName { get; set; }
            [Required(ErrorMessage ="Please Enter Code")]
            public string Code { get; set; }
            public string Contact { get; set; }
            public string Address { get; set; }
            [RegularExpression(@"^(\d{11})$", ErrorMessage = "Invalid Phone Number")]
            public string Phone { get; set; }
            [EmailAddress(ErrorMessage ="Invalid Email Address")]
            public string Email { get; set; }
            public bool isCredit { get; set; }
            public bool isDefault { get; set; }
            public int BranchID { get; set; }
            [DisplayName("Branch")]
            public string BranchName { get; set; }
            public int TownshipID { get; set; }
            [DisplayName("Township")]
            public string TownshipName { get; set; }
        }
    }
}