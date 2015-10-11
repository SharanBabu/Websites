using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MentorConnect.Models
{
    public class MentorshipModel
    {
        public string mentor { get; set; }
        public string student { get; set; }
        public int mentorID { get; set; }
        public int studentID { get; set; }
        public string subject { get; set; }
        public int subjectID { get; set; }
        public bool isPast { get; set; }
        public bool isFuture { get; set; }
        public bool isCurrentlyActive { get; set; }
        public int mentorshipID { get; set; }
    }
}