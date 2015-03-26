using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportClub.Models
{
    public class Group
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int GroupID { get; set; }

        [Required(ErrorMessage = "The Title field is required.")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Title { get; set; }

        public int SportID { get; set; }

        public virtual Sport Sport { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
    }
}