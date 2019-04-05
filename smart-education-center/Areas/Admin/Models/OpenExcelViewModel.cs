using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace smart_education_center.Areas.Admin.Models
{
    public class OpenExcelViewModel
    {
        [Required]
        public HttpPostedFileBase ExcelPaperFile { get; set; }

        public string SavedFileUrl { get; set; }

        public int Grade { get; set; }

        public string Subject { get; set; }

        public string PaperName { get; set; }

        public int PaperPart { get; set; }

        public int PaperTime { get; set; }

        public string UploaderName { get; set; }

        public DataTable QuestionTable { get; set; }
    }
}