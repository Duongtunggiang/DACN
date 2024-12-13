namespace QLNS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateInfoEmployeeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "BHYT", c => c.String());
            AddColumn("dbo.Employees", "CCCD", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "CCCD");
            DropColumn("dbo.Employees", "BHYT");
        }
    }
}
