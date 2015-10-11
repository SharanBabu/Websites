using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MentorConnect.Models;
using MentorConnect.Core;

namespace MentorConnect.Controllers
{
    public class MentorController : Controller
    {
        //
        // GET: /MentorHome/
        CoreMentorHome ch = new CoreMentorHome();
        public ActionResult Index()
        {
            string username = System.Web.HttpContext.Current.Session["username"] as String;
            ViewBag.userfname = System.Web.HttpContext.Current.Session["fname"] as String;
            List<MentorshipRequest> msrList = ch.FindAllBriefRequests(username);
            MentorRequestsModel mrm = new MentorRequestsModel();
            mrm._allrequests = msrList;
            return View(mrm);
        }
        public ActionResult DisplayRequestDetail(int mshipID)
        {
            MentorScheduleModel msm = ch.FindthisScheduleRequest(mshipID);
            return View("EachScheduleRequest", msm);
        }
        public ActionResult DisplayMentorshipDetail(int mshipID)
        {
            MentorScheduleModel msm = ch.FindthisMentorshipSchedule(mshipID);
            return View("WeeklySchedule", msm);
        }
        public ActionResult EditProfile()
        {
            ViewBag.userfname = System.Web.HttpContext.Current.Session["fname"] as String;
            string username = System.Web.HttpContext.Current.Session["username"] as String;
            MentorScheduleModel msm = ch.FindMentorSchedule(username);
            return View("EditMentorSchedule", msm);
        }

        public ActionResult UpdateMentorProfile(MentorScheduleModel msm)
        {
            if(ch.UpdateMentorProfile(msm) > 0)
                return RedirectToAction("Index");
            else
                return View("EditMentorSchedule", msm);
        }

        public ActionResult GetAllMentorships()
        {
            string username = System.Web.HttpContext.Current.Session["username"] as String;
            ViewBag.userfname = TempData["fname"];
            MentorshipList mshList = ch.GetAllMentorships(username);
            return View("MentorshipList", mshList);
        }

        public ActionResult StartMentorship(int mshipid)
        {
            ViewBag.userfname = System.Web.HttpContext.Current.Session["fname"] as String;            
            VideoConference vc = ch.GetVideoConference(mshipid);
            return View("Mentorship",vc);
        }

        public void AcceptScheduleRespose(int mid, int stuid, int subid, int mshipid)
        {
            ch.AcceptScheduleResponse(mid,stuid,subid,mshipid);
        }
        public void RejectScheduleRespose(int mid, int stuid, int subid, int mshipid)
        {
            ch.RejectScheduleResponse(mid, stuid, subid, mshipid);
        }
    }
}
