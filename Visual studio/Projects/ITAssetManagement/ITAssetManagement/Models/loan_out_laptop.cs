namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class loan_out_laptop
    {
        public int id { get; set; }

        public int laptop_id { get; set; }

        public int user_id { get; set; }

        [Required]
        [MaxLength]
        public string loan_out_document { get; set; }

        public int user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_updated { get; set; }
    }
}
