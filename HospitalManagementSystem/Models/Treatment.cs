using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//Author: Swetha KrishnamurthyRao
// UBID: 1004265
// Model for Treatment Table
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
        [Display(Name = "Doctor's Name")]
        public string d_name { get; set; }
    }
}