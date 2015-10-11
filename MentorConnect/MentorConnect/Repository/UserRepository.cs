using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MentorConnect.Models;

namespace MentorConnect.Repository
{
    public class UserRepository
    {
        DBMentorConnect context = new DBMentorConnect();
        public List<Mentor> SelectInitialMentorList()
        {
            var result = (from s in context.MentorProfiles
                          join u in context.Users on s.fk_mentor_ID equals u.ID
                          where u.subjects.Count > 0
                          select new Mentor
                          {
                              _imagePath = s.imgPath,
                              description = s.description,
                              _userName = string.Concat(u.fname, " ", u.lname != null ? u.lname : string.Empty),
                              major = s.major,
                              _id = u.ID,
                              subjects = u.subjects
                          }).Take(4);
            return result.ToList();
        }

        public List<Mentorship> GetAllMentorships(int mid)
        {
            var res = (from m in context.Mentorships where m.active == 1 && m.fk_mentorId == mid && m.status == 2 select m).ToList();
            return res;
        }

        public List<Mentorship> GetAllMentorshipsForStudent(int stuid)
        {
            var res = (from m in context.Mentorships where m.active == 1 && m.fk_studentId == stuid && m.status == 2 select m).ToList();
            return res;
        }

        public List<subject> SelectAllSubjects()
        {
            var result = (from s in context.subjects
                          select s
                          );
            return result.ToList();
        }

        public string FindSubnamefromCode(int code)
        {
            string res = (from s in context.subjects
                          where s.id == code
                          select s.name
                          ).FirstOrDefault();
            return res;
        }

        public List<Mentor> SelectMentorList(string sub)
        {
            int subInt = Convert.ToInt32(sub);
            var result = (from s in context.MentorProfiles
                          join u in context.Users on s.fk_mentor_ID equals u.ID
                          where u.subjects.Contains((from st in context.subjects
                                                     where st.id == subInt
                                                     select st).FirstOrDefault())
                          select new Mentor
                          {
                              _imagePath = s.imgPath,
                              description = s.description,
                              _userName = string.Concat(u.fname, " ", u.lname != null ? u.lname : string.Empty),
                              major = s.major,
                              _id = u.ID,
                              subjects = u.subjects
                          });
            return result.ToList();
        }

        public List<MentorSchedule> GetMentorSchedule(int mentorId)
        {
            var result = (from s in context.MentorSchedules
                          where s.fk_mentor_Id == mentorId 
                          select s);
            return result.ToList();
        }
        public List<MentorSchedule> GetMentorScheduleForMentor(int mentorId)
        {
            var result = (from s in context.MentorSchedules
                          where s.fk_mentor_Id == mentorId
                          select s);
            return result.ToList();
        }

        public Mentor GetMentorProfileDetails(int mentorId)
        {
            Mentor res = (from s in context.MentorProfiles
                          join u in context.Users on s.fk_mentor_ID equals u.ID
                          where u.ID == mentorId
                          select new Mentor
                          {
                              _imagePath = s.imgPath,
                              description = s.description,
                              _userName = string.Concat(u.fname, " ", u.lname != null ? u.lname : string.Empty),
                              major = s.major,
                              subjects = u.subjects
                          }).FirstOrDefault();
            return res;
        }
        
        public List<time> GetAllTime()
        {
            var tlist = (from t in context.times select t);
            return tlist.ToList();
        }

        public List<MentorSchedule> GetMentorScheduleBasedOnMShipID(int mshipid)
        {
            var res = (from msh in context.MentorSchedules where msh.fk_mentorship_id == mshipid select msh);
            return res.ToList();
        }

        public List<StudentSchedule> GetStudentScheduleBasedOnMShipID(int mshipid)
        {
            var res = (from msh in context.StudentSchedules where msh.fk_mentorship_id == mshipid select msh);
            return res.ToList();
        }

        public int GetIDfromEmail(string uname)
        {
            int id = (from u in context.Users
                      where u.UserID == uname
                      select u.ID).FirstOrDefault();
            return id;
        }
        public string GetNameFromID(int id)
        {
            string name = (from u in context.Users
                           where u.ID == id
                           select string.Concat(u.fname, " ", u.lname != null ? u.lname : string.Empty)).FirstOrDefault();
            return name;
        }
        public string GetStuImgPath(int id)
        {
            string name = (from u in context.StudentProfiles
                           where u.fk_student_id == id
                           select u.imgPath).FirstOrDefault();
            return name;
        }
        public int SaveMentorshipRequest(MentorScheduleModel msm)
        {
            Mentorship mship = new Mentorship();
            mship.active = 1;
            mship.fk_mentorId = msm._mentorId;
            mship.fk_studentId = msm.studentId;
            mship.fk_subject = msm.subject;
            mship.status = 1;//requested - 1,Accepted - 2,Rejected - 3
            mship.comments = msm.comment;
            context.Mentorships.Add(mship);
            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j < msm._schedule[i]._timeandRequest.Count; j++)
                {
                    MentorScheduleRequest msr = new MentorScheduleRequest();
                    msr.fk_day_Id = msm._schedule[i].day;
                    msr.fk_mentor_Id = msm._mentorId;
                    msr.fk_student_Id = msm.studentId;
                    msr.Mentorship = mship;
                    msr.isRequested = msm._schedule[i]._timeandRequest[j].isRequested;
                    msr.fk_time_id = msm._schedule[i]._timeandRequest[j].timeId;
                    context.MentorScheduleRequests.Add(msr);
                }

            }
            try
            {
                return context.SaveChanges();
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public List<StudentSchedule> GetStudentBusySchedule(int stuid)
        {
            var res = (from s in context.StudentSchedules where s.fk_student_Id == stuid && s.fk_mentorship_id != null select s);
            return res.ToList();
        }
        public List<MentorSchedule> GetMentorBusySchedule(int mid)
        {
            var res = (from s in context.MentorSchedules where s.fk_mentor_Id == mid && s.fk_mentorship_id != null select s);
            return res.ToList();
        }
        public int UpdateMentorProfile(MentorScheduleModel msm)
        {
            List<subject> sl = new List<subject>();
            subject s = (from us in context.subjects where us.id == msm.subject select us).FirstOrDefault();
            sl.Add(s);
            User u = (from us in context.Users where us.ID == msm._mentorId select us).FirstOrDefault();                        
            MentorProfile mp = (from us in context.MentorProfiles where us.fk_mentor_ID == msm._mentorId select us).FirstOrDefault();
            mp.description = msm.description;
            mp.imgPath = msm.imgpath;
            mp.major = msm.major;
            foreach(subject st in u.subjects){

                sl.Remove(st);
            }
            if(sl.Count > 0)
                u.subjects = sl;
            List<MentorSchedule> msList = (from r in context.MentorSchedules
                                           where r.fk_mentor_Id == msm._mentorId
                                           select r).ToList();
            for (int i = 0; i <= 6; i++)
            {
                for (int j = 0; j < msm._schedule[i]._timeandRequest.Count; j++)
                {
                    MentorSchedule msr = (from so in msList where so.fk_day_Id == msm._schedule[i].day & so.fk_time_id == msm._schedule[i]._timeandRequest[j].timeId select so).FirstOrDefault();                    
                    msr.isMentoring = msm._schedule[i]._timeandRequest[j].isMentorAvailable;                                        
                }

            }
            try
            {
                return context.SaveChanges();
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public List<MentorScheduleRequest> GetMentorScheduleRequests(int mshipID)
        {
            var res = (from r in context.MentorScheduleRequests
                       where r.fk_mentorship_id == mshipID
                       select r).ToList();
            return res;
        }
        public List<Mentorship> GetBriefMentorScheduleRequests(int mid)
        {
            var res = (from r in context.Mentorships
                       where r.fk_mentorId == mid & r.status == 1 & r.active == 1
                       select r).ToList();
            return res;
        }

        public void AcceptScheduleResponse(int mid, int stuid, int subid, int mshipid)
        {
            List<MentorScheduleRequest> msrList = (from r in context.MentorScheduleRequests
                                                   where r.fk_mentor_Id == mid & r.Mentorship.id == mshipid
                                                   select r).ToList();
            List<MentorSchedule> msList = (from r in context.MentorSchedules
                                           where r.fk_mentor_Id == mid
                                           select r).ToList();
            List<StudentSchedule> ssList = (from r in context.StudentSchedules
                                            where r.fk_student_Id == stuid
                                            select r).ToList();
            foreach (MentorScheduleRequest msr in msrList)
            {
                Mentorship mship = msr.Mentorship;
                mship.status = 2;//accepted                
                if (msr.isRequested == 1)
                {
                    MentorSchedule ms = (from m in msList
                                         where m.fk_day_Id == msr.fk_day_Id & m.fk_time_id == msr.fk_time_id
                                         select m).FirstOrDefault();
                    /*if(ms.fk_mentorship_id != null){
                        throw new Exception();
                    }*/
                    ms.fk_mentorship_id = msr.fk_mentorship_id;
                    StudentSchedule ss = (from m in ssList
                                          where m.fk_day_Id == msr.fk_day_Id & m.fk_time_id == msr.fk_time_id
                                          select m).FirstOrDefault();
                    ss.fk_mentorship_id = msr.fk_mentorship_id;                                       
                }
            }
            context.SaveChanges();
        }

        public void RejectScheduleResponse(int mid, int stuid, int subid, int mshipid)
        {
            List<MentorScheduleRequest> msrList = (from r in context.MentorScheduleRequests
                                                   where r.fk_mentor_Id == mid & r.Mentorship.id == mshipid
                                                   select r).ToList();
            foreach (MentorScheduleRequest msr in msrList)
            {
                Mentorship mship = msr.Mentorship;
                mship.status = 3;//rejected                             
            }
            context.SaveChanges();
        }

        public Mentorship GetMentorship(int mentorshipId)
        {
            return context.Mentorships.Where(u => u.id == mentorshipId).FirstOrDefault();
        }
    }
}