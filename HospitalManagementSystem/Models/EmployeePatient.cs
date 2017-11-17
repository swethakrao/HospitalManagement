using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models
{
    public class EmployeePatient
    {
        [Display(Name = "id")]
        public string id { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Salary")]
        public string salary { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Sex")]
        public string sex { get; set; }
        [Display(Name = "nid")]
        public string nid { get; set; }
        [Display(Name = "Contact_info")]
        public string contactinfo { get; set; }
        [Display(Name = "specialist")]
        public string specialist { get; set; }
        [Display(Name = "details")]
        public string details { get; set; }
        [Display(Name = "password")]
        public string password { get; set; }
    }
}