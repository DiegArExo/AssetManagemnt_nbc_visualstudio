namespace ITAssetManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class delete : DbContext
    {
        public delete()
            : base("name=delete")
        {
        }

        public virtual DbSet<firewall> firewalls { get; set; }
        public virtual DbSet<router_mtc> router_mtc { get; set; }
        public virtual DbSet<sdwan_laptops> sdwan_laptops { get; set; }
        public virtual DbSet<sdwan> sdwans { get; set; }

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
        }
    }
}
