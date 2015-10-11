using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MentorConnect.Models
{
    public class TimeandRequest
    {
        public int timeId { get; set; }
        public int isMentorAvailable { get; set; }
        public int isRequested { get; set; }
        public bool isFuture{get; set;}
        public bool isClashing { get; set; }
    }
}