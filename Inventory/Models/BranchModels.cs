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

            public string Name { get; set; }
        }
        public class BranchModel
        {
            public int BranchID { get; set; }
            [DisplayName("Branch Name")]
            public string BranchName { get; set; }
            [DisplayName("Short Name")]
            public string ShortName { get; set; }
            public string Description { get; set; }
            public string Code { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            [DisplayName("Tax(%)")]
            public string Tax { get; set; }
            [DisplayName("Service Charges(%)")]
            public string ServiceCharges { get; set; }
            public int LanguageID { get; set; }
        }
    }
}