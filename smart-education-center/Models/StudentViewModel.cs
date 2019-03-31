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

        [Required(ErrorMessage ="Please provide your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = ("Enter Username"))]
        public string username { get; set; }

        [Required(ErrorMessage = ("Enter Password"))]
        [DataType(DataType.Password)]
        public string passHash { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("passHash", ErrorMessage = "Password and confirm password did not match. please check and try again")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please provide an email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please provide a valid email address")]
        public string UserEmail { get; set; }

        public string ResetPasswordCode { get; set; }

        public Nullable<System.DateTime> RegisteredDate { get; set; }
    }
}