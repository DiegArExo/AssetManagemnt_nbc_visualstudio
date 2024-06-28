namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("authentication")]
    public partial class authentication
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string token { get; set; }

        public int user_id { get; set; }

        public DateTime expiry_time { get; set; }

        public int user_created { get; set; }

        public int? user_updated { get; set; }
    }
}
