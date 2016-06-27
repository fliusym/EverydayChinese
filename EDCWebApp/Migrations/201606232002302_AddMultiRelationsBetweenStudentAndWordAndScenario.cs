namespace EDCWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMultiRelationsBetweenStudentAndWordAndScenario : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EDCScenarioContent", "StudentName", "dbo.EDCStudent");
            DropForeignKey("dbo.EDCWord", "StudentName", "dbo.EDCStudent");
            DropIndex("dbo.EDCScenarioContent", new[] { "StudentName" });
            DropIndex("dbo.EDCWord", new[] { "StudentName" });
            CreateTable(
                "dbo.StudentScenario",
                c => new
                    {
                        StudentName = c.String(nullable: false, maxLength: 128),
                        ScenarioID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentName, t.ScenarioID })
                .ForeignKey("dbo.EDCStudent", t => t.StudentName, cascadeDelete: true)
                .ForeignKey("dbo.EDCScenarioContent", t => t.ScenarioID, cascadeDelete: true)
                .Index(t => t.StudentName)
                .Index(t => t.ScenarioID);
            
            CreateTable(
                "dbo.StudentWord",
                c => new
                    {
                        StudentName = c.String(nullable: false, maxLength: 128),
                        WordID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentName, t.WordID })
                .ForeignKey("dbo.EDCStudent", t => t.StudentName, cascadeDelete: true)
                .ForeignKey("dbo.EDCWord", t => t.WordID, cascadeDelete: true)
                .Index(t => t.StudentName)
                .Index(t => t.WordID);
            
            DropColumn("dbo.EDCScenarioContent", "StudentName");
            DropColumn("dbo.EDCWord", "StudentName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EDCWord", "StudentName", c => c.String(maxLength: 128));
            AddColumn("dbo.EDCScenarioContent", "StudentName", c => c.String(maxLength: 128));
            DropForeignKey("dbo.StudentWord", "WordID", "dbo.EDCWord");
            DropForeignKey("dbo.StudentWord", "StudentName", "dbo.EDCStudent");
            DropForeignKey("dbo.StudentScenario", "ScenarioID", "dbo.EDCScenarioContent");
            DropForeignKey("dbo.StudentScenario", "StudentName", "dbo.EDCStudent");
            DropIndex("dbo.StudentWord", new[] { "WordID" });
            DropIndex("dbo.StudentWord", new[] { "StudentName" });
            DropIndex("dbo.StudentScenario", new[] { "ScenarioID" });
            DropIndex("dbo.StudentScenario", new[] { "StudentName" });
            DropTable("dbo.StudentWord");
            DropTable("dbo.StudentScenario");
            CreateIndex("dbo.EDCWord", "StudentName");
            CreateIndex("dbo.EDCScenarioContent", "StudentName");
            AddForeignKey("dbo.EDCWord", "StudentName", "dbo.EDCStudent", "StudentName");
            AddForeignKey("dbo.EDCScenarioContent", "StudentName", "dbo.EDCStudent", "StudentName");
        }
    }
}
