using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MentorConnect.Models
{
    public class VideoConference
    {
        public int mshipID {get; set;}
        public bool isMentor { get; set; }
        public string mname { get; set; }
        public string sname { get; set; }
        public int mid { get; set; }
        public int sid { get; set; }
    }
}