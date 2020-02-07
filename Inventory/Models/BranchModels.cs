using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Inventory.Models
{
    public class BranchModels
    {
        public class BranchViewModel
        {
            public int BranchID { get; set; }

            public string BranchName { get; set; }

            public string ShortName { get; set; }

            public string Description { get; set; }

            public string Code { get; set; }

            public string Phone { get; set; }

            public string Address { get; set; }

            public string Email { get; set; }

            public string Tax { get; set; }

            public string ServiceCharges { get; set; }

        }
        public class BranchModel
        {
            public int BranchID { get; set; }
            [DisplayName("Branch Name")]
            [Required(ErrorMessage ="Please Enter Branch Name")]
            public string BranchName { get; set; }
            [DisplayName("Short Name")]
            [Required(ErrorMessage ="Please Enter Short Name for Branch")]
            public string ShortName { get; set; }
            public string Description { get; set; }
            [Required(ErrorMessage ="Please Enter Branch Code")]
            public string Code { get; set; }
            [RegularExpression(@"^(\d{11})$",ErrorMessage ="Invalid Phone Number")]
            public string Phone { get; set; }
            public string Address { get; set; }
            [EmailAddress(ErrorMessage ="Invalid Email Address")]
            public string Email { get; set; }
            [DisplayName("Tax(%)")]
            [RegularExpression(@"^(0|[1-9][0-9]?|100)$",ErrorMessage ="Invalid")]
            public string Tax { get; set; }
            [DisplayName("Service Charges(%)")]
            [RegularExpression(@"^(0|[1-9][0-9]?|100)$", ErrorMessage = "Invalid")]
            public string ServiceCharges { get; set; }
        }
    }
}