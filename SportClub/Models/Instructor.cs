using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportClub.Models
{
    public class Instructor
    {
        public int ID { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start date")]
        public DateTime Date { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

    }
}