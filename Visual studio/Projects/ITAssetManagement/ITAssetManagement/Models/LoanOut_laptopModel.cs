using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITAssetManagement.Models
{
    public class LoanOut_laptopModel 
    {
        public loaned_laptops LoanOut_Laptops { get; set; }
        public int user_id { get; set; }
        public int laptop_id { get; set; }
        public string loan_out_document { get; set; }

    }
}