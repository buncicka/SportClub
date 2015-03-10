using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportClub.Models
{
    public class Instructor
    {
        public int ID { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

    }
}