using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MentorConnect.Models;
using MentorConnect.Repository;

namespace MentorConnect.Core
{
    public class CoreStudentHome
    {
        UserRepository mr = new UserRepository();

        public VideoConference GetVideoConference(int mshipID)
        {
            VideoConference res = new VideoConference();
            res.isMentor = false;
            Mentorship mship = mr.GetMentorship(mshipID);
            int mid = mship.fk_mentorId;
            int sid = mship.fk_studentId;

            res.mname = mr.GetNameFromID(mid);
            res.sname = mr.GetNameFromID(sid);
            res.mshipID = mshipID;

            return res;
        }

        public MentorList GetInitMentorList()
        {
            List<Mentor> _list = mr.SelectInitialMentorList();
            MentorList res = new MentorList();
            MentorSearchResults ms = new MentorSearchResults();
            ms._mentorSearchResults = _list;
            res._mentorList = ms;
            return res;
        }

        public SubjectList GetAllSubjects()
        {
            List<subject> _list = mr.SelectAllSubjects();
            SubjectList s = new SubjectList();
            List<subjectModel> sml = new List<subjectModel>();
            foreach(subject sub in _list){
                subjectModel sm = new subjectModel();
                sm.id = sub.id;
                sm.name = sub.name;
                sm.description = sub.description;
                sml.Add(sm);
            }
            s._subList = sml;
            return s;
        }

        public MentorSearchResults FindMentors(string subject)
        {
            List<Mentor> _list = mr.SelectMentorList(subject);
            MentorList res = new MentorList();
            MentorSearchResults msr = new MentorSearchResults();
            msr._mentorSearchResults = _list;
            return msr;
        }

        public MentorScheduleModel FindMentorSchedule(string username, int mentorId)
        {
            List<MentorSchedule> scheduleList = mr.GetMentorSchedule(mentorId);
            Mentor mp = mr.GetMentorProfileDetails(mentorId);
            MentorScheduleModel msm = new MentorScheduleModel();
            msm._mentorId = mentorId;
            msm.studentId = mr.GetIDfromEmail(username);
            msm.imgpath = mp._imagePath;
            msm.major = mp.major;
            msm.description = mp.description;
            msm.name = mp._userName;
            msm.subject = mp.subjects.ToList().First().id;
            msm.subjectName = mp.subjects.ToList().First().name;
            List<DaySchedule> schList = new List<DaySchedule>();
            List<StudentSchedule> allMyBusySchedule = mr.GetStudentBusySchedule(msm.studentId);
            for (int i = 1; i <= 7; i++ )
            {
                DaySchedule ds = new DaySchedule();
                ds.day = i;
                List<TimeandRequest> timeList = new List<TimeandRequest>();
                List<MentorSchedule> eachdayschedulelist = (from l in scheduleList where l.fk_day_Id == i select l).ToList();
                foreach(MentorSchedule eachSchedule in eachdayschedulelist){
                    TimeandRequest tnr = new TimeandRequest();
                    tnr.timeId = eachSchedule.fk_time_id;
                    tnr.isMentorAvailable = (eachSchedule.isMentoring==1 && eachSchedule.Mentorship == null)? 1 : 0;
                    tnr.isFuture = IsFuture(i,tnr.timeId);
                    tnr.isClashing = (tnr.isMentorAvailable == 1) && ((from a in allMyBusySchedule where a.fk_day_Id == i && a.fk_time_id == tnr.timeId select a).Count() > 0);
                    timeList.Add(tnr);
                }
                ds._timeandRequest = timeList;
                schList.Add(ds);
            }
            msm._schedule = schList;
            return msm;
        }
        public int SubmitScheduleRequest(MentorScheduleModel msm){
            return mr.SaveMentorshipRequest(msm);
        }

        public MentorshipList GetAllMentorships(string stuName)
        {
            int stuID = mr.GetIDfromEmail(stuName);
            List<Mentorship> mshipsFromDB = mr.GetAllMentorshipsForStudent(stuID);
            //form the mentorshiplist object for UI
            List<MentorshipModel> _allList = new List<MentorshipModel>();
            foreach (Mentorship m in mshipsFromDB)
            {
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
            result.isMentor = false;
            result._allMShips = _allList;
            return result;
        }

        private bool IsFuture(int dayID, int timeID)
        {
            int currentDayID = GetCurrentDayID();
            int currentTimeID = GetCurrentTimeID();
            if ((currentDayID < dayID) || (currentDayID == dayID && currentTimeID < timeID))
            {
                return true;
            }
            return false;
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
            List<StudentSchedule> allMSch = mr.GetStudentScheduleBasedOnMShipID(mshipid);
            foreach (StudentSchedule m in allMSch)
            {
                if (currentDayID == m.fk_day_Id && currentTimeID == m.fk_time_id)
                {
                    return true;
                }
            }
            return false;
        }
        private int GetCurrentDayID()
        {
            string day = DateTime.Now.DayOfWeek.ToString();
            int dayid = 0;
            switch (day)
            {
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
            TimeSpan hoursTime = new TimeSpan(ts.Hours, 0, 0);
            foreach (time t in allTime)
            {
                if (hoursTime == t.starttime)
                {
                    return t.timeid;
                }                
            }
            if(hoursTime > new TimeSpan(22,0,0)){
                    return 14;
            }
            return 0;
        }

        public Mentorship GetMentorship(int id)
        {
            return mr.GetMentorship(id);
        }
    }
}