namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class firewall
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

        [StringLength(255)]
        public string comments { get; set; }

        public int status_id { get; set; }

        [MaxLength]
        public string invoice { get; set; }

        [MaxLength]
        public string attachment { get; set; }

        public int user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_updated { get; set; }
    }
}
