using smart_education_center.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smart_education_center.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(StudentViewModel model)
        {
            using(CyberSchoolEntities context = new CyberSchoolEntities())
            {
                if (ModelState.IsValid == true)
                {
                    if (context.Student.Any(x => x.username == model.username && x.passHash == model.passHash))
                    {
                        Student objStudent = context.Student.Where(o => o.username == model.username && o.passHash == model.passHash).FirstOrDefault();
                        Session["studentName"] = objStudent.FirstName;

                        return RedirectToAction("Index", "Exam");
                    }
                    else
                    {
                        TempData["errorMessage"] = "Incorrect Username or Password!";
                        return View(model);
                    }

                }
                else
                {

                    return View(model);
                }
            }
        }
    }
}