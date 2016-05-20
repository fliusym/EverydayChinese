namespace EDCWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EDCLearnRequest",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EDCStudent",
                c => new
                    {
                        StudentName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.StudentName);
            
            CreateTable(
                "dbo.EDCScenarioContent",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.String(),
                        StudentName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EDCStudent", t => t.StudentName)
                .Index(t => t.StudentName);
            
            CreateTable(
                "dbo.EDCScenarioImage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        ContentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EDCScenarioContent", t => t.ContentID, cascadeDelete: true)
                .Index(t => t.ContentID);
            
            CreateTable(
                "dbo.EDCWord",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Character = c.String(),
                        Pinyin = c.String(),
                        Audio = c.String(),
                        Explanation = c.String(),
                        BasicMeanings = c.String(),
                        Date = c.String(),
                        StudentName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EDCStudent", t => t.StudentName)
                .Index(t => t.StudentName);
            
            CreateTable(
                "dbo.EDCPhrase",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Chinese = c.String(),
                        English = c.String(),
                        Pinyin = c.String(),
                        WordID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EDCWord", t => t.WordID, cascadeDelete: true)
                .Index(t => t.WordID);
            
            CreateTable(
                "dbo.EDCPhraseExample",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Chinese = c.String(),
                        Englisgh = c.String(),
                        PhraseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EDCPhrase", t => t.PhraseID, cascadeDelete: true)
                .Index(t => t.PhraseID);
            
            CreateTable(
                "dbo.EDCQuote",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Where = c.String(),
                        What = c.String(),
                        Who = c.String(),
                        WordID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EDCWord", t => t.WordID, cascadeDelete: true)
                .Index(t => t.WordID);
            
            CreateTable(
                "dbo.StudentLearnRequest",
                c => new
                    {
                        StudentName = c.String(nullable: false, maxLength: 128),
                        LearnRequestID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentName, t.LearnRequestID })
                .ForeignKey("dbo.EDCStudent", t => t.StudentName, cascadeDelete: true)
                .ForeignKey("dbo.EDCLearnRequest", t => t.LearnRequestID, cascadeDelete: true)
                .Index(t => t.StudentName)
                .Index(t => t.LearnRequestID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EDCWord", "StudentName", "dbo.EDCStudent");
            DropForeignKey("dbo.EDCQuote", "WordID", "dbo.EDCWord");
            DropForeignKey("dbo.EDCPhrase", "WordID", "dbo.EDCWord");
            DropForeignKey("dbo.EDCPhraseExample", "PhraseID", "dbo.EDCPhrase");
            DropForeignKey("dbo.EDCScenarioContent", "StudentName", "dbo.EDCStudent");
            DropForeignKey("dbo.EDCScenarioImage", "ContentID", "dbo.EDCScenarioContent");
            DropForeignKey("dbo.StudentLearnRequest", "LearnRequestID", "dbo.EDCLearnRequest");
            DropForeignKey("dbo.StudentLearnRequest", "StudentName", "dbo.EDCStudent");
            DropIndex("dbo.StudentLearnRequest", new[] { "LearnRequestID" });
            DropIndex("dbo.StudentLearnRequest", new[] { "StudentName" });
            DropIndex("dbo.EDCQuote", new[] { "WordID" });
            DropIndex("dbo.EDCPhraseExample", new[] { "PhraseID" });
            DropIndex("dbo.EDCPhrase", new[] { "WordID" });
            DropIndex("dbo.EDCWord", new[] { "StudentName" });
            DropIndex("dbo.EDCScenarioImage", new[] { "ContentID" });
            DropIndex("dbo.EDCScenarioContent", new[] { "StudentName" });
            DropTable("dbo.StudentLearnRequest");
            DropTable("dbo.EDCQuote");
            DropTable("dbo.EDCPhraseExample");
            DropTable("dbo.EDCPhrase");
            DropTable("dbo.EDCWord");
            DropTable("dbo.EDCScenarioImage");
            DropTable("dbo.EDCScenarioContent");
            DropTable("dbo.EDCStudent");
            DropTable("dbo.EDCLearnRequest");
        }
    }
}
