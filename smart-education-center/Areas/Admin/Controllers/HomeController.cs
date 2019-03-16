using smart_education_center.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Data;

namespace smart_education_center.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;

        // GET: Admin/Home
        public ActionResult Index()
        {
            var model = new OpenExcelViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(OpenExcelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please select a excel file";
                return View(model);
            }

            if (model.ExcelPaperFile.ContentLength == 0)
            {
                TempData["ErrorMessage"] = "Please select a excel file";
                return View(model);
            }
            else
            {
                if (model.ExcelPaperFile.FileName.EndsWith("xls") || model.ExcelPaperFile.FileName.EndsWith("xlsx"))
                {
                    string fileName = "Exam" + DateTime.Now.ToString("yyMMddhhmmss") + Path.GetExtension(model.ExcelPaperFile.FileName);

                    model.SavedFileUrl = "~/Areas/Admin/ExamPapers/" + fileName;

                    fileName = Path.Combine(
                        Server.MapPath("~/Areas/Admin/ExamPapers/"), fileName);
                    model.ExcelPaperFile.SaveAs(fileName);

                    wb = excel.Workbooks.Open(fileName);
                    ws = wb.Worksheets[1];

                    model.Grade = Convert.ToInt32(ws.Cells[1, 6].value2);
                    model.Subject = ws.Cells[2, 6].value2;
                    model.PaperName = ws.Cells[3, 6].value2;
                    model.PaperPart = Convert.ToInt32(ws.Cells[4, 6].value2);
                    model.PaperTime = Convert.ToInt32(ws.Cells[5, 6].value2);

                    System.Data.DataTable dt = new System.Data.DataTable();

                    dt.Columns.Add("Question Number", typeof(int));
                    dt.Columns.Add("Question", typeof(string));
                    dt.Columns.Add("Choice 1", typeof(string));
                    dt.Columns.Add("Choice 2", typeof(string));
                    dt.Columns.Add("Choice 3", typeof(string));
                    dt.Columns.Add("Choice 4", typeof(string));
                    dt.Columns.Add("Answer", typeof(int));
                    dt.Columns.Add("Lesson", typeof(int));

                    for (int i = 10; i < 30; i = i + 4)
                    {
                        dt.Rows.Add(ws.Cells[i, 1].value2, ws.Cells[i, 2].value2, ws.Cells[i, 12].value2,
                            ws.Cells[i, 13].value2, ws.Cells[i, 14].value2, ws.Cells[i, 15].value2, ws.Cells[i, 16].value2,
                            ws.Cells[i, 17].value2);
                    }

                    model.QuestionTable = dt;



                    return View(model);
                }
                else
                {
                    TempData["ErrorMessage"] = "File type is incorrect";
                    return View(model);
                }
            }
        }

        public ActionResult StoreQuestions(System.Data.DataTable model)
        {
            return View();
        }
    }
}