using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models
{
    public class upcomingApp
    {
        [Display(Name = "Patient Name")]
        public string name { get; set; }
        [Display(Name = "Sex")]
        public string sex { get; set; }
        [Display(Name = "Email")]
        public string email { get; set; }
        [Display(Name = "Date of Appointment")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime date_admitted { get; set; }
        [Display(Name = "Time of Appointment")]
        public string time { get; set; }
        [Display(Name = "Type of Appointment")]
        public string status { get; set; }
        public string pdid { get; set; }
    }
}