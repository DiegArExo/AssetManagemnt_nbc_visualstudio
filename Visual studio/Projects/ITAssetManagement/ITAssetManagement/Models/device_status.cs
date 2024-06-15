namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class device_status
    {
         
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public int? user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_updated { get; set; }

        
    }
}
