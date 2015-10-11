//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MentorConnect.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mentorship
    {
        public Mentorship()
        {
            this.MentorSchedules = new HashSet<MentorSchedule>();
            this.StudentSchedules = new HashSet<StudentSchedule>();
            this.MentorScheduleRequests = new HashSet<MentorScheduleRequest>();
        }
    
        public int id { get; set; }
        public int fk_mentorId { get; set; }
        public int fk_studentId { get; set; }
        public int active { get; set; }
        public int fk_subject { get; set; }
        public int status { get; set; }
        public string comments { get; set; }
    
        public virtual MentorProfile MentorProfile { get; set; }
        public virtual ICollection<MentorSchedule> MentorSchedules { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<StudentSchedule> StudentSchedules { get; set; }
        public virtual Review Review { get; set; }
        public virtual subject subject { get; set; }
        public virtual ICollection<MentorScheduleRequest> MentorScheduleRequests { get; set; }
    }
}