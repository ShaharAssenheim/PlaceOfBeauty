using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kristin2.Models
{
    public class CustomerModel
    {

        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string LastTime { get; set; }
        public int AdminCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string ResetPasswordCode { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
    }
}