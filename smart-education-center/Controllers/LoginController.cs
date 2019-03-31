using smart_education_center.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
            using (CyberSchoolEntities context = new CyberSchoolEntities())
            {
                    if (context.Student.Any(x => x.username == model.username && x.passHash == model.passHash))
                    {
                        Student objStudent = context.Student.Where(o => o.username == model.username && o.passHash == model.passHash).FirstOrDefault();
                        Session["studentName"] = objStudent.FirstName;
                        Session["studentID"] = objStudent.Id;

                        return RedirectToAction("Index", "Exam");
                    }
                    else
                    {
                        TempData["message"] = "Incorrect Username or Password!";
                        return View(model);
                    }
            }
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(StudentViewModel model)
        {
            using (CyberSchoolEntities _context = new CyberSchoolEntities())
            {
                var account = _context.Student.Where(x => x.Email == model.UserEmail).FirstOrDefault();
                if (account != null)
                {
                    model.FirstName = account.FirstName;

                    account.ResetPassCode = Guid.NewGuid().ToString();
                    _context.Entry(account).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    model.ResetPasswordCode = account.ResetPassCode;

                    if (IsSentMail(model))
                    {
                        TempData["message"] = @"We've sent an email to you. Click the link in the email to reset your password.
                        If you don't see the email, check other places it might be, like your junk, spam, social or other folders.";
                        return RedirectToAction("ResetPassword");
                    }
                    else
                    {
                        TempData["message"] = "There was a problem in sending the email. Please contact your network administrator";
                        return RedirectToAction("ResetPassword");
                    }
                }
                else
                {
                    TempData["ResultCode"] = "003";
                    TempData["message"] = "There are no accounts registered with this email account. Please check your email and try angain";
                    return RedirectToAction("ResetPassword");
                }
            }
        }

        public ActionResult ChangePassword(string id)
        {
            using (CyberSchoolEntities _context = new CyberSchoolEntities())
            {
                var user = _context.Student.Where(x => x.ResetPassCode == id).FirstOrDefault();
                if (user != null)
                {
                    ChangePasswordViewModel model = new ChangePasswordViewModel
                    {
                        ResetCode = id
                    };
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (CyberSchoolEntities _context = new CyberSchoolEntities())
                {
                    var user = _context.Student.Where(x => x.ResetPassCode == model.ResetCode).FirstOrDefault();
                    user.passHash = model.NewPassword;
                    user.ResetPassCode = "";
                    _context.Configuration.ValidateOnSaveEnabled = false;
                    _context.SaveChanges();

                    TempData["ResultCode"] = "001";
                    TempData["message"] = "New password updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["message"] = "Something is invalid";
            }
            return View();
        }

        private bool IsSentMail(StudentViewModel model)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("cyber.school@outlook.com", "Cyber School - no reply");
                mailMessage.To.Add(new MailAddress(model.UserEmail));
                StringBuilder sbEmailBody = new StringBuilder();
                sbEmailBody.Append("Hi " + model.FirstName + ",<br/><br/>");
                sbEmailBody.Append("Reset your password, and we'll get you on your way.<br/>");
                sbEmailBody.Append("To change your Cyber School password, click the link below.<br/><br/>");
                sbEmailBody.Append("<a href=\"http://localhost:63976/Login/ChangePassword/" + model.ResetPasswordCode + "\">Reset my password</a><br/><br/>");
                sbEmailBody.Append("This link will expire in 24 hours, so be sure to use it right away.<br/>");
                sbEmailBody.Append("Thank you for using Cyber School.!<br/>");
                sbEmailBody.Append("The Cyber School Team");
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = sbEmailBody.ToString();
                mailMessage.Subject = "Reset Password - Cyber School";

                using (var client = new SmtpClient())
                {
                    client.Send(mailMessage);
                }
                return true;
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }
        }
        
        public ActionResult RegisterStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterStudent(StudentViewModel model)
        {
            using (CyberSchoolEntities _context = new CyberSchoolEntities())
            {
                Student student = new Student();
                student.FirstName = model.FirstName;
                student.LastName = model.LastName;
                student.RegisteredDate = DateTime.Now;
                student.username = model.username;
                student.Email = model.UserEmail;
                student.passHash = model.passHash;
                _context.Student.Add(student);
                _context.SaveChanges();

                TempData["message"] = "Account Created Successfully. Now you can login with your credentials.";
                return RedirectToAction("Index", "Login");
            }
        }

        public JsonResult ValidateUsername(string username, int id)
        {
            bool result;

            using (CyberSchoolEntities _context = new CyberSchoolEntities())
            {
                switch (id)
                {
                    case 1:
                        if (_context.Student.Any(x => x.username == username))
                        {
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                        break;

                    case 2:
                        if (_context.Student.Any(x => x.Email == username))
                        {
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                        break;
                    default:
                        result = false;
                        break;
                }

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}