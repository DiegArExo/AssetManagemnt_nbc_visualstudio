namespace ITAssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sign_out_desktop_cpu
    {
        public int id { get; set; }

        public int desktop_cpu_id { get; set; }

        public int user_id { get; set; }

        [Required]
        [MaxLength]
        public string signout_document { get; set; }

        public int user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_updated { get; set; }
    }
}
