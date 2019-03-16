using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_education_center.Models
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public Nullable<int> TestId { get; set; }
        public Nullable<int> LessonId { get; set; }
        public Nullable<int> QuestionNumber { get; set; }
        public string Question1 { get; set; }
        public Nullable<int> CorrectAnswer { get; set; }
        public Nullable<double> PointsOfQuestion { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> IsDeleted { get; set; }

        public List<ChoiceViewModel> CoiceVMList { get; set; }
        public string TestName { get; set; }
        public Nullable<int> DurationInMinutes { get; set; }
        public Nullable<int> PaperPart { get; set; }
        public int QuestionCount { get; set; }
    }
}