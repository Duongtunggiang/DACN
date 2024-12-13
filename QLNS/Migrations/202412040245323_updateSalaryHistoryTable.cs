namespace QLNS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSalaryHistoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalaryHistories", "SalaryBase", c => c.Double(nullable: true));
            AddColumn("dbo.SalaryHistories", "KPI", c => c.Double(nullable: true));
            AddColumn("dbo.SalaryHistories", "DateVacation", c => c.Long(nullable: true));
            AddColumn("dbo.SalaryHistories", "Coe", c => c.Double(nullable: true));
            AddColumn("dbo.SalaryHistories", "Revenue", c => c.Double(nullable: true));
            AddColumn("dbo.SalaryHistories", "Reward", c => c.Double(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalaryHistories", "Reward");
            DropColumn("dbo.SalaryHistories", "Revenue");
            DropColumn("dbo.SalaryHistories", "Coe");
            DropColumn("dbo.SalaryHistories", "DateVacation");
            DropColumn("dbo.SalaryHistories", "KPI");
            DropColumn("dbo.SalaryHistories", "SalaryBase");
        }
    }
}
