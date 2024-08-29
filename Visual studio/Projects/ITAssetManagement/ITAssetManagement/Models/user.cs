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

        [StringLength(250)]
        public string fullname { get; set; }

        [StringLength(250)]
        public string username { get; set; }

        [StringLength(255)]
        public string email { get; set; }

        public int? user_created { get; set; }

        public int? user_updated { get; set; }

        public DateTime? date_created { get; set; }

        public DateTime? date_updated { get; set; }

        [StringLength(50)]
        public string token { get; set; }

        [StringLength(255)]
        public string emp_status { get; set; }

        [StringLength(255)]
        public string emp_department { get; set; }

        [StringLength(255)]
        public string emp_section { get; set; }

        [StringLength(255)]
        public string emp_position { get; set; }

        [StringLength(255)]
        public string emp_office_number { get; set; }

        [StringLength(255)]
        public string emp_office_extention { get; set; }

        [StringLength(255)]
        public string emp_cellphone { get; set; }
    }
}
