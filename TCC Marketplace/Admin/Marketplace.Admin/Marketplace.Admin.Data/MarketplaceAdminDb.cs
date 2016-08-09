namespace Marketplace.Admin.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Marketplace.Admin.Model;

    public partial class MarketplaceAdminDb : DbContext
    {
        public MarketplaceAdminDb()
            : base("name=MarketplaceAdminDb")
        {
            Database.SetInitializer(new MarketPlaceAdminDbContextInitialiser());
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }


        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<ExtractFrequency> ExtractFrequencies { get; set; }
        public virtual DbSet<Frequency> Frequencies { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<SCF> SCFs { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
        public virtual DbSet<ServiceProduct> ServiceProducts { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<ServicesView> ServicesViews { get; set; }
        public virtual DbSet<ConfigurationSettings> ConfigurationSettings { get; set; }
        public virtual DbSet<ImageQueue> ScheduledItems { get; set;}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles")
                .MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Country>()
                .Property(e => e.Country_Name)
                .IsUnicode(false);
                
            modelBuilder.Entity<Country>()
                .Property(e => e.Country_Code)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.States)
                .WithRequired(e => e.Country)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ExtractFrequency>()
                .Property(e => e.UpdatedUser)
                .IsUnicode(false);

            modelBuilder.Entity<Frequency>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Frequency>()
                .HasMany(e => e.ExtractFrequencies)
                .WithRequired(e => e.Frequency)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ServiceProducts)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductCategory>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.ProductCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SCF>()
                .Property(e => e.SCF_Code)
                .IsUnicode(false);

            modelBuilder.Entity<SCF>()
                .Property(e => e.City_Name)
                .IsUnicode(false);

            modelBuilder.Entity<SCF>()
                .Property(e => e.DisplayText)
                .IsUnicode(false);

            modelBuilder.Entity<SCF>()
                .HasMany(e => e.Services)
                .WithMany(e => e.SCFs)
                .Map(m => m.ToTable("ServiceSCF").MapLeftKey("SCFId").MapRightKey("ServiceId"));

            modelBuilder.Entity<Service>()
                .Property(e => e.Tilte)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.ShortDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.LongDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.URL)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.PartnerPromoCode)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.IconImage)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.SliderImage)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.CustomField1)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.CustomField2)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.CustomField3)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.UpdatedUser)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.InAppPurchaseId)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .Property(e => e.PurchasePrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Service>()
                .Property(e => e.ZipCodes)
                .IsUnicode(false);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.ServiceProducts)
                .WithRequired(e => e.Service)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceCategory>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceType>()
                .HasMany(e => e.Services)
                .WithRequired(e => e.ServiceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<State>()
                .Property(e => e.State_Name)
                .IsUnicode(false);

            modelBuilder.Entity<State>()
                .Property(e => e.State_Code)
                .IsUnicode(false);

            modelBuilder.Entity<State>()
                .HasMany(e => e.SCFs)
                .WithRequired(e => e.State)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ServicesView>()
                .Property(e => e.Tilte)
                .IsUnicode(false);

            modelBuilder.Entity<ServicesView>()
                .Property(e => e.ShortDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ServicesView>()
                .Property(e => e.InAppPurchaseId)
                .IsUnicode(false);

            modelBuilder.Entity<ServicesView>()
                .Property(e => e.IconImage)
                .IsUnicode(false);

            modelBuilder.Entity<ServicesView>()
                .Property(e => e.URL)
                .IsUnicode(false);

            modelBuilder.Entity<ServicesView>()
                .Property(e => e.ServiceType)
                .IsUnicode(false);

            modelBuilder.Entity<ServicesView>()
                .Property(e => e.ZipCodes)
                .IsUnicode(false);

            modelBuilder.Entity<ImageQueue>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<ImageQueue>()
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ImageQueue>()
                .Property(e => e.IsDeleted)
                .HasColumnType("bit");
        
        }

    }
}
