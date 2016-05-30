namespace EDCWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHubConnection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EDCHubConnection",
                c => new
                    {
                        HubConnectionID = c.String(nullable: false, maxLength: 128),
                        Connected = c.Boolean(nullable: false),
                        LoginDate = c.String(),
                        LoginTime = c.String(),
                    })
                .PrimaryKey(t => t.HubConnectionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EDCHubConnection");
        }
    }
}
