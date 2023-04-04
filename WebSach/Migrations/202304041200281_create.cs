namespace WebSach.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Book_Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Category_Id = c.Int(nullable: false),
                        Author = c.String(maxLength: 50),
                        Create_at = c.DateTime(),
                        Update_at = c.DateTime(),
                        Avatar = c.String(maxLength: 50),
                        View = c.Int(),
                        Content = c.String(),
                        Description = c.String(),
                        User_Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Book_Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .ForeignKey("dbo.User", t => t.User_Name)
                .Index(t => t.Category_Id)
                .Index(t => t.User_Name);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Category_Id = c.Int(nullable: false, identity: true),
                        Category_Name = c.String(maxLength: 250),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Category_Id);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Comment_Id = c.Int(nullable: false, identity: true),
                        Comment_content = c.String(maxLength: 250),
                        Update_at = c.DateTime(nullable: false),
                        User_Name = c.String(nullable: false, maxLength: 50),
                        Book_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Comment_Id)
                .ForeignKey("dbo.Books", t => t.Book_Id)
                .ForeignKey("dbo.User", t => t.User_Name)
                .Index(t => t.User_Name)
                .Index(t => t.Book_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        User_Name = c.String(nullable: false, maxLength: 50),
                        Full_Name = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Avatar = c.String(maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        Create_at = c.DateTime(),
                        Last_Login = c.DateTime(),
                        Status = c.String(maxLength: 50, fixedLength: true),
                        Permission_Id = c.Boolean(),
                    })
                .PrimaryKey(t => t.User_Name);
            
            CreateTable(
                "dbo.Chapter",
                c => new
                    {
                        Chapter_Id = c.Int(nullable: false),
                        Book_Id = c.Int(nullable: false),
                        Chapter_Name = c.String(maxLength: 250),
                        Content = c.String(),
                    })
                .PrimaryKey(t => new { t.Chapter_Id, t.Book_Id })
                .ForeignKey("dbo.Books", t => t.Book_Id)
                .Index(t => t.Book_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chapter", "Book_Id", "dbo.Books");
            DropForeignKey("dbo.Comment", "User_Name", "dbo.User");
            DropForeignKey("dbo.Books", "User_Name", "dbo.User");
            DropForeignKey("dbo.Comment", "Book_Id", "dbo.Books");
            DropForeignKey("dbo.Books", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Chapter", new[] { "Book_Id" });
            DropIndex("dbo.Comment", new[] { "Book_Id" });
            DropIndex("dbo.Comment", new[] { "User_Name" });
            DropIndex("dbo.Books", new[] { "User_Name" });
            DropIndex("dbo.Books", new[] { "Category_Id" });
            DropTable("dbo.Chapter");
            DropTable("dbo.User");
            DropTable("dbo.Comment");
            DropTable("dbo.Categories");
            DropTable("dbo.Books");
        }
    }
}
