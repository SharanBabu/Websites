using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MentorConnect.Models
{
    public class WriteReviewViewModel
    {
        public int MentorshipId { get; set; }
        public String ImageFilename { get; set; }
        public String MentorName { get; set; }
        public String Email { get; set; }
        public String Subject { get; set; }
        // public String Date { get; set; }

        [Display(Name = "Rating")]
        [Range(1,4, ErrorMessage="Please select a rating.")]
        public int Rating { get; set; }

        [Required]
        [Display (Name="Comments")]
        public String Comment { get; set; }
    }
}