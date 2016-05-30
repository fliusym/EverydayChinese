namespace EDCWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStudentAndTeacherWithHubConnection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EDCHubConnection", "EDCStudent_StudentName", c => c.String(maxLength: 128));
            AddColumn("dbo.EDCHubConnection", "EDCTeacher_TeacherName", c => c.String(maxLength: 128));
            CreateIndex("dbo.EDCHubConnection", "EDCStudent_StudentName");
            CreateIndex("dbo.EDCHubConnection", "EDCTeacher_TeacherName");
            AddForeignKey("dbo.EDCHubConnection", "EDCStudent_StudentName", "dbo.EDCStudent", "StudentName");
            AddForeignKey("dbo.EDCHubConnection", "EDCTeacher_TeacherName", "dbo.EDCTeacher", "TeacherName");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EDCHubConnection", "EDCTeacher_TeacherName", "dbo.EDCTeacher");
            DropForeignKey("dbo.EDCHubConnection", "EDCStudent_StudentName", "dbo.EDCStudent");
            DropIndex("dbo.EDCHubConnection", new[] { "EDCTeacher_TeacherName" });
            DropIndex("dbo.EDCHubConnection", new[] { "EDCStudent_StudentName" });
            DropColumn("dbo.EDCHubConnection", "EDCTeacher_TeacherName");
            DropColumn("dbo.EDCHubConnection", "EDCStudent_StudentName");
        }
    }
}
