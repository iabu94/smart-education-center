using smart_education_center.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_education_center.Areas.Admin.Models
{
    public class AssignSubjectViewModel
    {
        public List<int?> GradeList { get; set; }

        public List<string> SubjectList { get; set; }

        public GradeVsSubject GradeVsSubject { get; set; }
    }
}