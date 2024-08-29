namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sdwan
    {
        public int id { get; set; }

        public int? firewall_id { get; set; }

        public int? sdwanlaptop_id { get; set; }

        public int? router_id { get; set; }

        [MaxLength]
        public string attachment { get; set; }

        public bool type { get; set; }

        [StringLength(255)]
        public string description { get; set; }

        public int user_assigned_id { get; set; }

        public int user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_updated { get; set; }
    }
}
