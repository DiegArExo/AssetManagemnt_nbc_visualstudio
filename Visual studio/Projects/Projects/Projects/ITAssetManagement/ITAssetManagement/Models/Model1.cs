namespace ITAssetManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<laptop> laptops { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
                .Property(e => e.Processors)
                .IsUnicode(false);

            modelBuilder.Entity<laptop>()
                .Property(e => e.comments)
                .IsUnicode(false);

            modelBuilder.Entity<laptop>()
                .Property(e => e.attachment)
                .IsUnicode(false);

            modelBuilder.Entity<laptop>()
                .Property(e => e.type)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
