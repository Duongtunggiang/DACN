namespace QLNS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNametoTableSalary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Salaries", "NameEmployee", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Salaries", "NameEmployee");
        }
    }
}
