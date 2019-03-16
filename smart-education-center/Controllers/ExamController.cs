using smart_education_center.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smart_education_center.Controllers
{
    public class ExamController : Controller
    {
        // GET: Exams
        public ActionResult Index()
        {
            using (CyberSchoolEntities context = new CyberSchoolEntities())
            {
                IQueryable<Grade> gradeList = context.Grade.Where(o => o.IsDeleted == 0);
                List<GradeViewModel> gradeVMList = gradeList.Select(x => new GradeViewModel
                {
                    Id = x.Id,
                    Grade1 = x.Grade1,
                    GradeDescription = x.GradeDescription
                }).ToList();

                return View(gradeVMList);
            }
        }

        public ActionResult ViewSubject(int gradeID)
        {
            using (CyberSchoolEntities context = new CyberSchoolEntities())
            {
                IQueryable<GradeVsSubject> gradeVsSubjectList = context.GradeVsSubject.Where(o => o.GradeId == gradeID);
                List<GradeVsSubject> objgradeVsSubjectListt = gradeVsSubjectList.ToList();
                List<SubjectViewModel> subjectVMList = new List<SubjectViewModel>();

                foreach (GradeVsSubject item in objgradeVsSubjectListt)
                {
                    Subject objSubject = context.Subject.Where(o => o.Id == item.SubjectId && o.IsActive == 0 && o.IsDeleted == 0).FirstOrDefault();
                    SubjectViewModel objSubjectVM = new SubjectViewModel();
                    objSubjectVM.Id = objSubject.Id;
                    objSubjectVM.SubjectCode = objSubject.SubjectCode;
                    objSubjectVM.SubjectDescription = objSubject.SubjectDescription;
                    objSubjectVM.SubjectName = objSubject.SubjectName;
                    objSubjectVM.gradeId = gradeID;

                    subjectVMList.Add(objSubjectVM);
                }
                Grade objGrade = context.Grade.Where(o => o.Id == gradeID).FirstOrDefault();
                Session["Grade"] = objGrade.Grade1;
                return View(subjectVMList);
            }

        }

        public ActionResult ViewPaper(int subjectID, int gradeId)
        {
            if (gradeId == 0)
            {
                return RedirectToAction("Index", "Exam");
            }
            using (CyberSchoolEntities context = new CyberSchoolEntities())
            {
                int gradeVsSubjectID = context.GradeVsSubject.Where(o => o.GradeId == gradeId && o.SubjectId == subjectID).FirstOrDefault().Id;
                IQueryable<Test> testList = context.Test.Where(o => o.GradeSubjectId == gradeVsSubjectID && o.IsActive == 0 && o.IsDeleted == 0);

                List<TestViewModel> testVMList = testList.Select(x => new TestViewModel
                {
                    Id = x.Id,
                    PaperPart = x.PaperPart,
                    DurationInMinutes = x.DurationInMinutes,
                    TestCode = x.TestCode,
                    TestDescription = x.TestDescription,
                    TestName = x.TestName
                }).ToList();
                TempData["GradeID"] = gradeId;
                string subjectName = context.Subject.Where(o => o.Id == subjectID).FirstOrDefault().SubjectName;
                Session["SubjectName"] = subjectName;
                return View(testVMList);
            }
        }

        public ActionResult ViewQuestion(int paperID, int questionNumber)
        {
            using (CyberSchoolEntities context = new CyberSchoolEntities())
            {
                Question objQuestion = context.Question.Where(o => o.TestId == paperID && o.QuestionNumber == questionNumber && o.IsActive == 0 && o.IsDeleted == 0).FirstOrDefault();
                QuestionViewModel objQuestionVM = new QuestionViewModel();
                objQuestionVM.QuestionNumber = objQuestion.QuestionNumber;
                objQuestionVM.CorrectAnswer = objQuestion.CorrectAnswer;
                objQuestionVM.Id = objQuestion.Id;
                objQuestionVM.PointsOfQuestion = objQuestion.PointsOfQuestion;
                objQuestionVM.Question1 = objQuestion.Question1;
                objQuestionVM.TestId = objQuestion.TestId;
                if (objQuestion.LessonId != null)
                {
                    objQuestionVM.LessonId = objQuestion.LessonId;
                }
                IQueryable<Choice> choiceList = context.Choice.Where(o => o.QuestionId == objQuestion.Id && o.IsActive == 0 && o.IsDeleted == 0);
                List<ChoiceViewModel> choiceVMList = choiceList.Select(x => new ChoiceViewModel
                {
                    ChoiceLabel = x.ChoiceLabel,
                    Id = x.Id,
                    QuestionId = x.QuestionId
                }).ToList();

                objQuestionVM.CoiceVMList = choiceVMList;
                Test objTest = context.Test.Where(o => o.Id == paperID).FirstOrDefault();
                objQuestionVM.TestName = objTest.TestName;
                objQuestionVM.DurationInMinutes = objTest.DurationInMinutes;
                objQuestionVM.PaperPart = objTest.PaperPart;
                objQuestionVM.QuestionCount = context.Question.Where(o => o.TestId == paperID && o.IsActive == 0 && o.IsDeleted == 0).ToList().Count;

                return View(objQuestionVM);
            }
            //if (Session["GradeID"] == null || Session["SubjectID"] == null)
            //{
            //    return RedirectToAction("Index", "Exam");
            //}
            
        }

        [HttpPost]
        public ActionResult PostAnswer()
        {
            return View();
        }
    }
}