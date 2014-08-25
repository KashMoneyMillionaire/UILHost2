namespace UILHost.Infrastructure.Data.Operational.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentIsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "IsActive");
        }
    }
}
