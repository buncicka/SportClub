using SportClub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SportClub.DAL
{
    public class SportContext: DbContext
    {
        public SportContext(): base("SportContext")
        {

        }

        public DbSet<Members> MembersDb { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Sport> Sports { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
           /* modelBuilder.Entity<Group>()
               .HasMany(c => c.Instructors).WithMany(i => i.Groups)
                .Map(t => t.MapLeftKey("CourseID")
                .MapRightKey("InstructorID")
                .ToTable("GroupInstructor"));*/
            modelBuilder.Entity<Sport>()
                 .HasMany(c => c.Instructors).WithMany(i => i.Sports)
                 .Map(t => t.MapLeftKey("SportID")
                 .MapRightKey("ID")
                 .ToTable("SportInstructor"));
            
        }
    }
}