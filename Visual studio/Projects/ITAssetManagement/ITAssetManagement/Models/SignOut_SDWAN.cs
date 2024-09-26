using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITAssetManagement.Models
{
    public class SignOut_SDWAN
    {
        public assigned_sdwans Sign_out_Sdwan { get; set; }
        public int user_id { get; set; }
        public int sdwan_id { get; set; }
        public string signout_document { get; set; }
    }
}