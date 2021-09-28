namespace Euphoria.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roletableAddeded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RoleMappings", "RoleId", "dbo.Roles");
            DropIndex("dbo.RoleMappings", new[] { "RoleId" });
            AddColumn("dbo.UserMasters", "roleId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserMasters", "roleId");
            AddForeignKey("dbo.UserMasters", "roleId", "dbo.Roles", "Id", cascadeDelete: true);
            DropTable("dbo.RoleMappings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RoleMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.UserMasters", "roleId", "dbo.Roles");
            DropIndex("dbo.UserMasters", new[] { "roleId" });
            DropColumn("dbo.UserMasters", "roleId");
            CreateIndex("dbo.RoleMappings", "RoleId");
            AddForeignKey("dbo.RoleMappings", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
        }
    }
}
