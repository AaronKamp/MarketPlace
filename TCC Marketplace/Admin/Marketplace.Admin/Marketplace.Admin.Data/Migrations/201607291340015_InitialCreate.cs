namespace Marketplace.Admin.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        CreatedUser = c.String(),
                        UpdatedUser = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ConfigurationSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedUser = c.String(nullable: false),
                        FtpHostAddress = c.String(nullable: false),
                        FtpPort = c.Int(nullable: false),
                        FtpUser = c.String(nullable: false),
                        SshPrivateKey = c.String(nullable: false),
                        IsSshPasswordProtected = c.Boolean(nullable: false),
                        SshPrivateKeyPassword = c.String(),
                        FtpRemotePath = c.String(nullable: false),
                        FromEmail = c.String(nullable: false),
                        FromEmailPassword = c.String(nullable: false),
                        ToEmails = c.String(nullable: false),
                        SmtpClient = c.String(nullable: false),
                        SmtpPort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Country_Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        Country_Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State_Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        State_Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        CountryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.SCF",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SCF_Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        City_Name = c.String(nullable: false, maxLength: 30, unicode: false),
                        DisplayText = c.String(nullable: false, maxLength: 36, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        StateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.State", t => t.StateId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.Service",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceTypeId = c.Byte(nullable: false),
                        Tilte = c.String(nullable: false, maxLength: 255, unicode: false),
                        ShortDescription = c.String(nullable: false, maxLength: 1000, unicode: false),
                        LongDescription = c.String(maxLength: 5000, unicode: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        URL = c.String(nullable: false, maxLength: 255, unicode: false),
                        PartnerPromoCode = c.String(maxLength: 255, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        IconImage = c.String(nullable: false, maxLength: 255, unicode: false),
                        SliderImage = c.String(maxLength: 255, unicode: false),
                        CustomField1 = c.String(maxLength: 255, unicode: false),
                        CustomField2 = c.String(maxLength: 255, unicode: false),
                        CustomField3 = c.String(maxLength: 255, unicode: false),
                        MakeLive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedUser = c.String(nullable: false, maxLength: 255, unicode: false),
                        LastExtractedDate = c.DateTime(),
                        InAppPurchaseId = c.String(maxLength: 256, unicode: false),
                        PurchasePrice = c.Decimal(nullable: false, precision: 10, scale: 2),
                        ThirdPartyAPI = c.String(),
                        ServiceStatusAPIAvailable = c.Boolean(nullable: false),
                        ZipCodes = c.String(maxLength: 2000, unicode: false),
                        DisableAPIAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceType", t => t.ServiceTypeId)
                .Index(t => t.ServiceTypeId);
            
            CreateTable(
                "dbo.ServiceProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .ForeignKey("dbo.Service", t => t.ServiceId)
                .Index(t => t.ServiceId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        ProductCategoryId = c.Byte(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategory", t => t.ProductCategoryId)
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        Id = c.Byte(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceType",
                c => new
                    {
                        Id = c.Byte(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExtractFrequency",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FrequencyId = c.Byte(nullable: false),
                        LastRunDate = c.DateTime(),
                        NextRunDate = c.DateTime(),
                        IsLastExecutionSuccess = c.Boolean(),
                        TotalFailedExtracts = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedUser = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Frequency", t => t.FrequencyId)
                .Index(t => t.FrequencyId);
            
            CreateTable(
                "dbo.Frequency",
                c => new
                    {
                        Id = c.Byte(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 75, unicode: false),
                        CronExpression = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ImageQueue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(nullable: false),
                        ActualDeletedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        PurgedDate = c.DateTime(),
                        DeletedUser = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceCategory",
                c => new
                    {
                        Id = c.Byte(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServicesView",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Tilte = c.String(nullable: false, maxLength: 255, unicode: false),
                        ShortDescription = c.String(nullable: false, maxLength: 1000, unicode: false),
                        InAppPurchaseId = c.String(nullable: false, maxLength: 256, unicode: false),
                        IconImage = c.String(nullable: false, maxLength: 255, unicode: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        URL = c.String(nullable: false, maxLength: 255, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        ServiceType = c.String(nullable: false, maxLength: 50, unicode: false),
                        ZipCodes = c.String(maxLength: 2000, unicode: false),
                        States = c.String(),
                        Countries = c.String(),
                        SCFCodes = c.String(),
                        Thermostats = c.String(),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.Tilte, t.ShortDescription, t.InAppPurchaseId, t.IconImage, t.StartDate, t.EndDate, t.URL, t.IsActive, t.ServiceType });
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Byte(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ServiceSCF",
                c => new
                    {
                        SCFId = c.Int(nullable: false),
                        ServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SCFId, t.ServiceId })
                .ForeignKey("dbo.SCF", t => t.SCFId, cascadeDelete: true)
                .ForeignKey("dbo.Service", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.SCFId)
                .Index(t => t.ServiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExtractFrequency", "FrequencyId", "dbo.Frequency");
            DropForeignKey("dbo.State", "CountryId", "dbo.Country");
            DropForeignKey("dbo.SCF", "StateId", "dbo.State");
            DropForeignKey("dbo.ServiceSCF", "ServiceId", "dbo.Service");
            DropForeignKey("dbo.ServiceSCF", "SCFId", "dbo.SCF");
            DropForeignKey("dbo.Service", "ServiceTypeId", "dbo.ServiceType");
            DropForeignKey("dbo.ServiceProduct", "ServiceId", "dbo.Service");
            DropForeignKey("dbo.ServiceProduct", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product", "ProductCategoryId", "dbo.ProductCategory");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.ServiceSCF", new[] { "ServiceId" });
            DropIndex("dbo.ServiceSCF", new[] { "SCFId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.ExtractFrequency", new[] { "FrequencyId" });
            DropIndex("dbo.Product", new[] { "ProductCategoryId" });
            DropIndex("dbo.ServiceProduct", new[] { "ProductId" });
            DropIndex("dbo.ServiceProduct", new[] { "ServiceId" });
            DropIndex("dbo.Service", new[] { "ServiceTypeId" });
            DropIndex("dbo.SCF", new[] { "StateId" });
            DropIndex("dbo.State", new[] { "CountryId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropTable("dbo.ServiceSCF");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Status");
            DropTable("dbo.ServicesView");
            DropTable("dbo.ServiceCategory");
            DropTable("dbo.ImageQueue");
            DropTable("dbo.Frequency");
            DropTable("dbo.ExtractFrequency");
            DropTable("dbo.ServiceType");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.Product");
            DropTable("dbo.ServiceProduct");
            DropTable("dbo.Service");
            DropTable("dbo.SCF");
            DropTable("dbo.State");
            DropTable("dbo.Country");
            DropTable("dbo.ConfigurationSettings");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
        }
    }
}
