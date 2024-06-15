namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sdwan_laptops
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string brand_name { get; set; }

        [Required]
        [StringLength(255)]
        public string serial_number { get; set; }

        [Required]
        [StringLength(255)]
        public string tag_number { get; set; }

        [Required]
        [StringLength(255)]
        public string model { get; set; }

        [StringLength(255)]
        public string comments { get; set; }

        public byte[] attachment { get; set; }

        public int status_id { get; set; }

        public int user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime date_created { get; set; }

        public DateTime? date_updated { get; set; }

       
    }
}
