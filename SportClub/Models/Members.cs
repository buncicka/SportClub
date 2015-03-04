using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportClub.Models
{
    public class Members
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Sport { get; set; }
        public string Group { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}