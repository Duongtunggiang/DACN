namespace QLNS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatehistorytable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalaryHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IdCheck = c.String(),
                        Date = c.DateTime(nullable: true),
                        Salary_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Salaries", t => t.Salary_Id)
                .Index(t => t.Salary_Id);
            
            CreateTable(
                "dbo.Vacations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IdCheck = c.String(),
                        Reson = c.String(),
                        Description = c.String(),
                        Date = c.DateTime(nullable: true),
                        SalaryHistory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SalaryHistories", t => t.SalaryHistory_Id)
                .Index(t => t.SalaryHistory_Id);
            
            AddColumn("dbo.Salaries", "DateVacation", c => c.Long(nullable: false));
            AddColumn("dbo.Salaries", "IdCheck", c => c.String());
            AddColumn("dbo.CheckInOuts", "SalaryHistory_Id", c => c.Int());
            CreateIndex("dbo.CheckInOuts", "SalaryHistory_Id");
            AddForeignKey("dbo.CheckInOuts", "SalaryHistory_Id", "dbo.SalaryHistories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vacations", "SalaryHistory_Id", "dbo.SalaryHistories");
            DropForeignKey("dbo.SalaryHistories", "Salary_Id", "dbo.Salaries");
            DropForeignKey("dbo.CheckInOuts", "SalaryHistory_Id", "dbo.SalaryHistories");
            DropIndex("dbo.Vacations", new[] { "SalaryHistory_Id" });
            DropIndex("dbo.CheckInOuts", new[] { "SalaryHistory_Id" });
            DropIndex("dbo.SalaryHistories", new[] { "Salary_Id" });
            DropColumn("dbo.CheckInOuts", "SalaryHistory_Id");
            DropColumn("dbo.Salaries", "IdCheck");
            DropColumn("dbo.Salaries", "DateVacation");
            DropTable("dbo.Vacations");
            DropTable("dbo.SalaryHistories");
        }
    }
}
