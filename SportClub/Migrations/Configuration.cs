namespace SportClub.Migrations
{
    using SportClub.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SportClub.DAL.SportContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SportClub.DAL.SportContext context)
        {
              //This method will be called after migrating to the latest version.

              //You can use the DbSet<T>.AddOrUpdate() helper extension method 
              //to avoid creating duplicate seed data. E.g.
            
              //  context.People.AddOrUpdate(
              //    p => p.FullName,
              //    new Person { FullName = "Andrew Peters" },
              //    new Person { FullName = "Brice Lambson" },
              //    new Person { FullName = "Rowan Miller" }
              //  );
            

            var members = new List<Members>
            {
                new Members{FirstName="Ivana", LastName="Buncic", DateOfBirth=DateTime.Parse("1989-12-25"), Group="Pro", Sport="Volleyball", EnrollmentDate=DateTime.Parse("2009-05-01")},
                new Members{FirstName="Milica", LastName="Cvejic", DateOfBirth=DateTime.Parse("1989-10-09"), Group="Amater", Sport="Football", EnrollmentDate=DateTime.Parse("2007-05-01")},
                new Members{FirstName="Milan", LastName="Nikolic", DateOfBirth=DateTime.Parse("1985-07-15"), Group="Recreational", Sport="Swimming", EnrollmentDate=DateTime.Parse("2005-01-01")},
                new Members{FirstName="Zoran", LastName="Lukic", DateOfBirth=DateTime.Parse("1992-07-25"), Group="Recreational", Sport="Basketball", EnrollmentDate=DateTime.Parse("2014-07-30")},
                new Members{FirstName="Nenad", LastName="Kojic", DateOfBirth=DateTime.Parse("1994-11-02"), Group="Pro", Sport="Volleyball", EnrollmentDate=DateTime.Parse("2006-07-18")},
                new Members{FirstName="Nikola", LastName="Savic", DateOfBirth=DateTime.Parse("1990-09-22"), Group="Amater", Sport="Swimming", EnrollmentDate=DateTime.Parse("2011-08-22")},
                new Members{FirstName="Ivica", LastName="Ninkovic", DateOfBirth=DateTime.Parse("1991-05-16"), Group="Recreational", Sport="Basketball", EnrollmentDate=DateTime.Parse("2012-11-02")},
                new Members{FirstName="Tamara", LastName="Has", DateOfBirth=DateTime.Parse("1987-10-01"), Group="Pro", Sport="Football", EnrollmentDate=DateTime.Parse("2007-03-10")},
                new Members{FirstName="Patricija", LastName="Todic", DateOfBirth=DateTime.Parse("1989-12-12"), Group="Amater", Sport="Volleyball", EnrollmentDate=DateTime.Parse("2013-02-27")},
                new Members{FirstName="Suzana", LastName="Pajic", DateOfBirth=DateTime.Parse("1986-09-27"), Group="Recreational", Sport="Basketball", EnrollmentDate=DateTime.Parse("2014-05-11")},
                new Members{FirstName="Dragan", LastName="Mitrovic", DateOfBirth=DateTime.Parse("1965-06-21"), Group="Pro", Sport="Football", EnrollmentDate=DateTime.Parse("2006-08-01")},
                new Members{FirstName="Andrija", LastName="Kolundzija", DateOfBirth=DateTime.Parse("1990-06-22"), Group="Amater", Sport="Swimming", EnrollmentDate=DateTime.Parse("2015-03-03")},
            };

            members.ForEach(s => context.MembersDb.AddOrUpdate(p => p.LastName, s));
            context.SaveChanges();

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

            var groups = new List<Group>
            {
                new Group{GroupID=2011, Title="Pro",SportID = sport.Single( s => s.Name == "Swimming").SportID, Instructors=new List<Instructor>() },
                new Group{GroupID=2012, Title="Amater",SportID = sport.Single( s => s.Name == "Football").SportID, Instructors=new List<Instructor>()},
                new Group{GroupID=2013, Title="Recreational",SportID = sport.Single( s => s.Name == "Basketball").SportID, Instructors=new List<Instructor>()},
                new Group{GroupID=2014, Title="Pro",SportID = sport.Single( s => s.Name == "Volleyball").SportID, Instructors=new List<Instructor>()},
            };
            groups.ForEach(s => context.Groups.AddOrUpdate(p => p.GroupID, s));
            context.SaveChanges();

            AddOrUpdateInstructor(context, "Football", "Harui");
            AddOrUpdateInstructor(context, "Basketball", "Milic");
            AddOrUpdateInstructor(context, "Swimming", "Gajanin");
            AddOrUpdateInstructor(context, "Volleyball", "Ubavin");

            context.SaveChanges();

            //var enrollments = new List<Enrollment>
            //{
            //    new Enrollment{ MembersID=members.Single(s=> s.LastName=="Buncic").ID, GroupID=groups.Single(g=> g.Title=="Pro").GroupID, Grade=Grade.Perfect},
            //    new Enrollment{MembersID=members.Single(s=> s.LastName=="Cvejic").ID, GroupID=groups.Single(g=> g.Title=="Amater").GroupID, Grade=Grade.VeryGood}
            //};

            //foreach (Enrollment e in enrollments)
            //{
            //    var enrollmentInDataBase = context.Enrollments.Where(
            //        s =>
            //             s.Members.ID == e.MembersID &&
            //             s.Group.GroupID == e.GroupID).SingleOrDefault();
            //    if (enrollmentInDataBase == null)
            //    {
            //        context.Enrollments.Add(e);
            //    }
            //}
            //context.SaveChanges();
        }

        private void AddOrUpdateInstructor(DAL.SportContext context, string p1, string p2)
        {
            var crs = context.Groups.SingleOrDefault(c => c.Title == p1);
            var inst = context.Instructors.SingleOrDefault(i => i.LastName == p2);
            if (inst == null)
            {
                crs.Instructors.Add(context.Instructors.Single(i => i.LastName == p2));
            }
            //throw new NotImplementedException();
        }
    }
}
