namespace UILHost.Infrastructure.Data.Operational.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodeToTeacher : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teachers", "PasswordResetCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teachers", "PasswordResetCode");
        }
    }
}
