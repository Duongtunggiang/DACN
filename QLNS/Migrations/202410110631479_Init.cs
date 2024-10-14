namespace QLNS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account_Position",
                c => new
                    {
                        PositionId = c.Int(nullable: false),
                        AccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PositionId, t.AccountId })
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Positions", t => t.PositionId, cascadeDelete: true)
                .Index(t => t.PositionId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Age = c.Int(nullable: true),
                        Address = c.String(),
                        Phone = c.String(),
                        Avatar = c.String(),
                        Gender = c.String(),
                        StartDate = c.DateTime(nullable: true),
                        Email = c.String(),
                        Coe = c.Double(nullable: true),
                        Description = c.String(),
                        AccountId = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Salaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SalaryBase = c.Double(nullable: false),
                        KPI = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Account_Position", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.Account_Position", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Salaries", "Id", "dbo.Employees");
            DropForeignKey("dbo.Accounts", "Id", "dbo.Employees");
            DropIndex("dbo.Salaries", new[] { "Id" });
            DropIndex("dbo.Accounts", new[] { "Id" });
            DropIndex("dbo.Account_Position", new[] { "AccountId" });
            DropIndex("dbo.Account_Position", new[] { "PositionId" });
            DropTable("dbo.Positions");
            DropTable("dbo.Salaries");
            DropTable("dbo.Employees");
            DropTable("dbo.Accounts");
            DropTable("dbo.Account_Position");
        }
    }
}
