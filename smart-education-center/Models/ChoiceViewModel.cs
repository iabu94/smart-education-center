using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_education_center.Models
{
    public class ChoiceViewModel
    {
        public int Id { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public string ChoiceLabel { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> ChoiceNumber { get; set; }
    }
}