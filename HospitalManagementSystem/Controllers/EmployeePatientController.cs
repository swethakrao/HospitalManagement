using HospitalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

//Author: Swetha KrishnamurthyRao
// UBID: 1004265
//Web API Controller 
namespace HospitalManagementSystem.Controllers
{
    public class EmployeePatientController : ApiController
    {
        public IHttpActionResult PostBookAppointment(BookAnAppointment appointment)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

           return Ok();
        }
        // Edit appointment details
        [Route("api/editappointment")]
        public IHttpActionResult Put(BookAnAppointment ba)
        {
            BusinessLayer blayer = new BusinessLayer();
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");


            if (blayer.updateAppointment(ba)!= null)
            {
                return Ok();
            }

            return NotFound();
        }
        //Updates the edited personal details to the Database
        [Route("api/ep")]
        public IHttpActionResult Put(EmployeePatient ep)
        {
            if(ep !=null){
                BusinessLayer blayer = new BusinessLayer();
                if (!ModelState.IsValid)
                    return BadRequest("Not a valid data");
                string uname = ep.epusername;
                string cat = ep.epcategory;
                if (blayer.updatePersonalDetails(ep, uname, cat) != null)
                {
                    return Ok();
                }
            }
            return NotFound();
        }
        // View Appointment for Patient's booked appointments
        [Route("api/viewappointment")]
        public IHttpActionResult GetAllAppointment(string id)
        {
            BusinessLayer blayer = new BusinessLayer();
            IList<BookAnAppointment> balist = new List<BookAnAppointment>();
            BookAnAppointment bookapp = null;
            string pdetails = "PD" + (id.Trim()).Substring(1, id.Trim().Length - 1);
            System.Data.DataTable dt = blayer.getAllAppointment(pdetails);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bookapp = new BookAnAppointment();
                    bookapp.pdid = row["PDID"].ToString();
                    bookapp.date_admitted = (DateTime)row["date_admitted"];
                    bookapp.status = row["status"].ToString();
                    bookapp.doctor_id = row["doctor_id"].ToString();
                    bookapp.doctor = blayer.getDoctorName(bookapp.doctor_id);
                    bookapp.admit_time = row["admit_time"].ToString();
                    bookapp.location = row["location"].ToString();
                    bookapp.PDetails = pdetails;
                    balist.Add(bookapp);
                }
            }
                if (balist.Count == 0)
                {
                    return NotFound();
                }

                return Ok(balist);
            }
        //Cancels an appointment
        [Route("api/va")]
        public IHttpActionResult Delete(string id)
        {
            BusinessLayer bl = new BusinessLayer();
            if (Int32.Parse(id) <= 0)
                return BadRequest("Not able to cancel appointment");
            if (!(bl.deleteAppointment(id) < 1))
            {
                return Ok();
            }

            else
            {
                return NotFound();
            }
        }
        // Display all Personal details
        [Route("api/info")]
        public IHttpActionResult GetAllPatientsEmployee(string id)
        {
            BusinessLayer blayer = new BusinessLayer();
            IList<EmployeePatient> eplist = new List<EmployeePatient>();
            EmployeePatient employee_patient = null;
            string uname = id.Split(':')[0];
            string cat = id.Split(':')[1];
            if (uname != null && cat != null)
            {
                System.Data.DataTable dt = blayer.getAllInformation(uname, cat);
                if (cat.Equals("Patient"))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        employee_patient = new EmployeePatient();
                        employee_patient.id = row["PID"].ToString();
                        employee_patient.name = row["Name"].ToString();
                        employee_patient.sex = row["Sex"].ToString();
                        employee_patient.address = row["Address"].ToString();
                        employee_patient.contactinfo = row["contact_no"].ToString();
                        eplist.Add(employee_patient);

                    }
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        employee_patient = new EmployeePatient();
                        employee_patient.id = row["EID"].ToString();
                        employee_patient.name = row["E_Name"].ToString();
                        employee_patient.salary = row["Salary"].ToString();
                        employee_patient.address = row["E_address"].ToString();
                        employee_patient.sex = row["sex"].ToString();
                        employee_patient.nid = row["NID"].ToString();
                        employee_patient.contactinfo = row["contact_info"].ToString();
                        employee_patient.specialist = row["specialist"].ToString();
                        eplist.Add(employee_patient);
                    }

                    if (eplist.Count == 0)
                    {
                        return NotFound();
                    }

                    return Ok(eplist);
                }
            }

                if (eplist.Count == 0)
                {
                    return NotFound();
                }

                return Ok(eplist);
            }
        }
    }

