using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models
{
    public class Value
    {
        [Display(Name = "Username")]
        public string usrname { get; set; }
        [Display(Name = "Password")]
        public string password { get; set; }
        
        public string category { get; set; }
    }
}