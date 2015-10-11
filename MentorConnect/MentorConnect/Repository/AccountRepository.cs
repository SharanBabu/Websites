using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MentorConnect.Models;
namespace MentorConnect.Repository
{    
    public class AccountRepository
    {
        DBMentorConnect context = new DBMentorConnect();
        public User GetUserByUserNamePassword(string user, string pass, bool isMentor)
        {
            int userType = isMentor?1:2;
            return context.Users.Where(u=> u.UserID==user & u.Password == pass & u.user_type == userType).FirstOrDefault();
        }

        public void RegisterNewUser(RegisterModel regMod)
        {
            User user = new User();
            user.fname = regMod.FirstName;
            user.lname = regMod.LastName;
            user.UserID = regMod.UserName;
            user.Password = regMod.Password;
            user.addr1 = regMod.Address1;
            user.addr2 = regMod.Address2;
            user.city = regMod.City;
            user.state = regMod.State;
            user.zip = regMod.ZipCode;
            user.phNo = regMod.PhoneNumber;
            user.user_type = regMod.EnrollAs;
            
            context.Users.Add(user);
            if (regMod.EnrollAs == 1)
            {
                MentorProfile mp = new MentorProfile();
                mp.description = null;
                mp.imgPath = "/Uploads/UserImage/m1.jpeg";
                mp.major = null;
                mp.User = user;
                context.MentorProfiles.Add(mp);

                List<weekday> weekdays = context.weekdays.Select(w => w).ToList();
                List<time> times = context.times.Select(w => w).ToList();
                foreach (weekday w in weekdays)
                {
                    foreach (time t in times)
                    {
                        MentorSchedule ms = new MentorSchedule();
                        ms.fk_day_Id = w.dayid;
                        ms.fk_time_id = t.timeid;
                        ms.MentorProfile = mp;
                        ms.fk_mentorship_id = null;
                        ms.isMentoring = 0;
                        context.MentorSchedules.Add(ms);
                    }
                }
            }
            else if (regMod.EnrollAs == 2)
            {
                StudentProfile mp = new StudentProfile();
                mp.description = null;
                mp.imgPath = "/Uploads/UserImage/m2.jpeg";
                mp.clas = null;
                mp.User = user;
                context.StudentProfiles.Add(mp);
                List<weekday> weekdays = context.weekdays.Select(w => w).ToList();
                List<time> times = context.times.Select(w => w).ToList();
                foreach (weekday w in weekdays)
                {
                    foreach (time t in times)
                    {
                        StudentSchedule ms = new StudentSchedule();
                        ms.fk_day_Id = w.dayid;
                        ms.fk_time_id = t.timeid;
                        ms.StudentProfile = mp;
                        ms.fk_mentorship_id = null;                        
                        context.StudentSchedules.Add(ms);
                    }
                }
            }
            context.SaveChanges();
        }

        public User GetUserByEmail(string email, int usertype)
        {
            return context.Users.Where(u => u.UserID == email && u.user_type == usertype).FirstOrDefault();
        }

        public User GetCurrentUser()
        {
            String email = System.Web.HttpContext.Current.Session["username"] as String;
            int utype = (int)System.Web.HttpContext.Current.Session["usertype"];
            //String email = "cchardinecheradee@utdallas.edu";    // hardcode for now
            return GetUserByEmail(email, utype);
        }

        public void ChangeCurrentUserAddress(string addr1, string addr2, string city, string state, string zip)
        {
            User user = GetCurrentUser();
            user.addr1 = addr1;
            user.addr2 = addr2;
            user.city = city;
            user.state = state;
            user.zip = zip;

            context.SaveChanges();
        }

        public void ChangeCurrentUserPhone(string phone)
        {
            User user = GetCurrentUser();
            user.phNo = phone;

            context.SaveChanges();
        }

        public void ChangeCurrentUserName(string fname, string lname)
        {
            User user = GetCurrentUser();
            user.fname = fname;
            user.lname = lname;

            context.SaveChanges();
        }

        public void ChangeCurrentUserEmail(string email)
        {
            User user = GetCurrentUser();
            user.UserID = email;

            context.SaveChanges();
        }

        public void ChangeCurrentUserPassword(string oldPw, string newPw)
        {
            if (newPw != "" && oldPw != "")
            {
                User user = GetCurrentUser();

                if (user.Password.Equals(oldPw))
                {
                    user.Password = newPw;

                    context.SaveChanges();
                }
            }
        }

        public void ChangeMentorImage(User user, string path, string type)
        {
            UserImage img = context.UserImages.Where(u => u.fk_userId == user.ID).FirstOrDefault();
            
            if (img != null)    // edit
            {
                img.imagePath = path;
                img.contentType = type;
            }
            else     // create new
            {
                img = new UserImage();
                img.fk_userId = user.ID;
                img.imagePath = path;
                img.contentType = type;
                user.MentorProfile.imgPath = path;
                context.UserImages.Add(img);
            }

            context.SaveChanges();
        }
        public void ChangeStudentImage(User user, string path, string type)
        {
            UserImage img = context.UserImages.Where(u => u.fk_userId == user.ID).FirstOrDefault();

            if (img != null)    // edit
            {
                img.imagePath = path;
                img.contentType = type;
            }
            else     // create new
            {
                img = new UserImage();
                img.fk_userId = user.ID;
                img.imagePath = path;
                img.contentType = type;
                user.StudentProfile.imgPath = path;
                context.UserImages.Add(img);
            }

            context.SaveChanges();
        }
        public void ChangeUserImage(string path, string type)
        {
            User user = GetCurrentUser();
            if(user.user_type == 1){
                ChangeMentorImage(user, path, type);
            }
            else
            {
                ChangeStudentImage(user, path, type);
            }
        }

        public UserImage GetCurrentUserImage()
        {
            User user = GetCurrentUser();

            return context.UserImages.Where(u => u.fk_userId == user.ID).FirstOrDefault();
        }

        public Boolean IsCurrentUserMentor()
        {
            User user = GetCurrentUser();
            if (user.user_type == 1)            // 1 is mentor, 2 is student
            {
                return true;
            }

            return false;
        }

        public Mentorship GetMentorship(int mentorshipId)
        {
            return context.Mentorships.Where(u => u.id == mentorshipId).FirstOrDefault();
        }

        public void AddReview(int mentorshipId, double rating, String comments)
        {
            Review review = context.Reviews.Where(u => u.fk_mentorshipId == mentorshipId).FirstOrDefault();
            // add new review if no review yet
            if (review == null)
            {
                review = new Review();
                review.comments = comments;
                review.fk_mentorshipId = mentorshipId;
                review.rating = rating;

                context.Reviews.Add(review);
            }
            else
            // otherwise, edit existing review
            {
                review.comments = comments;
                review.rating = rating;
            }

            context.SaveChanges();
        }
    }
}