using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MentorConnect.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Please Enter first name")]
        [Display(Name = "First Name")]        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        [Required]
        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }        
        [Required]
        public int EnrollAs{ get; set; }
       
        [Required]
        [RegularExpression(".+@utdallas\\.edu", ErrorMessage = "Please Enter Correct Email Address")]
        public string UserName { get; set; }
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm not matched.")]
        public string ConfirmPassword { get; set; }
    }
}