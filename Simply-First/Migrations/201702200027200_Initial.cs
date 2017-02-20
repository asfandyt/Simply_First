namespace Simply_First.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        AspNetUsers_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUsers_Id)
                .Index(t => t.AspNetUsers_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AspNetUsers_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUsers_Id)
                .Index(t => t.AspNetUsers_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId });
            
            CreateTable(
                "dbo.AspNetUsersAspNetRoles",
                c => new
                    {
                        AspNetUsers_Id = c.String(nullable: false, maxLength: 128),
                        AspNetRoles_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AspNetUsers_Id, t.AspNetRoles_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUsers_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.AspNetRoles_Id, cascadeDelete: true)
                .Index(t => t.AspNetUsers_Id)
                .Index(t => t.AspNetRoles_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserLogins", "AspNetUsers_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "AspNetUsers_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsersAspNetRoles", "AspNetRoles_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUsersAspNetRoles", "AspNetUsers_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsersAspNetRoles", new[] { "AspNetRoles_Id" });
            DropIndex("dbo.AspNetUsersAspNetRoles", new[] { "AspNetUsers_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "AspNetUsers_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "AspNetUsers_Id" });
            DropTable("dbo.AspNetUsersAspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
        }
    }
}
