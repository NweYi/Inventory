﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class SettingModels
    {
        public class StartupViewModel
        {
            public byte[] Photo { get; set; }          
            public string Name { get; set; }          
            public string Description { get; set; }  
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string Website { get; set; }
            public string Tax { get; set; }
            public string ServiceCharges { get; set; }
            public int MultiCurrency { get; set; }
            public int Barcode { get; set; }
            public int LanguageID { get; set; }
            public int Credit { get; set; }
            public int ProductPhoto { get; set; }
        }
    }
}