using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smart_education_center.Controllers
{
    public class ExamsController : Controller
    {
        // GET: Exams
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SubjectView(int gradeID)
        {
            TempData["gradeID"] = gradeID;
            return View();
        }

        public ActionResult ViewQuestion()
        {
            return View();
        }
    }
}