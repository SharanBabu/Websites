//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MentorConnect.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserImage
    {
        public int fk_userId { get; set; }
        public string imagePath { get; set; }
        public string contentType { get; set; }
    
        public virtual User User { get; set; }
    }
}
