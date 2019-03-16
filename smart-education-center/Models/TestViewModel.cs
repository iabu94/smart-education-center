using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_education_center.Models
{
    public class TestViewModel
    {
        public int Id { get; set; }
        public Nullable<int> GradeSubjectId { get; set; }
        public string TestCode { get; set; }
        public string TestName { get; set; }
        public Nullable<int> PaperPart { get; set; }
        public Nullable<int> DurationInMinutes { get; set; }
        public string TestDescription { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> IsDeleted { get; set; }

        public string SubjectName { get; set; }
    }
}