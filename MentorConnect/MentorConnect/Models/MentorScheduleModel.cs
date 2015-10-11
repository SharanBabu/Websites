using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MentorConnect.Models
{
    public class MentorScheduleModel
    {
        public int _mentorId{get; set;}
        public int studentId { get; set; }
        public string name { get; set; }
        public int subject { get; set; }
        public string subjectName { get; set; }
        public string description { get; set; }
        public string imgpath { get; set; }
        public string major { get; set; }
        public List<DaySchedule> _schedule { get; set; }
        public string comment { get; set; }
        public IEnumerable<SelectListItem> _subjects { get; set; }
        public int mentorShipID { get; set; }
    }
}