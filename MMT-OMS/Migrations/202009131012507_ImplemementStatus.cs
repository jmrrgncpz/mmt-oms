namespace MMT_OMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImplemementStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerOrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CustomerOrder", "OrderDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CustomerOrder", "OrderStatusId", c => c.Int(nullable: false));
            CreateIndex("dbo.CustomerOrder", "OrderStatusId");
            AddForeignKey("dbo.CustomerOrder", "OrderStatusId", "dbo.CustomerOrderStatus", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerOrder", "OrderStatusId", "dbo.CustomerOrderStatus");
            DropIndex("dbo.CustomerOrder", new[] { "OrderStatusId" });
            DropColumn("dbo.CustomerOrder", "OrderStatusId");
            DropColumn("dbo.CustomerOrder", "OrderDate");
            DropTable("dbo.CustomerOrderStatus");
        }
    }
}
