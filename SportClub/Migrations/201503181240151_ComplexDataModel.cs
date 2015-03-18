namespace SportClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplexDataModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sport", "InstructorID", "dbo.Instructor");
            DropIndex("dbo.Sport", new[] { "InstructorID" });
            CreateTable(
                "dbo.SportInstructor",
                c => new
                    {
                        SportID = c.Int(nullable: false),
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SportID, t.ID })
                .ForeignKey("dbo.Sport", t => t.SportID, cascadeDelete: true)
                .ForeignKey("dbo.Instructor", t => t.ID, cascadeDelete: true)
                .Index(t => t.SportID)
                .Index(t => t.ID);

            // Create  a department for course to point to.
            //Sql("INSERT INTO dbo.Group (GroupID, Title) VALUES ('007', 'Proba')");
            //  default value for FK points to department created above.
            //AddColumn("dbo.Group", "SportID", c => c.Int(nullable: false, defaultValue: 1));
            //AddColumn("dbo.Course", "DepartmentID", c => c.Int(nullable: false));

            
            DropColumn("dbo.Sport", "InstructorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sport", "InstructorID", c => c.Int());
            DropForeignKey("dbo.SportInstructor", "ID", "dbo.Instructor");
            DropForeignKey("dbo.SportInstructor", "SportID", "dbo.Sport");
            DropIndex("dbo.SportInstructor", new[] { "ID" });
            DropIndex("dbo.SportInstructor", new[] { "SportID" });
            DropTable("dbo.SportInstructor");
            CreateIndex("dbo.Sport", "InstructorID");
            AddForeignKey("dbo.Sport", "InstructorID", "dbo.Instructor", "ID");
        }
    }
}
