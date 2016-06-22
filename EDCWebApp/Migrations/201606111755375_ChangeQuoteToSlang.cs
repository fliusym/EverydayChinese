namespace EDCWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeQuoteToSlang : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EDCQuote", "WordID", "dbo.EDCWord");
            DropIndex("dbo.EDCQuote", new[] { "WordID" });
            CreateTable(
                "dbo.EDCSlang",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SlangEnglish = c.String(),
                        SlangChinese = c.String(),
                        SlangExampleEnglish = c.String(),
                        SlangExampleChinese = c.String(),
                        WordID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EDCWord", t => t.WordID, cascadeDelete: true)
                .Index(t => t.WordID);
            
            DropTable("dbo.EDCQuote");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.EDCSlang", "WordID", "dbo.EDCWord");
            DropIndex("dbo.EDCSlang", new[] { "WordID" });
            DropTable("dbo.EDCSlang");
            CreateIndex("dbo.EDCQuote", "WordID");
            AddForeignKey("dbo.EDCQuote", "WordID", "dbo.EDCWord", "ID", cascadeDelete: true);
        }
    }
}
