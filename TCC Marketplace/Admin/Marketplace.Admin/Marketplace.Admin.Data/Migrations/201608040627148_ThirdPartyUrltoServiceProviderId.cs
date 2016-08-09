namespace Marketplace.Admin.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdPartyUrltoServiceProviderId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Service", "ServiceProviderId", c => c.String());
            DropColumn("dbo.Service", "ThirdPartyAPI");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Service", "ThirdPartyAPI", c => c.String());
            DropColumn("dbo.Service", "ServiceProviderId");
        }
    }
}
