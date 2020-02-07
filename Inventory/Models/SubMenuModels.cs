using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Inventory.Models
{
    public class SubMenuModels
    {
        public class SubMenuModel
        {
            public int SubMenuID { get; set; }
            [Required(ErrorMessage ="Please Enter Code")]
            public string Code { get; set; }
            [DisplayName("Sort Code")]
            [Required(ErrorMessage = "Please Enter Sort Code")]
            public int SortCode { get; set; }
            [DisplayName("Name")]
            [Required(ErrorMessage = "Please Enter Sub Menu Name")]
            public string SubMenuName { get; set; }
            public byte[] Photo { get; set; }
            public int MainMenuID { get; set; }
            public string MainMenuName { get; set; }            
        }
    }
}