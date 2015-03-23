using System.Collections.Generic;
using SportClub.Models;

namespace SportClub.ViewModels
{
    public class SportIndexData
    {
        public IEnumerable<Sport> Sports { get; set; }
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
