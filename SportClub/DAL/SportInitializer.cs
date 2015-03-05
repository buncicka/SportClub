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
            var members = new List<Members>
            {
                new Members{FirstName="Ivana", LastName="Buncic", DateOfBirth=DateTime.Parse("1989-12-09"), Group="Pro", Sport="Volleyball", EnrollmentDate=DateTime.Parse("2009-05-01")},
                new Members{FirstName="Andrija", LastName="Sesic", DateOfBirth=DateTime.Parse("1992-7-23"), Group="Recreational", Sport="Swimming", EnrollmentDate=DateTime.Parse("2009-05-01")},
                new Members{FirstName="Marko", LastName="Kojic", DateOfBirth=DateTime.Parse("1984-03-09"), Group="Pro", Sport="Basketball", EnrollmentDate=DateTime.Parse("2009-05-01")},
                new Members{FirstName="Luka", LastName="Pantovic", DateOfBirth=DateTime.Parse("1987-06-17"), Group="Amater", Sport="Volleyball", EnrollmentDate=DateTime.Parse("2009-05-01")},
                new Members{FirstName="Petar", LastName="Pujic", DateOfBirth=DateTime.Parse("1990-10-28"), Group="Pro", Sport="Football", EnrollmentDate=DateTime.Parse("2009-05-01")},
                new Members{FirstName="Marija", LastName="Savic", DateOfBirth=DateTime.Parse("1992-04-22"), Group="Amater", Sport="Basketball", EnrollmentDate=DateTime.Parse("2009-05-01")}
            };
            members.ForEach(m => context.MembersDb.Add(m));
            context.SaveChanges();

            //base.Seed(context);
        }
    }
}