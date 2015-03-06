using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportClub.Models
{
    public enum Grade
    {
        Bad, Good, OK, VeryGood, Perfect 
    }
    public class Enrollment
    {
        
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int MembersID { get; set; }
        public Grade? Grade { get; set; }

        public virtual Group Group { get; set; }
        public virtual Members Members { get; set; }
    }
}