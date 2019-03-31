using smart_education_center.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static smart_education_center.Common.Enum;

namespace smart_education_center.Areas.Admin.Controllers
{
    public class ManagePeopleController : Controller
    {
        private CyberSchoolEntities _context = new CyberSchoolEntities();

        // GET: Admin/ManagePeople
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddStudent()
        {
            return PartialView("AddStudentPV");
        }

        [HttpPost]
        public ActionResult AddStudent(Student model)
        {
            try
            {
                using (_context)
                {
                    if (_context.Student.Any(x => x.StudentNumber == model.StudentNumber))
                    {
                        TempData["ResultCode"] = (int)ResultCode.FAILED;
                        TempData["ResultMessage"] = "The student number is already exists.";
                        return RedirectToAction("Index");
                    }
                    if (_context.Student.Any(x => x.username == model.username))
                    {
                        TempData["ResultCode"] = (int)ResultCode.FAILED;
                        TempData["ResultMessage"] = "The username is already exists.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Student student = new Student();
                        student = model;
                        student.RegisteredDate = DateTime.Now;
                        TempData["ResultCode"] = (int)ResultCode.SUCCESS;
                        TempData["ResultMessage"] = "Added Successfully";
                        _context.Student.Add(student);
                        _context.SaveChanges();
                    }

                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["ResultMessage"] = e.ToString();
                return RedirectToAction("Index");
            }


        }

        public ActionResult AddTeacher()
        {
            return PartialView("AddTeacherPV");
        }

        public ActionResult AddAdmin()
        {
            return PartialView("AddAdminPV");
        }

    }
}