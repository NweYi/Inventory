using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class UserModels
    {
        public class LoginViewModel
        {
            public int UserID { get; set; }

            [Display(Name="Name")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Please enter Password.")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            public int BranchID { get; set; }
        }
    }
}