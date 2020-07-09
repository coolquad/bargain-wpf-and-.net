using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bargain.Models
{
    public class MemberModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string Password { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
    }


}