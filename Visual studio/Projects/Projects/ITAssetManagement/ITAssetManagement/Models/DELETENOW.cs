namespace ITAssetManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DELETENOW : DbContext
    {
        public DELETENOW()
            : base("name=DELETENOW")
        {
        }

        public virtual DbSet<role> roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
