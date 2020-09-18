namespace MMT_OMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerOrderFieldsValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomerOrder", "ClientName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.CustomerOrder", "ClientFacebookName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.CustomerOrder", "BrandName", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomerOrder", "BrandName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.CustomerOrder", "ClientFacebookName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.CustomerOrder", "ClientName", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
