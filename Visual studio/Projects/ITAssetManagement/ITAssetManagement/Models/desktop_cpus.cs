namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class desktop_cpus
    {
         

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string brand_name { get; set; }

        [Required]
        [StringLength(100)]
        public string model { get; set; }

        [Required]
        [StringLength(50)]
        public string cpu_serial_number { get; set; }

        [Required]
        [StringLength(50)]
        public string cpu_tag_number { get; set; }

        [Column(TypeName = "text")]
        public string comments { get; set; }

        [MaxLength]
        public string attachment { get; set; }

        public int status_id { get; set; }

        public int user_assigned_id { get; set; }

        public int user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_updated { get; set; }

       
    }
}
