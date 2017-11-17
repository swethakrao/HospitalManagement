using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalManagementSystem.Models
{
    public class BookAnAppointment
    {
        [Display(Name = "UserID")]
        public string pdid { get; set; }
        [Display(Name = "Location")]
        public string location { get; set; }
        [Display(Name = "Date Of Appointment(mm/dd/yyyy)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? date_admitted { get; set; }
        [Display(Name = "Date Of Discharge")]
        public string date_discharged { get; set; }
        [Display(Name= "Type of Appointment")]
        public string status { get; set; }
        [Display (Name = "Doctor ID")]
        public string doctor_id { get; set; }
        [Display (Name = "Time")]
        public string admit_time { get; set; }
        [Display(Name = "Doctor")]
        public string doctor { get; set; }
        [Display(Name = "Time of Discharge")]
        public string discharge_time { get; set; }
        public string PDetails { get; set; }
        public string month { get; set; }
        public string day { get; set; }
        public string year { get; set; }
        public IEnumerable<SelectListItem> appointmentType { get; set; }
        public IEnumerable<SelectListItem> doctorItem { get; set; }
        public IEnumerable<SelectListItem> docterId { get; set; }

    }
}