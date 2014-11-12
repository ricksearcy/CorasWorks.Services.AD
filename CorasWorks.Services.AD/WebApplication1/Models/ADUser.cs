using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorasWorks.Services.AD.Models
{
    public class ADUser
    {
        public string FirstName { get; set; }       
        public string LastName { get; set; }      
        public string Email { get; set; }      
        public string UserName { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }       
        public string Company { get; set; }       
        
    }
    // public List<Groups> { get; set; }
    // public string Url { get; set; }
    // public string MiddleInitialpublic { get; set; }
    //public string DistinguishedName { get; set; }
    //public bool IsAccountActive { get; set; }
    // public string DisplayName { get; set; }
    // public string UserPrincipalName { get; set; }
    // public string PostalAddress { get; set; }
    // public string MailingAddress { get; set; }
    ////public string ResidentialAddress { get; set; }
    //public string Title { get; set; }
    //public string HomePhone { get; set; }
    //public string OfficePhone { get; set; }
    //public string Mobile { get; set; }
    //public string Fax { get; set; }
}