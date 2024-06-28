namespace ITAssetManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ITAssetManagementDB : DbContext
    {
        public ITAssetManagementDB()
            : base("name=ITAssetManagementDB")
        {
        }
        public virtual DbSet<cpu_invoice> cpu_invoice { get; set; }
        public virtual DbSet<laptop_invoice> laptop_invoice { get; set; }
        public virtual DbSet<loan_out_laptop> loan_out_laptop { get; set; }
        public virtual DbSet<monitor_invoice> monitor_invoice { get; set; }
        public virtual DbSet<sign_out_desktop_cpu> sign_out_desktop_cpu { get; set; }
        public virtual DbSet<sign_out_desktop_monitor> sign_out_desktop_monitor { get; set; }
        public virtual DbSet<sign_out_laptop> sign_out_laptop { get; set; }

        public virtual DbSet<assigned_desktops> assigned_desktops { get; set; }
        public virtual DbSet<assigned_laptops> assigned_laptops { get; set; }
        public virtual DbSet<assigned_sdwans> assigned_sdwans { get; set; }
        public virtual DbSet<authentication> authentication { get; set; }
        public virtual DbSet<desktop_cpus> desktop_cpus { get; set; }
        public virtual DbSet<desktop_monitors> desktop_monitors { get; set; }
        public virtual DbSet<device_status> device_status { get; set; }
        public virtual DbSet<firewall> firewalls { get; set; }
        public virtual DbSet<joined_desktops_monitors> joined_desktops_monitors { get; set; }
        public virtual DbSet<laptop> laptops { get; set; }
        public virtual DbSet<loaned_laptops> loaned_laptops { get; set; }
        public virtual DbSet<loaned_sdwans> loaned_sdwans { get; set; }
        public virtual DbSet<router_mtc> router_mtc { get; set; }
        public virtual DbSet<sdwan_laptops> sdwan_laptops { get; set; }
        public virtual DbSet<sdwan> sdwans { get; set; }
        public virtual DbSet<trailing_assigned_desktops> trailing_assigned_desktops { get; set; }
        public virtual DbSet<trailing_assigned_laptops> trailing_assigned_laptops { get; set; }
        public virtual DbSet<trailing_assigned_sdwans> trailing_assigned_sdwans { get; set; }
        public virtual DbSet<trailing_device_status> trailing_device_status { get; set; }
        public virtual DbSet<trailing_joined_desktops_monitors> trailing_joined_desktops_monitors { get; set; }
        public virtual DbSet<trailing_loaned_laptops> trailing_loaned_laptops { get; set; }
        public virtual DbSet<trailing_loaned_sdwans> trailing_loaned_sdwans { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<firewall>()
               .Property(e => e.tag_number)
               .IsUnicode(false);

            modelBuilder.Entity<firewall>()
                .Property(e => e.serial_number)
                .IsUnicode(false);

            modelBuilder.Entity<firewall>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<firewall>()
                .Property(e => e.comments)
                .IsUnicode(false);

            modelBuilder.Entity<router_mtc>()
                .Property(e => e.tag_number)
                .IsUnicode(false);

            modelBuilder.Entity<router_mtc>()
                .Property(e => e.serial_number)
                .IsUnicode(false);

            modelBuilder.Entity<router_mtc>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<router_mtc>()
                .Property(e => e.comments)
                .IsUnicode(false);

            modelBuilder.Entity<router_mtc>()
               .Property(e => e.invoices)
               .IsUnicode(false);

            modelBuilder.Entity<sdwan_laptops>()
                .Property(e => e.brand_name)
                .IsUnicode(false);

            modelBuilder.Entity<sdwan_laptops>()
                .Property(e => e.serial_number)
                .IsUnicode(false);

            modelBuilder.Entity<sdwan_laptops>()
                .Property(e => e.tag_number)
                .IsUnicode(false);

            modelBuilder.Entity<sdwan_laptops>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<sdwan_laptops>()
                .Property(e => e.comments)
                .IsUnicode(false);

            modelBuilder.Entity<sdwan>()
                .Property(e => e.description)
                .IsUnicode(false);
            modelBuilder.Entity<desktop_cpus>()
                .Property(e => e.brand_name)
                .IsUnicode(false);

            modelBuilder.Entity<desktop_cpus>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<desktop_cpus>()
                .Property(e => e.cpu_serial_number)
                .IsUnicode(false);

            modelBuilder.Entity<desktop_cpus>()
                .Property(e => e.cpu_tag_number)
                .IsUnicode(false);

            modelBuilder.Entity<desktop_cpus>()
                .Property(e => e.comments)
                .IsUnicode(false);

           

            modelBuilder.Entity<desktop_monitors>()
                .Property(e => e.brand_name)
                .IsUnicode(false);

            modelBuilder.Entity<desktop_monitors>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<desktop_monitors>()
                .Property(e => e.monitor_serial_number)
                .IsUnicode(false);

            modelBuilder.Entity<desktop_monitors>()
                .Property(e => e.monitor_tag_number)
                .IsUnicode(false);

            modelBuilder.Entity<desktop_monitors>()
                .Property(e => e.comments)
                .IsUnicode(false);

           

            modelBuilder.Entity<device_status>()
                .Property(e => e.name)
                .IsUnicode(false);

           

            

           

            modelBuilder.Entity<laptop>()
                .Property(e => e.brand_name)
                .IsUnicode(false);

            modelBuilder.Entity<laptop>()
                .Property(e => e.serial_number)
                .IsUnicode(false);

            modelBuilder.Entity<laptop>()
                .Property(e => e.tag_number)
                .IsUnicode(false);

            modelBuilder.Entity<laptop>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<laptop>()
                .Property(e => e.comments)
                .IsUnicode(false);

           

            modelBuilder.Entity<loaned_laptops>()
                .Property(e => e.descriptions)
                .IsUnicode(false);

            modelBuilder.Entity<loaned_sdwans>()
                .Property(e => e.descriptions)
                .IsUnicode(false);

            

             

            modelBuilder.Entity<trailing_device_status>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<trailing_loaned_laptops>()
                .Property(e => e.descriptions)
                .IsUnicode(false);

            modelBuilder.Entity<trailing_loaned_sdwans>()
                .Property(e => e.descriptions)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.fullname)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);


            
        }
    }
}
