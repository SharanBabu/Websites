using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using MentorConnect.Models;
using MentorConnect.Core;
using MentorConnect.Repository;

namespace MentorConnect.Controllers
{
    public class StudentController : Controller
    {
        CoreStudentHome ch = new CoreStudentHome();
        public ActionResult Index()
        {
            ViewBag.userfname = System.Web.HttpContext.Current.Session["fname"] as String;
            MentorList result = ch.GetInitMentorList();
            return View(result);
        }

        [HttpGet]
        public ActionResult FindSubjects()
        {
            SubjectList sl = ch.GetAllSubjects();            

            return Json(sl, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FindMentors(string subject)
        {
            MentorSearchResults msr = ch.FindMentors(subject);
            System.Web.HttpContext.Current.Session["searchsub"] = subject; 
            return View("MentorList",msr);
        }

        public ActionResult GetAllMentorships()
        {
            string username = System.Web.HttpContext.Current.Session["username"] as String;            
            MentorshipList mshList = ch.GetAllMentorships(username);
            return View("MentorshipList", mshList);
        }

        public ActionResult StartMentorship(int mshipid)
        {
            ViewBag.userfname = System.Web.HttpContext.Current.Session["fname"] as String;
            VideoConference vc = ch.GetVideoConference(mshipid);
            return View("Mentorship", vc);
        }

        public ActionResult DisplayMentorSchedule(int mentorid)
        {
            ViewBag.userfname = System.Web.HttpContext.Current.Session["fname"] as String;
            string username = System.Web.HttpContext.Current.Session["username"] as String; 
            MentorScheduleModel msm = ch.FindMentorSchedule(username, mentorid);
            return View("MentorSchedule", msm);
        }
        [HttpPost]
        public ActionResult SubmitScheduleRequest(MentorScheduleModel msm)
        {
            bool isValid = false;
            foreach(DaySchedule ds in msm._schedule){
                foreach(TimeandRequest ts in ds._timeandRequest){
                    if(ts.isRequested == 1){
                        isValid = true;
                    }
                }
            }
            if(!isValid){                
                ModelState.AddModelError("error", "No slot selected. Request cannot be made!");
                return View("MentorSchedule", msm);
            }
            else{
                if (ch.SubmitScheduleRequest(msm) > 1)
                    return RedirectToAction("Index");
                else
                    return View("MentorSchedule", msm);
            }
        }


        public ActionResult WriteReview(int mshipID)
        {
            Mentorship mentorship = ch.GetMentorship(mshipID);

            UserImage img = mentorship.MentorProfile.User.UserImage;
            Review rev = mentorship.Review;

            WriteReviewViewModel model = new WriteReviewViewModel
            {
                MentorshipId = mshipID,
                ImageFilename = img != null ? img.imagePath : "",
                MentorName = mentorship.MentorProfile.User.fname + " " + mentorship.MentorProfile.User.lname,
                Email = mentorship.MentorProfile.User.UserID,
                Subject = mentorship.subject.name,
                Rating = rev == null ? -1 : (int)rev.rating,
                Comment = rev == null ? "" : rev.comments
            };
            return View(model);
        }

        
        public void SubmitReview(int mshipID, int rating, string comments)
        {
            AccountRepository ur = new AccountRepository();
            ur.AddReview(mshipID, rating, comments);     

        }
    }
}
