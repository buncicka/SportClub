﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SportClub.Models
{
    public class Sport
    {
        public int SportID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        //[Display(Name = "Instructor")]
        //public int? InstructorID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual ICollection<Group> Groups { get; set; }

        //public virtual Instructor Instructor { get; set; }
    }
}