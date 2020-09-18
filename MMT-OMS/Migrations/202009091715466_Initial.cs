namespace MMT_OMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bundle",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 20),
                        Description = c.String(maxLength: 100),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.BundleTintType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        BundleCode = c.String(nullable: false, maxLength: 20),
                        TintTypeId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TintType", t => t.TintTypeId)
                .ForeignKey("dbo.Bundle", t => t.BundleCode)
                .Index(t => t.BundleCode)
                .Index(t => t.TintTypeId);
            
            CreateTable(
                "dbo.TintType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Shade",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        TintTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TintType", t => t.TintTypeId)
                .Index(t => t.TintTypeId);
            
            CreateTable(
                "dbo.CustomerOrderShade",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CustomerOrderId = c.Guid(nullable: false),
                        ShadeId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ShadeName = c.String(maxLength: 30),
                        LayoutImageFilePath = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerOrder", t => t.CustomerOrderId)
                .ForeignKey("dbo.Shade", t => t.ShadeId)
                .Index(t => t.CustomerOrderId)
                .Index(t => t.ShadeId);
            
            CreateTable(
                "dbo.CustomerOrder",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ClientName = c.String(nullable: false, maxLength: 50),
                        ClientFacebookName = c.String(nullable: false, maxLength: 50),
                        BundleCode = c.String(maxLength: 20),
                        Amount = c.Int(nullable: false),
                        BrandName = c.String(nullable: false, maxLength: 20),
                        PhoneNumber = c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false),
                        DeliveryAddress = c.String(nullable: false, maxLength: 200),
                        UniqueCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bundle", t => t.BundleCode)
                .Index(t => t.BundleCode);
            
            CreateTable(
                "dbo.CustomerOrderPayment",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CustomerOrderId = c.Guid(nullable: false),
                        PaymentImageFilePath = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerOrder", t => t.CustomerOrderId)
                .Index(t => t.CustomerOrderId);
            
            CreateTable(
                "dbo.Settlement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        SettlementDate = c.DateTime(nullable: false),
                        CustomerOrderId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerOrder", t => t.CustomerOrderId, cascadeDelete: true)
                .Index(t => t.CustomerOrderId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 20),
                        PasswordHash = c.Binary(nullable: false, maxLength: 64, fixedLength: true),
                        Salt = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BundleTintType", "BundleCode", "dbo.Bundle");
            DropForeignKey("dbo.Shade", "TintTypeId", "dbo.TintType");
            DropForeignKey("dbo.CustomerOrderShade", "ShadeId", "dbo.Shade");
            DropForeignKey("dbo.CustomerOrderShade", "CustomerOrderId", "dbo.CustomerOrder");
            DropForeignKey("dbo.Settlement", "CustomerOrderId", "dbo.CustomerOrder");
            DropForeignKey("dbo.CustomerOrderPayment", "CustomerOrderId", "dbo.CustomerOrder");
            DropForeignKey("dbo.CustomerOrder", "BundleCode", "dbo.Bundle");
            DropForeignKey("dbo.BundleTintType", "TintTypeId", "dbo.TintType");
            DropIndex("dbo.Settlement", new[] { "CustomerOrderId" });
            DropIndex("dbo.CustomerOrderPayment", new[] { "CustomerOrderId" });
            DropIndex("dbo.CustomerOrder", new[] { "BundleCode" });
            DropIndex("dbo.CustomerOrderShade", new[] { "ShadeId" });
            DropIndex("dbo.CustomerOrderShade", new[] { "CustomerOrderId" });
            DropIndex("dbo.Shade", new[] { "TintTypeId" });
            DropIndex("dbo.BundleTintType", new[] { "TintTypeId" });
            DropIndex("dbo.BundleTintType", new[] { "BundleCode" });
            DropTable("dbo.User");
            DropTable("dbo.Settlement");
            DropTable("dbo.CustomerOrderPayment");
            DropTable("dbo.CustomerOrder");
            DropTable("dbo.CustomerOrderShade");
            DropTable("dbo.Shade");
            DropTable("dbo.TintType");
            DropTable("dbo.BundleTintType");
            DropTable("dbo.Bundle");
        }
    }
}
