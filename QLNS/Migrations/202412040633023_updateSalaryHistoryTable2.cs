namespace QLNS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSalaryHistoryTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalaryHistories", "Result", c => c.Double(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalaryHistories", "Result");
        }
    }
}
