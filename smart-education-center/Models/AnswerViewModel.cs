using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace smart_education_center.Models
{
    public class AnswerViewModel
    {
        public int mark { get; set; }
        public int choiceID { get; set; }
        public string token { get; set; }
        public int QuestionID { get; set; }
        public int TestID { get; set; }
        public string Direction { get; set; }
        public int QuestionNumber { get; set; }
        public int CorrectAnswer { get; set; }
        //public DataTable AnswerTable { get; set; }
        public List<Answers> AnswerList { get; set; }
        public int SelectQuestionNumber { get; set; }
    }
}