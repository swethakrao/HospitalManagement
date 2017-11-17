using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalManagementSystem.Models
{
    public class Treatment
    {
        [Display(Name = "UserID")]
        public string pdid { get; set; }
        [Display(Name = "Patient Name")]
        public string name { get; set; }
        [Display(Name = "Diagnosis")]
        public string diagnosis { get; set; }
        [Display(Name = "Scan")]
        public string scan { get; set; }
        [Display(Name = "Medicine[Med-name(dosage)]")]
        public string medicine { get; set; }
        [Display(Name = "Estimated Recovery Time")]
        public string ert { get; set; }
    }
}