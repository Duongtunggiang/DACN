namespace QLNS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecheckinou : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckInOuts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCheck = c.String(),
                        Name = c.String(),
                        CheckInTime = c.DateTime(nullable: true),
                        CheckOutTime = c.DateTime(nullable: true),
                        Coe = c.Double(nullable: true),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Employees", "IdCheck", c => c.String());
            AddColumn("dbo.Employees", "QRCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "QRCode");
            DropColumn("dbo.Employees", "IdCheck");
            DropTable("dbo.CheckInOuts");
        }
    }
}
