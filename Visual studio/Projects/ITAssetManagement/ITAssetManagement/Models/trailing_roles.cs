namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class trailing_roles
    {
        public int? id { get; set; }

        [StringLength(100)]
        public string role_name { get; set; }

        public int? user_id { get; set; }

        public int? user_created { get; set; }

        public DateTime? date_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_updated { get; set; }

        [Key]
        public DateTime deleted_at { get; set; }
    }
}
