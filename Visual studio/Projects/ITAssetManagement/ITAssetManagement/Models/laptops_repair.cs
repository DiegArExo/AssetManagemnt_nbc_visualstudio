namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class laptops_repair
    {
        public int id { get; set; }

        public int? laptop_id { get; set; }

        [Column(TypeName = "text")]
        public string comment { get; set; }

        public string attachment { get; set; }

        public int? user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_updated { get; set; }

        public DateTime? date_created { get; set; }
    }
}
