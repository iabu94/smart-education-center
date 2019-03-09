﻿using System;
using System.Collections.Generic;
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
            List<string> list = new List<string>();
            list.Add("Grade 6");
            list.Add("Grade 7");
            list.Add("Grade 8");
            list.Add("Grade 9");
            list.Add("Grade 10");
            list.Add("Grade 11");
            ViewBag.gradeList = list;
            return View();
        }

        public ActionResult ViewSubject(int gradeID)
        {
            Session["GradeID"] = gradeID;
            return View();
        }

        public ActionResult ViewPaper(int subjectID)
        {
            if (Session["GradeID"] == null)
            {
                return RedirectToAction("Index", "Exam");
            }
            Session["SubjectID"] = subjectID;
            return View();
        }

        public ActionResult ViewQuestion(int paperID)
        {
            if (Session["GradeID"] == null || Session["SubjectID"] ==null)
            {
                return RedirectToAction("Index", "Exam");
            }
            Session["PaperID"] = paperID;
            return View();
        }
    }
}