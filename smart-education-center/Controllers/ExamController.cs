using Newtonsoft.Json;
using smart_education_center.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
                    Subject objSubject = context.Subject.Where(o => o.Id == item.SubjectId && o.IsActive == 1 && o.IsDeleted == 0).FirstOrDefault();
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
                IQueryable<Test> testList = context.Test.Where(o => o.GradeSubjectId == gradeVsSubjectID && o.IsActive == 1 && o.IsDeleted == 0);

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

        public ActionResult Instruction(int paperID)
        {
            using (CyberSchoolEntities context = new CyberSchoolEntities())
            {
                Test objTest = context.Test.Where(o => o.Id == paperID).FirstOrDefault();
                TestViewModel objTestVM = new TestViewModel();
                objTestVM.DurationInMinutes = objTest.DurationInMinutes;
                objTestVM.PaperPart = objTest.PaperPart;
                objTestVM.TestDescription = objTest.TestDescription;
                objTestVM.TestName = objTest.TestName;
                objTestVM.SubjectName = objTest.GradeVsSubject.Subject.SubjectName;
                objTestVM.Grade = objTest.GradeVsSubject.Grade.Grade1.ToString();
                objTestVM.Id = objTest.Id;


                return PartialView("InstructionPV", objTestVM);
            }
        }

        public ActionResult AddTestEntry(int testID, int durationInMinutes)
        {
            using (CyberSchoolEntities context = new CyberSchoolEntities())
            {
                //create test entry account
                try
                {
                    if (Session["studentID"] != null)
                    {
                        TestEntry objTestEntry = new TestEntry();
                        objTestEntry.EntryDateTime = DateTime.Now;
                        objTestEntry.StudentID = (int)Session["studentID"];
                        objTestEntry.TestID = testID;
                        objTestEntry.Token = Guid.NewGuid().ToString();
                        objTestEntry.TokenExpireTime = DateTime.Now.AddMinutes(durationInMinutes);

                        context.TestEntry.Add(objTestEntry);
                        context.SaveChanges();

                        string token = objTestEntry.Token;
                        return RedirectToAction("ViewQuestion", "Exam", new { token = token });
                    }
                    else
                    {
                        TempData["nullStudentID"] = "Time was expired. Please Login again.";
                        return RedirectToAction("Index", "Login");
                    }

                }
                catch (Exception)
                {
                    TempData["dbError"] = "Server Error. Please Login again.";
                    return RedirectToAction("Index", "Login");
                    //throw;
                }
            }
        }
        //, int? questionNumber, string token
        public ActionResult ViewQuestion(int? questionNumber, string token, string answerModel)
        {
            if (token == null)
            {
                TempData["message"] = "You have invalid token. Please re-login and try again.";
                return RedirectToAction("Index", "Login");
            }
            TempData["token"] = token;
            using (CyberSchoolEntities context = new CyberSchoolEntities())
            {
                TestEntry objTestEntry = context.TestEntry.Where(x => x.Token.Equals(token)).FirstOrDefault();
                if (objTestEntry == null)
                {
                    TempData["message"] = "Token is invalid.";
                    return RedirectToAction("Index", "Login");
                }
                if (objTestEntry.TokenExpireTime < DateTime.Now)
                {
                    TempData["message"] = "The exam duration has expired at" + objTestEntry.TokenExpireTime.ToString() + ".";
                    return RedirectToAction("Index", "Login");
                }
                //questionNumber.GetValueOrDefault()
                if (questionNumber.GetValueOrDefault() < 1)
                {
                    questionNumber = 1;
                }
                Question objQuestion = context.Question.Where(o => o.TestId == objTestEntry.TestID && o.QuestionNumber == questionNumber && o.IsActive == 1 && o.IsDeleted == 0).FirstOrDefault();
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
                IQueryable<Choice> choiceList = context.Choice.Where(o => o.QuestionId == objQuestion.Id && o.IsActive == 1 && o.IsDeleted == 0);
                List<ChoiceViewModel> choiceVMList = choiceList.Select(x => new ChoiceViewModel
                {
                    ChoiceLabel = x.ChoiceLabel,
                    Id = x.Id,
                    QuestionId = x.QuestionId
                }).ToList();

                objQuestionVM.CoiceVMList = choiceVMList;
                Test objTest = context.Test.Where(o => o.Id == objTestEntry.TestID).FirstOrDefault();
                objQuestionVM.TestName = objTest.TestName;
                objQuestionVM.DurationInMinutes = objTest.DurationInMinutes;
                objQuestionVM.PaperPart = objTest.PaperPart;
                //objQuestionVM.QuestionCount = objTest.Question.Count;
                objQuestionVM.QuestionCount = context.Question.Where(o => o.TestId == objTestEntry.TestID && o.IsActive == 1 && o.IsDeleted == 0).ToList().Count;
                objQuestionVM.SubjectName = objTest.GradeVsSubject.Subject.SubjectName;
                objQuestionVM.Grade = objTest.GradeVsSubject.Grade.Grade1.ToString();
                objQuestionVM.TokenExpireTime = objTestEntry.TokenExpireTime;

                if (answerModel != null)
                {
                    objQuestionVM.AnswerList = JsonConvert.DeserializeObject<List<Answers>>(answerModel);
                    var objAnswer = objQuestionVM.AnswerList.FirstOrDefault(x => x.QuestionID == objQuestion.Id);
                    if (objAnswer != null)
                    {
                        objQuestionVM.ChoiceID = objAnswer.ChoiceID;
                    }
                }else
                {
                    if (objTestEntry.RightAnswers != null)
                    {
                        var objAnswerList = JsonConvert.DeserializeObject<List<Answers>>(objTestEntry.RightAnswers);
                        var objAnswer = objAnswerList.FirstOrDefault(x => x.QuestionID == objQuestion.Id);
                        if (objAnswer != null)
                        {
                            objQuestionVM.ChoiceID = objAnswer.ChoiceID;
                        }
                    }
                }

                return View(objQuestionVM);
            }

        }

        [HttpPost]
        public ActionResult PostAnswer(AnswerViewModel model)
        {
            if (model.token == null)
            {
                TempData["message"] = "You have invalid token. Please re-login and try again.";
                return RedirectToAction("Index", "Login");
            }

            using (CyberSchoolEntities context = new CyberSchoolEntities())
            {
                try
                {
                    int isCorrect = 0;
                    TestEntry objTestEntry = context.TestEntry.Where(x => x.Token == model.token).FirstOrDefault();
                    List<Answers> objAnswerList;
                    if (objTestEntry.RightAnswers != null)
                    {
                        string answers = objTestEntry.RightAnswers;
                        objAnswerList = JsonConvert.DeserializeObject<List<Answers>>(answers);
                        if (objAnswerList.Any(x => x.QuestionID == model.QuestionID))
                        {
                            var updateChoiceID = objAnswerList.FirstOrDefault(x => x.QuestionID == model.QuestionID);
                            if (updateChoiceID != null)
                            {
                                
                                if (model.CorrectAnswer == model.choiceID && updateChoiceID.ChoiceID != model.choiceID)
                                {
                                    updateChoiceID.IsCorrect = 1;
                                    objTestEntry.TotalMarks += model.mark;
                                }
                                else
                                {
                                    if(updateChoiceID.ChoiceID == model.CorrectAnswer && model.CorrectAnswer != model.choiceID)
                                    {
                                        updateChoiceID.IsCorrect = 0;
                                        objTestEntry.TotalMarks -= model.mark;
                                    }
                                }
                                updateChoiceID.ChoiceID = model.choiceID;

                                objTestEntry.RightAnswers = JsonConvert.SerializeObject(objAnswerList.ToList());
                                context.Entry(objTestEntry).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            if (model.CorrectAnswer == model.choiceID)
                            {
                                isCorrect = 1;
                            }
                            else
                            {
                                isCorrect = 0;
                            }
                            objAnswerList.Add(new Answers { QuestionID = model.QuestionID, ChoiceID = model.choiceID, IsCorrect = isCorrect });

                            if (isCorrect == 1)
                            {
                                objTestEntry.TotalMarks += model.mark;
                            }
                            objTestEntry.RightAnswers = JsonConvert.SerializeObject(objAnswerList.ToList());
                            context.Entry(objTestEntry).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        objAnswerList = new List<Answers>();
                        if (model.CorrectAnswer == model.choiceID)
                        {
                            isCorrect = 1;
                        }
                        else
                        {
                            isCorrect = 0;
                        }
                        objAnswerList.Add(new Answers { QuestionID = model.QuestionID, ChoiceID = model.choiceID, IsCorrect = isCorrect });

                        if (isCorrect == 1)
                        {
                            objTestEntry.TotalMarks = model.mark;
                        }
                        objTestEntry.RightAnswers = JsonConvert.SerializeObject(objAnswerList.ToList());
                        context.Entry(objTestEntry).State = EntityState.Modified;
                        context.SaveChanges();

                    }
                    
                    model.AnswerList = objAnswerList;
                    //model.AnswerList.Add(model.choiceID.ToString());
                    string answerModel = JsonConvert.SerializeObject(model.AnswerList.ToList());


                }
                catch (Exception)
                {
                    TempData["dbError"] = "Server Error. Please Login again.";
                    return RedirectToAction("Index", "Login");
                }

            }

            if (model.Direction == "forward")
            {
                model.QuestionNumber++;
            }
            else if(model.Direction == "backward")
            {
                model.QuestionNumber--;
            }else
            {
                return RedirectToAction("FinalResult", "Exam",new { token=model.token });
            }

            return RedirectToAction("ViewQuestion", "Exam", new { questionNumber = model.QuestionNumber, token = model.token, answerModel = JsonConvert.SerializeObject(model.AnswerList.ToList()) });
        }

        public ActionResult FinalResult(string token)
        {
            using(CyberSchoolEntities context=new CyberSchoolEntities())
            {
                TestEntry objTestEntry = context.TestEntry.Where(x => x.Token == token).FirstOrDefault();

                int marks = 0;
                if (objTestEntry.TotalMarks != null)
                {
                    marks = (int)objTestEntry.TotalMarks;
                }

                int testID = (int)objTestEntry.TestID;
                List<Question> objQuestionList = context.Question.Where(x => x.TestId == testID && x.IsActive == 1 && x.IsDeleted == 0).ToList();
                double totalMarks = 0;
                foreach(var item in objQuestionList)
                {
                    totalMarks += (double)item.PointsOfQuestion;
                }

                TempData["FinalMarks"] = (int)((marks / totalMarks) * 100);

                Test objTest = context.Test.Where(o => o.Id == objTestEntry.TestID).FirstOrDefault();

                TempData["Grade"] = objTest.GradeVsSubject.Grade.Grade1.ToString();
                TempData["Subject"] = objTest.GradeVsSubject.Subject.SubjectName;
                TempData["TestName"] = objTest.TestName;
                TempData["Part"] = objTest.PaperPart;

                try
                {
                    objTestEntry.Token = Guid.NewGuid().ToString();
                    objTestEntry.TokenExpireTime = DateTime.Now;
                    context.Entry(objTestEntry).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["dbError"] = "Server Error. Please Login again.";
                    return RedirectToAction("Index", "Login");
                }
            }
            return View();
        }
    }
}