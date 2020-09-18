namespace MMT_OMS
{
    using System.Data.Entity;
    using MMT_OMS.Models;

    public partial class MMTModel : DbContext
    {
        public MMTModel()
            : base("name=MMTModel")
        {
            Database.SetInitializer<MMTModel>(new DropCreateDatabaseIfModelChanges<MMTModel>());
        }

        public virtual DbSet<Bundle> Bundles { get; set; }
        public virtual DbSet<BundleTintType> BundleTintTypes { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }
        public virtual DbSet<CustomerOrderPayment> CustomerOrderPayments { get; set; }
        public virtual DbSet<CustomerOrderShade> CustomerOrderShades { get; set; }
        public virtual DbSet<Shade> Shades { get; set; }
        public virtual DbSet<TintType> TintTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Settlement> Settlements { get; set; }
        public virtual DbSet<CustomerOrderStatus> CustomerOrderStatuses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bundle>()
                .HasMany(e => e.BundleTintTypes)
                .WithRequired(e => e.Bundle)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerOrder>()
                .Property(e => e.PhoneNumber)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CustomerOrder>()
                .HasMany(e => e.CustomerOrderPayments)
                .WithRequired(e => e.CustomerOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerOrder>()
                .HasMany(e => e.CustomerOrderShades)
                .WithRequired(e => e.CustomerOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shade>()
                .HasMany(e => e.CustomerOrderShades)
                .WithRequired(e => e.Shade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TintType>()
                .HasMany(e => e.BundleTintTypes)
                .WithRequired(e => e.TintType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TintType>()
                .HasMany(e => e.Shades)
                .WithRequired(e => e.TintType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.PasswordHash)
                .IsFixedLength();
        }
    }
}
