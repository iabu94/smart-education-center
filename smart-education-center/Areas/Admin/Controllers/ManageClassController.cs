using smart_education_center.Areas.Admin.Models;
using smart_education_center.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static smart_education_center.Common.Enum;

namespace smart_education_center.Areas.Admin.Controllers
{
    public class ManageClassController : Controller
    {
        private CyberSchoolEntities _context = new CyberSchoolEntities();
        // GET: Admin/ManageClass
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddClass()
        {
            return PartialView("AddClassPV");
        }

        [HttpPost]
        public ActionResult AddClass(Grade model)
        {
            try
            {
                using (_context)
                {
                    if (_context.Grade.Any(x => x.Grade1 == model.Grade1))
                    {
                        TempData["ResultCode"] = (int)ResultCode.FAILED;
                        TempData["ResultMessage"] = "The class is already exists.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Grade grade = new Grade();
                        grade.Grade1 = model.Grade1;
                        grade.GradeDescription = model.GradeDescription;
                        grade.IsDeleted = (int)IsDeleted.NO;

                        _context.Grade.Add(grade);
                        _context.SaveChanges();

                        TempData["ResultCode"] = (int)ResultCode.SUCCESS;
                        TempData["ResultMessage"] = "Added Successfully";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["ResultCode"] = (int)ResultCode.FAILED;
                TempData["ResultCode"] = e.ToString();
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddSubject()
        {
            return PartialView("AddSubjectPV");
        }

        [HttpPost]
        public ActionResult AddSubject(Subject model)
        {
            try
            {
                using (_context)
                {
                    if (_context.Subject.Any(x => x.SubjectCode == model.SubjectCode))
                    {
                        TempData["ResultCode"] = (int)ResultCode.FAILED;
                        TempData["ResultMessage"] = "The subject code is already exists.";
                        return RedirectToAction("Index");
                    }
                    if (_context.Subject.Any(x => x.SubjectName == model.SubjectName))
                    {
                        TempData["ResultCode"] = (int)ResultCode.FAILED;
                        TempData["ResultMessage"] = "The subject name is already exists.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Subject subject = new Subject();
                        subject.SubjectName = model.SubjectName;
                        subject.SubjectCode = model.SubjectCode;
                        subject.SubjectDescription = model.SubjectDescription;

                        subject.IsDeleted = (int)IsDeleted.NO;
                        subject.IsActive = (int)IsActive.YES;

                        _context.Subject.Add(subject);
                        _context.SaveChanges();

                        TempData["ResultCode"] = (int)ResultCode.SUCCESS;
                        TempData["ResultMessage"] = "Added Successfully";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["ResultCode"] = (int)ResultCode.FAILED;
                TempData["ResultCode"] = e.ToString();
                return RedirectToAction("Index");
            }
        }

        public ActionResult AssignSubject()
        {
            try
            {
                using (_context)
                {
                    ViewBag.Grades = new SelectList(_context.Grade, "Id", "Grade1");
                    return PartialView("AssignSubjectPV");
                }
            }
            catch (Exception e)
            {
                TempData["ResultCode"] = (int)ResultCode.FAILED;
                TempData["ResultCode"] = e.ToString();
                return RedirectToAction("Index");
            }
        }
    }
}