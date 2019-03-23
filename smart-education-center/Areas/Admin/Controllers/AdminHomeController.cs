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
using smart_education_center.Models;
using static smart_education_center.Common.Enum;

namespace smart_education_center.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        private CyberSchoolEntities _context = new CyberSchoolEntities();
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

                    Test testModel = new Test();
                    testModel.GradeVsSubject = _context.GradeVsSubject.Where(x => x.Grade.Grade1 == model.Grade && x.Subject.SubjectName == model.Subject).FirstOrDefault();
                    testModel.TestCode = DateTime.Now.ToString("yyMMddhhmm");
                    testModel.TestName = model.PaperName;
                    testModel.PaperPart = model.PaperPart;
                    testModel.DurationInMinutes = model.PaperTime;
                    testModel.IsActive = 1;
                    testModel.IsDeleted = 0;

                    testModel.TestDescription = "x";

                    _context.Test.Add(testModel);
                    _context.SaveChanges();

                    foreach (DataRow row in dt.Rows)
                    {
                        Question question = new Question();
                        question.TestId = testModel.Id;
                        question.QuestionNumber = Convert.ToInt32(row["Question Number"]);
                        question.Question1 = row["Question"].ToString();
                        question.IsActive = (int)IsActive.YES;
                        question.IsDeleted = (int)IsDeleted.NO;

                        _context.Question.Add(question);
                        _context.SaveChanges();

                        for (int i = 1; i < 5; i++)
                        {
                            Choice choices = new Choice();

                            choices.QuestionId = question.Id;
                            choices.ChoiceLabel = row["Choice " + i].ToString();
                            choices.ChoiceNumber = i;
                            choices.IsActive = 1;
                            choices.IsDeleted = 0;

                            _context.Choice.Add(choices);
                            _context.SaveChanges();
                            if (choices.ChoiceNumber == Convert.ToInt32(row["Answer"]))
                            {
                                question.CorrectAnswer = choices.Id;
                                _context.Entry(question).State = System.Data.Entity.EntityState.Modified;
                                _context.SaveChanges();
                            }
                        }
                    }

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