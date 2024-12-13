namespace QLNS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEmployeeSalaryRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Salaries", "Id", "dbo.Employees");
            DropIndex("dbo.Salaries", new[] { "Id" });
            AddColumn("dbo.Salaries", "Employee_Id", c => c.Int());
            CreateIndex("dbo.Salaries", "Employee_Id");
            AddForeignKey("dbo.Salaries", "Employee_Id", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Salaries", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.Salaries", new[] { "Employee_Id" });
            DropColumn("dbo.Salaries", "Employee_Id");
            CreateIndex("dbo.Salaries", "Id");
            AddForeignKey("dbo.Salaries", "Id", "dbo.Employees", "Id");
        }
    }
}
