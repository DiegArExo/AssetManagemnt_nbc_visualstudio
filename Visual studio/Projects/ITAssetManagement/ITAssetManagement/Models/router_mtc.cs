namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class router_mtc
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string tag_number { get; set; }

        [Required]
        [StringLength(255)]
        public string serial_number { get; set; }

        [Required]
        [StringLength(255)]
        public string model { get; set; } 

        public string Processors { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Year { get; set; }

        [StringLength(255)]
        public string comments { get; set; }

        public int status_id { get; set; }

        public string invoices { get; set; }
        [MaxLength]
        public string attachment { get; set; }

        public int user_created { get; set; }

        public int user_updated { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_updated { get; set; }
    }
}
