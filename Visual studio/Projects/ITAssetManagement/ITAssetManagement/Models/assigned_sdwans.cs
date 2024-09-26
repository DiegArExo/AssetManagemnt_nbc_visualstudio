namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class assigned_sdwans
    {
        public int id { get; set; }

        public int sdwan_id { get; set; }

        public int user_assigned_id { get; set; }

        public DateTime? start_date { get; set; }

        public DateTime? end_date { get; set; }

        public int user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_updated { get; set; }
    }
}
