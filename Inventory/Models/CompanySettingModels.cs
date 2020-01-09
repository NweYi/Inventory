using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory.Models
{
    public class CompanySettingModels
    {
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public byte[] Logo { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string Tax { get; set; }

        public string ServiceCharges { get; set; }

        public bool? IsMultiCurrency { get; set; }

        public bool? IsMultiUnit { get; set; }

        public bool? IsMultiBranch { get; set; }

        public bool? IsProductByBranch { get; set; }

        public bool? IsBarcode { get; set; }

        public bool? IsProductPhoto { get; set; }
    }
}