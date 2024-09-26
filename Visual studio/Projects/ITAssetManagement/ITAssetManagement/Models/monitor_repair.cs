namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class monitor_repair
    {
        public int id { get; set; }

        public int? monitor_id { get; set; }

        [Column(TypeName = "text")]
        public string comment { get; set; }

        [MaxLength]
        public string attachment { get; set; }

        public int? user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_updated { get; set; }

        public DateTime? date_created { get; set; }
    }
}
