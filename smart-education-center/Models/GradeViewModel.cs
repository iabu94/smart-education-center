using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_education_center.Models
{
    public class GradeViewModel
    {
        public int Id { get; set; }
        public Nullable<int> Grade1 { get; set; }
        public string GradeDescription { get; set; }
        public Nullable<int> IsDeleted { get; set; }
    }
}