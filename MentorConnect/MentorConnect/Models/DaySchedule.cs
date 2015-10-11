using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MentorConnect.Models
{
    public class DaySchedule
    {
        public int day { get; set; }
        public List<TimeandRequest> _timeandRequest { get; set; }
    }
}