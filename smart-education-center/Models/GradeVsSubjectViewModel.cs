using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_education_center.Models
{
    public class GradeVsSubjectViewModel
    {
        public int Id { get; set; }
        public Nullable<int> GradeId { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public string Description { get; set; }
    }
}