using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_education_center.Models
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string SubjectDescription { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> IsDeleted { get; set; }

        public int gradeId { get; set; }
    }
}