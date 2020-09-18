namespace MMT_OMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrintedFlagToShade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerOrderShade", "IsPrinted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerOrderShade", "IsPrinted");
        }
    }
}
