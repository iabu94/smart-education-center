using System;
using System.Collections.Generic;
using System.Data;
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
        public string Grade { get; set; }
        public string SubjectName { get; set; }
        public int ChoiceID { get; set; }
        public List<Answers> AnswerList { get; set; }
        public Nullable<System.DateTime> TokenExpireTime { get; set; }
    }
    public class Answers
    {
        public int QuestionID { get; set; }
        public int ChoiceID { get; set; }
        public int IsCorrect { get; set; }
        public int QuestionNumber { get; set; }
    }
}