namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class trailing_loaned_sdwans
    {
        public int id { get; set; }

        public int loaned_sdwans_id { get; set; }

        public int user_loaned_id { get; set; }

        [StringLength(255)]
        public string descriptions { get; set; }

        public int user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_updated { get; set; }
    }
}
