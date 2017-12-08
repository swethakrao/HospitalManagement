using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
// Author: Swetha KrishnamurthyRao
// UBID: 1004265
// Login credentials Model.
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