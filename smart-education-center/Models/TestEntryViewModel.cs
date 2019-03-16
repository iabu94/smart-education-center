using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_education_center.Models
{
    public class TestEntryViewModel
    {
        public int Id { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> TestID { get; set; }
        public Nullable<System.DateTime> EntryDateTime { get; set; }
        public string Token { get; set; }
        public Nullable<System.DateTime> TokenExpireTime { get; set; }
        public Nullable<int> TotalMarks { get; set; }
        public string RightAnswers { get; set; }
        public string WrongAswers { get; set; }
    }
}