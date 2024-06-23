namespace ITAssetManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DeleteModel : DbContext
    {
        public DeleteModel()
            : base("name=DeleteModel")
        {
        }

        public virtual DbSet<authentication> authentications { get; set; }
        public virtual DbSet<cpu_invoice> cpu_invoice { get; set; }
        public virtual DbSet<laptop_invoice> laptop_invoice { get; set; }
        public virtual DbSet<loan_out_laptop> loan_out_laptop { get; set; }
        public virtual DbSet<monitor_invoice> monitor_invoice { get; set; }
        public virtual DbSet<sign_out_desktop_cpu> sign_out_desktop_cpu { get; set; }
        public virtual DbSet<sign_out_desktop_monitor> sign_out_desktop_monitor { get; set; }
        public virtual DbSet<sign_out_laptop> sign_out_laptop { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
