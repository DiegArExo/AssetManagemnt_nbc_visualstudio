namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class laptop
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string brand_name { get; set; }

        [Required]
        [StringLength(50)]
        public string serial_number { get; set; }

        [Required]
        [StringLength(50)]
        public string tag_number { get; set; }

        [Required]
        [StringLength(100)]
        public string model { get; set; }

        public string Processors { get; set; }

        public int? Year { get; set; }

        [Column(TypeName = "text")]
        public string comments { get; set; }

        public string attachment { get; set; }

        [Required]
        [StringLength(1)]
        public string type { get; set; }

        public int? device_status_id { get; set; }

        public int? user_assigned_id { get; set; }

        public int user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime date_created { get; set; }

        public DateTime? date_updated { get; set; }
    }
}
