using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MentorConnect.Models
{
    public class ManageAccountViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [Required]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [RegularExpression(@"(^(?!0{5})(\d{5})(?!-?0{4})(-?\d{4})?$)", ErrorMessage = "Improperly formatted zip code.  It must be entered as nnnnn or nnnnn-nnnnn.")]
        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /*
        [DataType(DataType.Password)]
        public String Password { get; set; }
         * */

        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        [ConfirmPassword(ErrorMessage = "Current Password is incorrect.")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [FileSize(5000)]
        [FileTypes("jpg,jpeg,png")]
        public HttpPostedFileBase ImageFile { get; set; }

        public String ImageFilename { get; set; }

        [FileSize(5000)]
        [FileTypes("pdf")]
        public HttpPostedFileBase PdfFile { get; set; }

        public String PdfFilename { get; set; }

        public Boolean IsMentor { get; set; }
    }

    public class ConfirmPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string entPw = value as String;
                Repository.AccountRepository userRep = new Repository.AccountRepository();
                User user = userRep.GetCurrentUser();

                if (user.Password.Equals(entPw))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            return true;
        }
    }

    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;      // in kB

        public FileSizeAttribute (int maxSize)
        {
            _maxSize = maxSize ;     
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            return (value as HttpPostedFileBase).ContentLength <= _maxSize * 1024;      // ContentLength is in bytes
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("The file size should not exceed {0} kB", _maxSize);
        }
    }

    public class FileTypesAttribute : ValidationAttribute
    {
        private readonly List<string> _types;

        public FileTypesAttribute (string types)
        {
            _types = types.Split(',').ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var fileExt = System.IO.Path.GetExtension((value as HttpPostedFileBase).FileName).Substring(1);
            return _types.Contains(fileExt, StringComparer.OrdinalIgnoreCase);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Invalid file type. Only the following types {0} are supported.", String.Join(", ", _types));
        }
    }
}