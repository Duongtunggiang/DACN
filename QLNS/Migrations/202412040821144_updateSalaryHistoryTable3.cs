namespace QLNS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSalaryHistoryTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalaryHistories", "KPITarget", c => c.Double(nullable: true));
            AddColumn("dbo.SalaryHistories", "DateVacationTarget", c => c.Long(nullable: true));
            AddColumn("dbo.SalaryHistories", "Status", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalaryHistories", "Status");
            DropColumn("dbo.SalaryHistories", "DateVacationTarget");
            DropColumn("dbo.SalaryHistories", "KPITarget");
        }
    }
}
