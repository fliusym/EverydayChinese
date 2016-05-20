namespace EDCWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeacherEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EDCTeacher",
                c => new
                    {
                        TeacherName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.TeacherName);
            
            AddColumn("dbo.EDCLearnRequest", "EDCTeacher_TeacherName", c => c.String(maxLength: 128));
            CreateIndex("dbo.EDCLearnRequest", "EDCTeacher_TeacherName");
            AddForeignKey("dbo.EDCLearnRequest", "EDCTeacher_TeacherName", "dbo.EDCTeacher", "TeacherName");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EDCLearnRequest", "EDCTeacher_TeacherName", "dbo.EDCTeacher");
            DropIndex("dbo.EDCLearnRequest", new[] { "EDCTeacher_TeacherName" });
            DropColumn("dbo.EDCLearnRequest", "EDCTeacher_TeacherName");
            DropTable("dbo.EDCTeacher");
        }
    }
}
