using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MentorConnect.Models
{
    public class Mentor
    {
        public string _imagePath { get; set; }
        public int _id { get; set; }
        public string _userName { get; set; }
        public string major { get; set; }
        public ICollection<subject> subjects { get; set; }
        public string description { get; set; }

        public string ListSubjects {
            get { 
                string list = "";
                foreach(subject sub in subjects)
                {
                    list += sub.name + ", ";
                }
                return list.TrimEnd(' ').TrimEnd(',');
            }
        }
    }
}