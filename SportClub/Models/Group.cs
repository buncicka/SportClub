using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportClub.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public string Title { get; set; }
        public int SportID { get; set; }

        public virtual Sport Sport { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}