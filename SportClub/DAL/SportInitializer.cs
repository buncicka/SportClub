using SportClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportClub.DAL
{
    public class SportInitializer: System.Data.Entity.DropCreateDatabaseIfModelChanges<SportContext>
    {
        protected override void Seed(SportContext context)
        {
           
            /*var groups = new List<Group>
            {
                new Group{Title="Pro",},
                new Group{Title="Amater"},
                new Group{Title="Recreational"}
            };
            groups.ForEach(g => context.Groups.Add(g));
            context.SaveChanges();*/

           /* var enrollments = new List<Enrollment>
            {
                new Enrollment{MembersID=2, GroupID=2, Grade=Grade.OK},
                new Enrollment{MembersID=3, GroupID=1, Grade=Grade.Perfect},
                new Enrollment{MembersID=5, GroupID=3, Grade=Grade.Bad},
                new Enrollment{MembersID=4, GroupID=2, Grade=Grade.VeryGood},
                new Enrollment{MembersID=3, GroupID=3, Grade=Grade.Good},
                new Enrollment{MembersID=5, GroupID=1}
            
            };
            enrollments.ForEach(e => context.Enrollments.Add(e));
            context.SaveChanges();*/
           /* var instructors = new List<Instructor>
            {
                new Instructor{ FirtName="Kim", LastName="Harui", Date=DateTime.Parse("2011-02-22")},
                new Instructor{ FirtName="Stefan", LastName="Milic", Date=DateTime.Parse("2009-11-02")},
                new Instructor{ FirtName="Branko", LastName="Gajanin", Date=DateTime.Parse("2004-09-18")},
                new Instructor{ FirtName="Jovan", LastName="Ubavin", Date=DateTime.Parse("2006-09-11")},
            };
            instructors.ForEach(s => context.Instructors.AddOrUpdate(p => p.LastName, s));
            context.SaveChanges();*/

           /* var sports = new List<Sport>
            {
                new Sport{Name="Basketball"},
                new Sport{Name="Volleyball"},
                new Sport{Name="Football"},
                new Sport{Name="Swimming"}
            };

            sports.ForEach(s => context.Sports.Add(s));
            context.SaveChanges();*/



            /*
            var instructors = new List<Instructor>
            {
                new Instructor{ FirtName="Kim", LastName="Harui", Date=DateTime.Parse("2011-02-22")},
                new Instructor{ FirtName="Stefan", LastName="Milic", Date=DateTime.Parse("2009-11-02")},
                new Instructor{ FirtName="Branko", LastName="Gajanin", Date=DateTime.Parse("2004-09-18")},
                new Instructor{ FirtName="Jovan", LastName="Ubavin", Date=DateTime.Parse("2006-09-11")},
            };
            instructors.ForEach(s => context.Instructors.AddOrUpdate(p => p.LastName, s));
            context.SaveChanges();

            var sport = new List<Sport>
            {
                new Sport{ Name="Football", Price=2000, StartDate=DateTime.Parse("2015-10-03"), InstructorID=instructors.Single(i=> i.LastName=="Milic").ID,  },
                new Sport{ Name="Volleyball", Price=3000, StartDate=DateTime.Parse("2015-08-03"), InstructorID=instructors.Single(i=> i.LastName=="Harui").ID,  },
                new Sport{ Name="Swimming", Price=3500, StartDate=DateTime.Parse("2002-11-03"), InstructorID=instructors.Single(i=> i.LastName=="Gajanin").ID,   },
                new Sport{ Name="Basketball", Price=4000, StartDate=DateTime.Parse("2008-10-12"), InstructorID=instructors.Single(i=> i.LastName=="Ubavin").ID, },
            };
            sport.ForEach(s => context.Sports.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
            var members = new List<Members>
            {
                new Members{FirstName="Ivana", LastName="Buncic", DateOfBirth=DateTime.Parse("1989-12-09"), Group="Pro", SportID=sport.Single(i=> i.Name=="Football").SportID, EnrollmentDate=DateTime.Parse("2009-05-01")},
                new Members{FirstName="Andrija", LastName="Sesic", DateOfBirth=DateTime.Parse("1992-7-23"), Group="Recreational", SportID=sport.Single(i=> i.Name=="Basketball").SportID, EnrollmentDate=DateTime.Parse("2009-05-01")},
                new Members{FirstName="Marko", LastName="Kojic", DateOfBirth=DateTime.Parse("1984-03-09"), Group="Pro", SportID=sport.Single(i=> i.Name=="Swimming").SportID, EnrollmentDate=DateTime.Parse("2009-05-01")},
                new Members{FirstName="Luka", LastName="Pantovic", DateOfBirth=DateTime.Parse("1987-06-17"), Group="Amater",SportID=sport.Single(i=> i.Name=="Volleyball").SportID, EnrollmentDate=DateTime.Parse("2009-05-01")},
                new Members{FirstName="Petar", LastName="Pujic", DateOfBirth=DateTime.Parse("1990-10-28"), Group="Pro", SportID=sport.Single(i=> i.Name=="Swimming").SportID, EnrollmentDate=DateTime.Parse("2009-05-01")},
                new Members{FirstName="Marija", LastName="Savic", DateOfBirth=DateTime.Parse("1992-04-22"), Group="Amater", SportID=sport.Single(i=> i.Name=="Volleyball").SportID, EnrollmentDate=DateTime.Parse("2009-05-01")},
            };
            members.ForEach(m => context.MembersDb.Add(m));
            context.SaveChanges();
            */


          //  base.Seed(context);
        }
    }
}