namespace EDCWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThemeToScenarioContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EDCScenarioContent", "ThemeEnglish", c => c.String());
            AddColumn("dbo.EDCScenarioContent", "ThemeChinese", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EDCScenarioContent", "ThemeChinese");
            DropColumn("dbo.EDCScenarioContent", "ThemeEnglish");
        }
    }
}
