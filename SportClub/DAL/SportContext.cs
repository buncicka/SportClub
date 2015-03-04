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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}