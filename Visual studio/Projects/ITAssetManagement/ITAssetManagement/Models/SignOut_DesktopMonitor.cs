using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITAssetManagement.Models
{
    public class SignOut_DesktopMonitor
    {
        public desktop_monitors Sigout_monitor { get; set; }
        public int user_id { get; set; }
        public int desktop_monitor_id { get; set; }
        public string signout_document { get; set; }
    }
}