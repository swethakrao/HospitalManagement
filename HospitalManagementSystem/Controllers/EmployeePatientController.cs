using HospitalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

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
        [Route("api/ep")]
        public IHttpActionResult Put(EmployeePatient ep)
        {
            BusinessLayer blayer = new BusinessLayer();
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");
            

                if (blayer.updatePersonalDetails(ep) != null)
                {
                return Ok();
                }
               
            return NotFound();
        }
        [Route("api/viewappointment")]
        public IHttpActionResult GetAllAppointment()
        {
            BusinessLayer blayer = new BusinessLayer();
            IList<BookAnAppointment> balist = new List<BookAnAppointment>();
            BookAnAppointment bookapp = null;
            string pdetails = "PD" + (HttpContext.Current.Application["Username"].ToString()).Substring(1, (HttpContext.Current.Application["Username"].ToString()).Length - 1);
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
        [Route("api/info")]
        public IHttpActionResult GetAllPatientsEmployee()
        {
            BusinessLayer blayer = new BusinessLayer();
            IList<EmployeePatient> eplist = new List<EmployeePatient>();
            EmployeePatient employee_patient = null;
            if (HttpContext.Current.Application["Username"] != null && HttpContext.Current.Application["Category"] != null)
            {
                System.Data.DataTable dt = blayer.getAllInformation(HttpContext.Current.Application["Username"].ToString(), HttpContext.Current.Application["Category"].ToString());
                if (HttpContext.Current.Application["Category"].Equals("Patient"))
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

