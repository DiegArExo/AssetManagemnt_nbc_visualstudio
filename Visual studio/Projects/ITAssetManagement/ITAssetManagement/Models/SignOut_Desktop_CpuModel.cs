using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITAssetManagement.Models
{
    public class SignOut_Desktop_CpuModel
    {
        public desktop_cpus Signout_Cpu { get; set; }
        public int user_id { get; set; }
        public int desktop_cpu_id { get; set; }
        public string signout_document { get; set; }
    }
}