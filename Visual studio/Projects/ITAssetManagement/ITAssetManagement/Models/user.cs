namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user
    {
        
        public int id { get; set; }

        [Required]
        [StringLength(250)]
        public string fullname { get; set; }

        [Required]
        [StringLength(250)]
        public string username { get; set; }

        [Required]
        [StringLength(255)]
        public string email { get; set; }

        public int? user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_updated { get; set; }

        
    }
}
