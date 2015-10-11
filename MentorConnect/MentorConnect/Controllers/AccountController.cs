using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using MentorConnect.Core;
using MentorConnect.Models;
using MentorConnect.Repository;
using System.IO;

namespace MentorConnect.Controllers
{
    public class AccountController : Controller
    {
        DBMentorConnect context = new DBMentorConnect();

        //
        // GET: /Account/Login
        CoreAccount coreAcc = new CoreAccount();
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.userfname = "guest";
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginDetails)
        {
            string fname = coreAcc.IsAuthenticated(loginDetails.UserName, loginDetails.Password, loginDetails.isMentor);
            if (ModelState.IsValid && !string.IsNullOrEmpty(fname))
            {                
                System.Web.HttpContext.Current.Session["username"] = loginDetails.UserName;
                System.Web.HttpContext.Current.Session["fname"] = fname;
                System.Web.HttpContext.Current.Session["usertype"] = loginDetails.isMentor ? 1 : 2;
                if (loginDetails.isMentor == true)
                {
                    return RedirectToAction("Index","Mentor");
            }
            else
                {
                    return RedirectToAction("Index", "Student");
                }
            }
            else
                ModelState.AddModelError("","Invalid login credentials");
             return View(loginDetails);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            System.Web.HttpContext.Current.Session["fname"] = "guest";
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel regDetails)
        {
            if (coreAcc.IsSuccessfulRegisteration(regDetails))
            {
                System.Web.HttpContext.Current.Session["username"] = regDetails.UserName;
                System.Web.HttpContext.Current.Session["usertype"] = regDetails.EnrollAs;
                System.Web.HttpContext.Current.Session["fname"] = regDetails.FirstName;
                if (regDetails.EnrollAs == 1)
                {
                    return RedirectToAction("Index", "Mentor");
                }
                else if (regDetails.EnrollAs == 2)
                {
                return RedirectToAction("Index", "Student");
            }
            }
            
            return View();
        }

        public ActionResult ManageAccount(String Message)
        {
            ViewBag.StatusMessage = Message;
            AccountRepository userRep = new AccountRepository();
            User user = userRep.GetCurrentUser();
            UserImage img = userRep.GetCurrentUserImage();
            string path = "/Uploads/UserImage/m1.jpeg";
            if (img != null)
            {
                path = img.imagePath;
            }

            ManageAccountViewModel ma = new ManageAccountViewModel
            {
                FirstName = user.fname,
                LastName = user.lname,

                Address1 = user.addr1,
                Address2 = user.addr2,
                City = user.city,
                State = user.state,
                Zip = user.zip,

                PhoneNumber = user.phNo,
                Email = user.UserID,

                ImageFilename = path,
                //ImageFilename = Path.Combine(Server.MapPath("~/Images/userImages/"), "cpc111020.jpg")
                IsMentor = userRep.IsCurrentUserMentor()
            };
            ViewBag.userfname = System.Web.HttpContext.Current.Session["fname"] as String;
            return View(ma);
        }

        [HttpPost]
        public ActionResult ManageAccount(ManageAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ViewBag.userfname = System.Web.HttpContext.Current.Session["fname"] as String;
            AccountRepository userRep = new AccountRepository();
            userRep.ChangeCurrentUserName(model.FirstName, model.LastName);
            userRep.ChangeCurrentUserAddress(model.Address1, model.Address2, model.City, model.State, model.Zip);
            userRep.ChangeCurrentUserEmail(model.Email);
            userRep.ChangeCurrentUserPhone(model.PhoneNumber);
            userRep.ChangeCurrentUserPassword(model.OldPassword, model.NewPassword);

            User user = userRep.GetCurrentUser();

            if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
            {
                string directory = "/Uploads/UserImage/";
                string[] tmp = model.Email.Split('@');

                var extension = Path.GetExtension(model.ImageFile.FileName);
                var fileName = tmp[0] + "-" + user.user_type + extension;
                var path = Path.Combine(Server.MapPath(directory), fileName);
                model.ImageFile.SaveAs(path);

                userRep.ChangeUserImage(directory+fileName, extension);
                model.ImageFilename = directory + fileName;
            }
            else
            {
                UserImage img = userRep.GetCurrentUserImage();
                string path = "/Uploads/UserImage/m1.jpeg";
                if (img != null)
                {
                    path = img.imagePath;
                }

                model.ImageFilename = path;
            }

            if (model.PdfFile != null && model.PdfFile.ContentLength > 0)
            {
                string directory = "/Uploads/Resume/";
                string[] tmp = model.Email.Split('@');

                var extension = Path.GetExtension(model.PdfFile.FileName);
                var fileName = tmp[0] + extension;
                var path = Path.Combine(Server.MapPath(directory), fileName);
                model.PdfFile.SaveAs(path);     // save physical copy in server

                // save path in database
                MentorResume pdf = context.MentorResumes.Where(u => u.fk_mentorId == user.ID).FirstOrDefault();

                if (pdf != null)    // edit
                {
                    pdf.filePath = directory+fileName;
                }
                else     // create new
                {
                    pdf = new MentorResume();
                    pdf.fk_mentorId = user.ID;
                    pdf.filePath = directory + fileName;
                  

                    context.MentorResumes.Add(pdf);
                }

                /*
                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    
                }      */
                context.SaveChanges();
            }

            model.IsMentor = userRep.IsCurrentUserMentor();

            return model.IsMentor ? RedirectToAction("Index", "Mentor") : RedirectToAction("Index", "Student");
        }

        public FileResult DisplayResumePdf()
        {
            AccountRepository userRep = new AccountRepository();
            User user = userRep.GetCurrentUser();

            // Maybe put as a function in repository, but whatever
            MentorResume resume = context.MentorResumes.Where(u => u.fk_mentorId == user.ID).FirstOrDefault();

            if (resume == null)
            {
                return null;
            }


            return File(resume.filePath, "application/pdf");
        }
    }


}
