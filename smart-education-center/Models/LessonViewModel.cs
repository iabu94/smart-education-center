using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_education_center.Models
{
    public class LessonViewModel
    {
        public int Id { get; set; }
        public Nullable<int> GradeSubjectId { get; set; }
        public string LessonName { get; set; }
        public string LessonDescription { get; set; }
        public Nullable<int> LessonNumber { get; set; }
    }
}