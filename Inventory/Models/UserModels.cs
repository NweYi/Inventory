using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class UserModels
    {
        public class LoginUserModel
        {
            public LoginUserModel()
            {
                this.Branches = new List<SelectListItem>();
                this.Users = new List<SelectListItem>();
            }

            public List<SelectListItem> Branches { get; set; }

            public List<SelectListItem> Users { get; set; }

            public int UserID { get; set; }

            public string UserName { get; set; }

            [Required(ErrorMessage = "Please enter Password.")]
            [DataType(DataType.Password)]
            public string UserPassword { get; set; }

            public int BranchID { get; set; }

            public string BranchName { get; set; }

            public int AdminID { get; set; }

            [Required(ErrorMessage = "Please enter Admin.")]
            public string AdminName { get; set; }

            [Required(ErrorMessage = "Please enter Password.")]
            [DataType(DataType.Password)]
            public string AdminPassword { get; set; }

            public bool ClickedLogin { get; set; }
        }

        public class UserModel
        {
            public int UserID { get; set; }
            public String UserName { get; set; }
            public string UserPassword { get; set; }
            public int BranchID { get; set; }
            public string BranchName { get; set; }
            public bool IsDefaultLocation { get; set; }
            public int LocationID { get; set; }
            public string LocationName { get; set; }
            public List<UserModel> lstUser { get; set; }
        }
        
    }
}