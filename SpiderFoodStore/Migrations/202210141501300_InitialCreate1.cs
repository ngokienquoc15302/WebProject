namespace SpiderFoodStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryDate = c.DateTime(nullable: false),
                        NameOfConsignee = c.String(),
                        AddressOfConsignee = c.String(),
                        PhoneOfConsignee = c.String(),
                        isDelivered = c.Boolean(nullable: false),
                        isPaid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CategoryId = c.Int(nullable: false),
                        Size = c.String(),
                        Weight = c.String(),
                        Description = c.String(),
                        ImagePath = c.String(),
                        AtUpdate = c.DateTime(nullable: false),
                        RemainingAmount = c.Int(nullable: false),
                        QuanlityPurchased = c.Int(nullable: false),
                        NumberOfViews = c.Int(nullable: false),
                        isHide = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Products");
            DropTable("dbo.OrderInvoices");
            DropTable("dbo.Categories");
        }
    }
}
