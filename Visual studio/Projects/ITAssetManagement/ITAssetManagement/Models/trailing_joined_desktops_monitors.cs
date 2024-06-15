namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class trailing_joined_desktops_monitors
    {
        public int id { get; set; }

        public int desktop_cpu_id { get; set; }

        public int desktop_monitor_id { get; set; }

        public int user_assigned_id { get; set; }

        public int user_created { get; set; }

        public int? user_update { get; set; }

        public DateTime date_created { get; set; }

        public DateTime? date_updated { get; set; }
    }
}
