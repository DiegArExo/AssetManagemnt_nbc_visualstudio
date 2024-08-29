using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITAssetManagement.Models
{
    public class LaptopSignOut
    {
        public assigned_laptops Assign_Laptop { get; set; }
        public int user_id { get; set; }
        public int laptop_id { get; set; }
        public string signout_document { get; set; }
    }
}