namespace QLNS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebilltable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IdCheck = c.String(),
                        Date = c.DateTime(nullable: true),
                        Value = c.Double(nullable: true),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Employee_Id)
                .Index(t => t.Employee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.Bills", new[] { "Employee_Id" });
            DropTable("dbo.Bills");
        }
    }
}
