using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MentorConnect.Repository;
using MentorConnect.Models;

namespace MentorConnect.Core
{
    public class CoreMentorHome
    {
        UserRepository mr = new UserRepository();

        public VideoConference GetVideoConference(int mshipID)
        {
            VideoConference res = new VideoConference();
            res.isMentor = true;
            Mentorship mship = mr.GetMentorship(mshipID);
            int mid = mship.fk_mentorId;
            int sid = mship.fk_studentId;

            res.mname = mr.GetNameFromID(mid);
            res.sname = mr.GetNameFromID(sid);
            res.mshipID = mshipID;

            return res;
        }
        public MentorshipList GetAllMentorships(string mentorName)
        {
            int mentorID = mr.GetIDfromEmail(mentorName);
            List<Mentorship> mshipsFromDB = mr.GetAllMentorships(mentorID);
            //form the mentorshiplist object for UI
            List<MentorshipModel> _allList = new List<MentorshipModel>();
            foreach(Mentorship m in mshipsFromDB){
                MentorshipModel msm = new MentorshipModel();
                msm.mentorID = m.fk_mentorId;
                msm.studentID = m.fk_studentId;
                msm.subjectID = m.fk_subject;
                msm.mentorshipID = m.id;
                msm.mentor = mr.GetNameFromID(m.fk_mentorId);
                msm.student = mr.GetNameFromID(m.fk_studentId);
                msm.subject = mr.FindSubnamefromCode(m.fk_subject);
                msm.isCurrentlyActive = CheckIfCurrentlyActive(m.id);
                msm.isPast = CheckIfPast(m.id);
                msm.isFuture = !msm.isCurrentlyActive && !msm.isPast;
                _allList.Add(msm);
            }
            MentorshipList result = new MentorshipList();
            result.isMentor = true;
            result._allMShips = _allList;
            return result;
        }

        private bool CheckIfPast(int mshipid)
        {
            int currentDayID = GetCurrentDayID();
            int currentTimeID = GetCurrentTimeID();
            List<StudentSchedule> allMSch = mr.GetStudentScheduleBasedOnMShipID(mshipid);
            foreach (StudentSchedule m in allMSch)
            {
                if ((currentDayID == m.fk_day_Id && currentTimeID == m.fk_time_id) || (currentDayID < m.fk_day_Id) || (currentDayID == m.fk_day_Id && currentTimeID < m.fk_time_id))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckIfCurrentlyActive(int mshipid)
        {
            int currentDayID = GetCurrentDayID();
            int currentTimeID = GetCurrentTimeID();
            List<MentorSchedule> allMSch = mr.GetMentorScheduleBasedOnMShipID(mshipid);
            foreach(MentorSchedule m in allMSch){
                if(currentDayID == m.fk_day_Id && currentTimeID == m.fk_time_id){
                    return true;
                }
            }
            return false;
        }
        private int GetCurrentDayID()
        {
            string day = DateTime.Now.DayOfWeek.ToString();
            int dayid = 0;
            switch(day){
                case "Sunday":
                    dayid = 1;
                    break;
                case "Monday":
                    dayid = 2;
                    break;
                case "Tuesday":
                    dayid = 3;
                    break;
                case "Wednesday":
                    dayid = 4;
                    break;
                case "Thursday":
                    dayid = 5;
                    break;
                case "Friday":
                    dayid = 6;
                    break;
                case "Saturday":
                    dayid = 7;
                    break;
            }
            return dayid;
        }
        private int GetCurrentTimeID()
        {
            TimeSpan ts = DateTime.Now.TimeOfDay;
            List<time> allTime = mr.GetAllTime();
            TimeSpan hoursTime = new TimeSpan(ts.Hours,0,0);
            foreach (time t in allTime)
            {
                if(hoursTime == t.starttime){
                    return t.timeid;
                }
            }
            if (hoursTime > new TimeSpan(22, 0, 0))
            {
                return 14;
            }
            return 0;
        }
        public MentorScheduleModel FindMentorSchedule(string username)
        {
            int mentorId = mr.GetIDfromEmail(username);
            List<MentorSchedule> scheduleList = mr.GetMentorScheduleForMentor(mentorId);
            Mentor mp = mr.GetMentorProfileDetails(mentorId);
            MentorScheduleModel msm = new MentorScheduleModel();
            msm._mentorId = mentorId;
            msm.studentId = 0;
            msm.imgpath = mp._imagePath;
            msm.major = mp.major;
            msm.description = mp.description;
            msm.name = mp._userName;
            msm.subject = mp.subjects.Count > 0 ? mp.subjects.ToList().First().id : 1;
            msm._subjects = GetAllSubjects();
            List<DaySchedule> schList = new List<DaySchedule>();

            for (int i = 1; i <= 7; i++)
            {
                DaySchedule ds = new DaySchedule();
                ds.day = i;
                List<TimeandRequest> timeList = new List<TimeandRequest>();
                List<MentorSchedule> eachdayschedulelist = (from l in scheduleList where l.fk_day_Id == i select l).ToList();
                foreach (MentorSchedule eachSchedule in eachdayschedulelist)
                {
                    TimeandRequest tnr = new TimeandRequest();
                    tnr.timeId = eachSchedule.fk_time_id;
                    tnr.isMentorAvailable = eachSchedule.isMentoring;
                    timeList.Add(tnr);
                }
                ds._timeandRequest = timeList;
                schList.Add(ds);
            }
            msm._schedule = schList;
            return msm;
        }

        public IEnumerable<SelectListItem> GetAllSubjects()
        {
            List<subject> allSubs = mr.SelectAllSubjects();
            var subjects = allSubs
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.id.ToString(),
                                    Text = x.name
                                });

            return new SelectList(subjects, "Value", "Text");
        }

        public int UpdateMentorProfile(MentorScheduleModel msm)
        {
            return mr.UpdateMentorProfile(msm);
        }
        public List<MentorshipRequest> FindAllBriefRequests(string uname)
        {
            int mentorId = mr.GetIDfromEmail(uname);
            List<Mentorship> reqList = mr.GetBriefMentorScheduleRequests(mentorId);
            List<MentorshipRequest> msrList = new List<MentorshipRequest>();
            
            foreach (Mentorship mship in reqList)
            {
                    MentorshipRequest msr = new MentorshipRequest();
                    msr._mentorId = mship.fk_mentorId;
                    msr.studentId = mship.fk_studentId;
                    msr.stuname = mr.GetNameFromID(mship.fk_studentId);
                    msr.subject = mship.subject.id;
                    msr.subject_name = mship.subject.name;
                    msr.mentorshipID = mship.id;
                    msrList.Add(msr);            
            }
            return msrList;
        }
        public MentorScheduleModel FindthisScheduleRequest(int mshipID)
        {

            List<MentorScheduleRequest> reqList = mr.GetMentorScheduleRequests(mshipID);            
           Mentorship mship = mr.GetMentorship(mshipID);
                    MentorScheduleModel msm = new MentorScheduleModel();
                    msm._mentorId = mship.fk_mentorId;
                    msm.subject = mship.fk_subject;
                    msm.subjectName = mship.subject.name;
                    msm.studentId = mship.fk_studentId;
                    msm.comment = mship.comments;//same mentorship id for all mentorschedulerequest records
                    msm.name = mr.GetNameFromID(mship.fk_studentId);
                    msm.imgpath = mr.GetStuImgPath(mship.fk_studentId);
                    msm.mentorShipID = mshipID;
                    List<DaySchedule> schList = new List<DaySchedule>();
                    List<MentorSchedule> allMyBusySchedule = mr.GetMentorBusySchedule(mship.fk_mentorId);
                    for (int i = 1; i <= 7; i++)
                    {
                        DaySchedule ds = new DaySchedule();
                        ds.day = i;
                        List<TimeandRequest> timeList = new List<TimeandRequest>();
                        List<MentorScheduleRequest> eachdayschedulelist = (from l in reqList where l.fk_day_Id == i select l).ToList();
                        foreach (MentorScheduleRequest eachSchedule in eachdayschedulelist)
                        {
                            TimeandRequest tnr = new TimeandRequest();
                            tnr.timeId = eachSchedule.fk_time_id;
                            tnr.isRequested = eachSchedule.isRequested;
                            tnr.isClashing = (tnr.isRequested == 1) && ((from a in allMyBusySchedule where a.fk_day_Id == i && a.fk_time_id == tnr.timeId select a).Count() > 0);
                            timeList.Add(tnr);
                        }
                        ds._timeandRequest = timeList;
                        schList.Add(ds);
                    }
                    msm._schedule = schList;                    
                
            return msm;
        }



        public MentorScheduleModel FindthisMentorshipSchedule(int mshipID)
        {

            Mentorship mship = mr.GetMentorship(mshipID);            
           
                    MentorScheduleModel msm = new MentorScheduleModel();
                    msm._mentorId = mship.fk_mentorId;
                    msm.subject = mship.fk_subject;
                    msm.subjectName = mship.subject.name;
                    msm.studentId = mship.fk_studentId;                                        
                    msm.mentorShipID = mshipID;
                    List<DaySchedule> schList = new List<DaySchedule>();

                    List<MentorSchedule> msList = mr.GetMentorSchedule(mship.fk_mentorId);//find all mentor schedule records from the mentorSchedule table 
                    for (int i = 1; i <= 7; i++)
                    {
                        DaySchedule ds = new DaySchedule();
                        ds.day = i;
                        List<TimeandRequest> timeList = new List<TimeandRequest>();
                        List<MentorSchedule> eachdayschedulelist = (from l in msList where l.fk_day_Id == i select l).ToList();
                        foreach (MentorSchedule eachSchedule in eachdayschedulelist)
                        {
                            TimeandRequest tnr = new TimeandRequest();
                            tnr.timeId = eachSchedule.fk_time_id;
                            tnr.isRequested = eachSchedule.fk_mentorship_id == mshipID ? 1 : 0;
                            timeList.Add(tnr);
                        }
                        ds._timeandRequest = timeList;
                        schList.Add(ds);
                    }
                    msm._schedule = schList;                    
                
            return msm;
        }
        public void AcceptScheduleResponse(int mid, int stuid, int subid, int mshipid)
        {
            mr.AcceptScheduleResponse(mid, stuid, subid, mshipid);
        }
        public void RejectScheduleResponse(int mid, int stuid, int subid, int mshipid)
        {
            mr.RejectScheduleResponse(mid, stuid, subid, mshipid);
        }
    }
}