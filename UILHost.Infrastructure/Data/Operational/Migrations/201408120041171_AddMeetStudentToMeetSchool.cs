namespace UILHost.Infrastructure.Data.Operational.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMeetStudentToMeetSchool : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeetStudents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MeetSchool_Id = c.Long(nullable: false),
                        Student_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MeetSchools", t => t.MeetSchool_Id, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_Id)
                .Index(t => t.MeetSchool_Id)
                .Index(t => t.Student_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeetStudents", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.MeetStudents", "MeetSchool_Id", "dbo.MeetSchools");
            DropIndex("dbo.MeetStudents", new[] { "Student_Id" });
            DropIndex("dbo.MeetStudents", new[] { "MeetSchool_Id" });
            DropTable("dbo.MeetStudents");
        }
    }
}
