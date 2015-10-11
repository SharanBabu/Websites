using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MentorConnect.Models
{
    public class MentorshipRequest
    {
        public int _mentorId { get; set; }
        public int studentId { get; set; }
        public string stuname { get; set; }
        public int subject { get; set; }
        public string subject_name { get; set; }
        public int mentorshipID { get; set; }
    }
}