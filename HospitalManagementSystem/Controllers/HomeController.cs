using HospitalManagementSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HospitalManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        BusinessLayer blayer = new BusinessLayer();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult success()
        {
            return View();
        }
        public ActionResult logout()
        {
            HttpContext.Application["Username"] = null;
            return RedirectToAction("login");
        }

        [HttpGet]
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(Value v)
        {
            if(blayer.authentication(v) != null)
            {
                HttpContext.Application["Username"] = blayer.authentication(v);
                //Session["username"] = blayer.authentication(v);
                //Session["category"] = v.category;
                HttpContext.Application["Category"] = v.category;
                
                ViewBag.Message = "Correct credentials";
                return RedirectToAction("success");

            }
            else
            {
                ViewBag.Message = "Wrong credentials.";
            }
            return View();
        }
        public ActionResult edit()
        {
            EmployeePatient ep = null;
            System.Data.DataTable dt = blayer.getAllInformation(HttpContext.Application["Username"].ToString(), HttpContext.Application["Category"].ToString());
            foreach(DataRow row in dt.Rows)
            {
                if (HttpContext.Application["Category"].ToString().Equals("Patient"))
                {
                    ep = new EmployeePatient();
                    ep.id = row["PID"].ToString();
                    ep.name = row["Name"].ToString();
                    ep.sex = row["Sex"].ToString();
                    ep.address = row["Address"].ToString();
                    ep.contactinfo = row["contact_no"].ToString();
                }
                else if(HttpContext.Application["Category"].ToString().Equals("Employee"))
                {
                    ep = new EmployeePatient();
                    ep.id = row["EID"].ToString();
                    ep.name = row["E_Name"].ToString();
                    ep.sex = row["sex"].ToString();
                    ep.address = row["E_address"].ToString();
                    ep.contactinfo = row["contact_info"].ToString();
                }
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57394/api/");
                //HTTP GET
                var responseTask = client.GetAsync("ep?id=" + HttpContext.Application["Username"].ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<EmployeePatient>();
                    readTask.Wait();

                    ep = readTask.Result;
                }
            }

            return View(ep);
        }
        [HttpPost]
        public ActionResult Edit(EmployeePatient ep)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:57394/api/ep");

                    //HTTP POST
                    var putTask = client.PutAsJsonAsync<EmployeePatient>("ep", ep);
                    putTask.Wait();


                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        return RedirectToAction("PersonalDetails");
                    }
                }
            }
            catch(AggregateException ae)
            {
                ViewBag.Message = ae.Message;
            }
            return View(ep);
        }
    


    public ActionResult bookapp()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Checkup", Value = "0", Selected = true });

            items.Add(new SelectListItem { Text = "Inpatient", Value = "1" });

            items.Add(new SelectListItem { Text = "Outpatient Checkup", Value = "2" });

            ViewBag.appointmentType = new List<SelectListItem>();
            ViewBag.appointmentType = items;

            List<SelectListItem> docList = new List<SelectListItem>();
            List<SelectListItem> docid = new List<SelectListItem>();
            string locations = HttpContext.Application["location"].ToString();
            System.Data.DataTable dt = blayer.getAllDoctors(locations);
            int count = 0;
            foreach(DataRow row in dt.Rows)
            {
                docList.Add(new SelectListItem { Text = row["E_Name"].ToString() + ", " + row["specialist"].ToString(), Value = count.ToString()});
                docid.Add(new SelectListItem { Text = row["EID"].ToString(), Value = count.ToString()});
                count++;
            }
            ViewBag.doctor = new List<SelectListItem>();
            ViewBag.doctor = docList;
            ViewBag.docid = new List<SelectListItem>();
            ViewBag.docid = docid;
            ViewBag.location = new List<SelectListItem>();
            return View();
        }
      [HttpPost]
        public ActionResult bookapp(BookAnAppointment ba)
        {
            int count = 0;
            Random r_no = new Random();
            string locations = HttpContext.Application["location"].ToString();
            System.Data.DataTable dt = blayer.getAllDoctors(locations);
            if (ba.status.Equals("0"))
            {
                ba.status = "Checkup";

            }
            else if (ba.status.Equals("1"))
            {
                ba.status = "Inpatient";
            }
            else
            {
                ba.status = "Outpatient Checkup";
            }
            foreach(DataRow row in dt.Rows)
            {
                               
                if (Int32.Parse(ba.doctor) == count) 
                {
                    
                    ba.doctor_id = row["EID"].ToString();
                    ba.PDetails = "PD"+(HttpContext.Application["Username"].ToString()).Substring(1, HttpContext.Application["Username"].ToString().Length - 1);
                    ba.pdid = (r_no.Next(1000, 9999)).ToString();
                    
                }
                count++;
            }

            ba.location = HttpContext.Application["location"].ToString();
            string email_id = blayer.insertAppointment(ba);
            if(email_id.Length > 5)
            {
                try
                {
                    WebMail.SmtpServer = "smtp.gmail.com";
                    //gmail port to send emails  
                    WebMail.SmtpPort = 587;
                    WebMail.SmtpUseDefaultCredentials = true;
                    //sending emails with secure protocol  
                    WebMail.EnableSsl = true;
                    //EmailId used to send emails from application  
                    WebMail.UserName = "kraoswetha2017@gmail.com";
                    WebMail.Password = "Krishna1409";

                    //Sender email address.  
                    WebMail.From = "kraoswetha2017@gmail.com";

                    //Send email  
                    WebMail.Send(to: email_id, subject: "Doctor Appointment Confirmation", body: "<p>Hello,</p><br/><p>This is to confirm your appointment for " + ba.appointmentType + "on " + ((DateTime)ba.date_admitted).ToString("MM/dd/yyyy") + "at " + ba.admit_time + "(24 hours ET)</p>" , isBodyHtml: true);
                }
                catch(Exception e)
                {
                    ViewBag.Message = "Check the entered details.";
                }
            }

            return RedirectToAction("SuccessAppointment");
        }
       public ActionResult locations()
        {

            return View();
        }
        public ActionResult PersonalDetails()
        {
            IEnumerable<EmployeePatient> info_list = null;
                   
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:57394/api/");
                    //HTTP GET
                    var responseTask = client.GetAsync("info");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<EmployeePatient>>();
                        readTask.Wait();

                        info_list = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        info_list = Enumerable.Empty<EmployeePatient>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }
                return View(info_list);  
        }

        public ActionResult bookbasedlocation()
        {
            System.Data.DataTable dt =  blayer.getAllBranchLocations();
            List<SelectListItem> ls = new List<SelectListItem>();
            int count = 0;
            foreach(DataRow row in dt.Rows)
            {
                
                ls.Add(new SelectListItem { Text = row["Name"].ToString(), Value = count.ToString() });
                count++;
            }
            ViewBag.loca = ls;
            return View();
        }

        [HttpPost]
        public ActionResult bookbasedlocation(BookAnAppointment ba)
        {
            string loc = null;
            if(ba.location.Length != 0)
            {
                switch (ba.location)
                {
                    case "0":
                        loc = "Boston";
                        break;
                    case "1":
                        loc = "Bridgeport";
                        break;
                    case "2":
                        loc = "Michigan";
                        break;
                    case "3":
                        loc = "San Francisco";
                        break;
                    case "4":
                        loc = "Texas";
                        break;
                }

                HttpContext.Application["location"] = loc;
            }

            return RedirectToAction("bookapp");
        }

        public ActionResult SuccessAppointment()
        {
            return View();
        }

        public ActionResult ViewAppointment()
        {
            IEnumerable<BookAnAppointment> info_list = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57394/api/");
                //HTTP GET
                var responseTask = client.GetAsync("viewappointment");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<BookAnAppointment>>();
                    readTask.Wait();

                    info_list = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    info_list = Enumerable.Empty<BookAnAppointment>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(info_list);
        }

        public ActionResult edita(string id, DateTime date_admitted, string status, string doctor, string admittime, string location, string doctor_id)
        {
            BookAnAppointment book = new BookAnAppointment();
            book.pdid = id;
            book.date_admitted = date_admitted;
            book.month = date_admitted.Month.ToString();
            book.day = date_admitted.Day.ToString();
            book.year = date_admitted.Year.ToString();
            book.status = status;
            book.doctor = doctor;
            book.admit_time = admittime;
            book.location = location;
            book.doctor_id = doctor_id;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57394/api/");
                //HTTP GET
                var responseTask = client.GetAsync("editappointment?id=" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<BookAnAppointment>();
                    readTask.Wait();

                    book = readTask.Result;
                }
            }

            return View(book);
        }
        [HttpPost]
        public ActionResult edita(BookAnAppointment ba)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:57394/api/editappointment");

                    //HTTP POST
                    var putTask = client.PutAsJsonAsync<BookAnAppointment>("editappointment", ba);
                    putTask.Wait();


                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        return RedirectToAction("ViewAppointment");
                    }
                }
            }
            catch (AggregateException ae)
            {
                ViewBag.Message = ae.Message;
            }
            return View(ba);
        }

        public ActionResult cancel(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57394/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("va?id=" + id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("ViewAppointment");
                }
            }

            return RedirectToAction("ViewAppointment");
        }
        public ActionResult upcomingAppointment()
        {
            IList<upcomingApp> up = new List<upcomingApp>();
            upcomingApp upapp = null;
            System.Data.DataTable dt = null;
            string doctor_id = HttpContext.Application["Username"].ToString();
            dt = blayer.getAllPatientDetails(doctor_id);
            if(dt == null)
            {
                ViewBag.Message = "There are no appointments for you.";
                return View();
            }
            foreach(DataRow row in dt.Rows)
            {
                upapp = new upcomingApp();
                upapp.date_admitted = (DateTime)row["date_admitted"];
                if (upapp.date_admitted < DateTime.Now)
                {
                    continue;
                }
                upapp.name = row["Name"].ToString();
                upapp.sex = row["Sex"].ToString();
                upapp.email = row["email"].ToString();
                upapp.time = row["admit_time"].ToString();
                upapp.status = row["status"].ToString();
                upapp.pdid = row["pdid"].ToString();
                up.Add(upapp);
            }

            return View(up);
        }
        public ActionResult cancel_appointment(string id)
        {
            int res = blayer.deleteAppointment(id);
            if (res < 1)
            {
                ViewBag.Message = "Unable to cancel appointment. Try again later.";
            }
            return RedirectToAction("upcomingAppointment");
        }
        [HttpPost]
        public ActionResult AddTreatment(FormCollection frm)
        {
            Treatment treat = new Treatment();
            treat.pdid = (Request.Form[0]).Split(':')[0];
            treat.name = (Request.Form[0]).Split(':')[1];
            treat.diagnosis = Request.Form[1];
            treat.scan = Request.Form[2];
            treat.ert = Request.Form[3];
            for(int i = 4; i < frm.AllKeys.Length; i++)
            {
                treat.medicine = treat.medicine + "," + Request.Form[i];
            }
            blayer.updateTreatmentDetails(treat);
           
            return View();
        }
        public ActionResult AddTreatment()
        {
            System.Data.DataTable dt = blayer.getPatientName_id(HttpContext.Application["Username"].ToString());
            List<SelectListItem> ls = new List<SelectListItem>();
            int count = 0;
            foreach (DataRow row in dt.Rows)
            {

                ls.Add(new SelectListItem { Text = row["PID"].ToString() + ": " + row["Name"].ToString(), Value = count.ToString() });
                count++;
            }
            ViewBag.patientlst = ls;
            return View();
        }
        }
    } 

