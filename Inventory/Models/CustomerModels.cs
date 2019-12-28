using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Inventory.Models
{
    public class CustomerModels
    {
        public class CustomerModel
        {
            public int CustomerID { get; set; }
            public string Code { get; set; }
            [DisplayName("Customer Name")]
            public string CustomerName { get; set; }
            public int TownshipID { get; set; }
            [DisplayName("Township")]
            public string TownshipName { get; set; }
            public string Contact { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public int BranchID { get; set; }
            [DisplayName("Branch Name")]
            public string BranchName { get; set; }
            public bool IsCredit { get; set; }
            public bool IsDefault { get; set; }
        }
    }
}