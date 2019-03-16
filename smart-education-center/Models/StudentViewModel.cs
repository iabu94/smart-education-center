using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smart_education_center.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string StudentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> RegisteredDate { get; set; }

        [Required(ErrorMessage = ("Enter Username"))]
        public string username { get; set; }

        [Required(ErrorMessage = ("Enter Password"))]
        public string passHash { get; set; }
    }
}