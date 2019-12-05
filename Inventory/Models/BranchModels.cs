using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}