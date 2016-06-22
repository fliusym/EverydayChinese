namespace EDCWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScenarioWord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EDCScenarioWord",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ChineseWord = c.String(),
                        ChineseWordPinyin = c.String(),
                        ChineseWordAudio = c.String(),
                        ImageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EDCScenarioImage", t => t.ImageId, cascadeDelete: true)
                .Index(t => t.ImageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EDCScenarioWord", "ImageId", "dbo.EDCScenarioImage");
            DropIndex("dbo.EDCScenarioWord", new[] { "ImageId" });
            DropTable("dbo.EDCScenarioWord");
        }
    }
}
