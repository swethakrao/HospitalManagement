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

//Author: Swetha KrishnamurthyRao
//UBID: 1004265
// Home Contoller responsible for the views of the entire application.
namespace HospitalManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        BusinessLayer blayer = new BusinessLayer();
        ICharts _ICharts;

        //Displays the index page of the application
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        //Success message page
        public ActionResult success()
        {
            return View();
        }
        // On user logout, remove all the application contexts
        public ActionResult logout()
        {
            HttpContext.Session["Username"] = null;
            HttpContext.Session["Category"] = null;
            return RedirectToAction("login");
        }
        // Login page on load
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }
        //Authenticates the login credentials with that of the database
        [HttpPost]
        public ActionResult login(Value v)
        {
            try
            {
                if (blayer.authentication(v) != null)
                {
                    
                    HttpContext.Session["Username"] = blayer.authentication(v);
                    //Session["username"] = blayer.authentication(v);
                    //Session["category"] = v.category;
                    HttpContext.Session["Category"] = v.category;

                    ViewBag.Message = "Correct credentials";
                    return RedirectToAction("success");

                }
                else
                {
                    ViewBag.Message = "Wrong credentials.";
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return View();
        }
        //Edits the personal details of the user using WebAPI GET
        public ActionResult edit()
        {
            EmployeePatient ep = null;
            System.Data.DataTable dt = blayer.getAllInformation(HttpContext.Session["Username"].ToString(), HttpContext.Session["Category"].ToString());
            foreach(DataRow row in dt.Rows)
            {
                if (HttpContext.Session["Category"].ToString().Equals("Patient"))
                {
                    ep = new EmployeePatient();
                    ep.id = row["PID"].ToString();
                    ep.name = row["Name"].ToString();
                    ep.sex = row["Sex"].ToString();
                    ep.address = row["Address"].ToString();
                    ep.contactinfo = row["contact_no"].ToString();
                }
                else if(HttpContext.Session["Category"].ToString().Equals("Employee"))
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
                var responseTask = client.GetAsync("ep?id=" + HttpContext.Session["Username"].ToString());
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
        //Web API post message on editing the personal details of the logged in user.
        [HttpPost]
        public ActionResult Edit(EmployeePatient ep)
        {
            EmployeePatient empPatient = new EmployeePatient();
            empPatient = ep;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:57394/api/ep");
                    empPatient.epusername = HttpContext.Session["Username"].ToString();
                    empPatient.epcategory = HttpContext.Session["Category"].ToString();
                    //HTTP POST
                    var putTask = client.PutAsJsonAsync<EmployeePatient>("ep", empPatient);
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
    

        // Allows the patient to book an appointment with the doctor
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
            string locations = HttpContext.Session["location"].ToString();
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
        //On booking, sends an email confirmation and stores the same in database
      [HttpPost]
        public ActionResult bookapp(BookAnAppointment ba)
        {
            int count = 0;
            Random r_no = new Random();
            string locations = HttpContext.Session["location"].ToString();
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
                    ba.PDetails = "PD"+(HttpContext.Session["Username"].ToString()).Substring(1, HttpContext.Session["Username"].ToString().Length - 1);
                    ba.pdid = (r_no.Next(1000, 9999)).ToString();
                    
                }
                count++;
            }

            ba.location = HttpContext.Session["location"].ToString();
            string emailep = HttpContext.Session["Username"].ToString();
            string email_id = blayer.insertAppointment(ba, emailep);
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
                    WebMail.UserName = "*****";
                    WebMail.Password = "*****";

                    //Sender email address.  
                    WebMail.From = "****";

                    //Send email  
                    WebMail.Send(to: email_id, subject: "Doctor Appointment Confirmation", body: "<p>Hello,</p><br/><p>This is to confirm your appointment for " + ba.status + " on " + ((DateTime)ba.date_admitted).ToString("MM/dd/yyyy") + "at " + ba.admit_time + "(24 hours ET)</p>" , isBodyHtml: true);
                }
                catch(Exception e)
                {
                    ViewBag.Message = "Check the entered details.";
                }
            }

            return RedirectToAction("SuccessAppointment");
        }
        //uses Google API to display all the location of the Hospital in the USA
       public ActionResult locations()
        {

            return View();
        }
        //Loads the Personal details of the logged in user as a table
        public ActionResult PersonalDetails()
        {

            IEnumerable<EmployeePatient> info_list = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:57394/api/");
                    //HTTP GET
                    var responseTask = client.GetAsync("info?id=" + HttpContext.Session["Username"].ToString() + ":" + HttpContext.Session["Category"].ToString());
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
            }
            catch(Exception e)
            {
                throw e;
            }
                return View(info_list);  
        }
        //Displays the locations of the hospital so that the patient can book an appointment with the doctor in that area.
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

        // Posting the selected location for displaying the doctors present in the selected area.
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

                HttpContext.Session["location"] = loc;
            }

            return RedirectToAction("bookapp");
        }

        public ActionResult SuccessAppointment()
        {
            return View();
        }
// The Doctor can view all the appointments scheduled using WebAPI GET
        public ActionResult ViewAppointment()
        {
            IEnumerable<BookAnAppointment> info_list = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57394/api/");
                //HTTP GET
                var responseTask = client.GetAsync("viewappointment?id= "+ HttpContext.Session["Username"].ToString());
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
        // Allows the doctor to cancel an appointment
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
        //Displays all the upcoming appointments with the particular doctor
        public ActionResult upcomingAppointment()
        {
            IList<upcomingApp> up = new List<upcomingApp>();
            upcomingApp upapp = null;
            System.Data.DataTable dt = null;
            string doctor_id = HttpContext.Session["Username"].ToString();
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
        //Cancels an appointment and updates the table
        public ActionResult cancel_appointment(string id)
        {
            int res = blayer.deleteAppointment(id);
            if (res < 1)
            {
                ViewBag.Message = "Unable to cancel appointment. Try again later.";
            }
            return RedirectToAction("upcomingAppointment");
        }
        //Add treatments to patients
        [HttpPost]
        public ActionResult AddTreatment(FormCollection frm)
        {
            Treatment treat = new Treatment();
            List<SelectListItem> lst = (List<SelectListItem>)HttpContext.Session["pidname"];
            treat.pdid = lst[int.Parse(Request.Form[0])].Text.ToString().Split(':')[0];
            treat.name = lst[int.Parse(Request.Form[0])].Text.ToString().Split(':')[1];
            treat.diagnosis = Request.Form[1];
            treat.scan = Request.Form[2];
            treat.ert = Request.Form[3];
            treat.medicine = Request.Form[4];
            string unameep = HttpContext.Session["Username"].ToString();
            for (int i = 5; i < frm.AllKeys.Length; i++)
            {
                treat.medicine = treat.medicine + "," + Request.Form[i];
            }
           if( blayer.updateTreatmentDetails(treat, unameep) > 0)
            {
                return RedirectToAction("successTreatment");
            }
           
            return View();
        }
        public ActionResult successTreatment()
        {
            return View();
        }
        //Displays a list of patients under that doctor so that the doctor can choose to update treatment details
        public ActionResult AddTreatment()
        {
            System.Data.DataTable dt = blayer.getPatientName_id(HttpContext.Session["Username"].ToString());
            List<SelectListItem> ls = new List<SelectListItem>();
            int count = 0;
            foreach (DataRow row in dt.Rows)
            {

                ls.Add(new SelectListItem { Text = row["PID"].ToString() + ": " + row["Name"].ToString(), Value = count.ToString() });
                count++;
            }
            ViewBag.patientlst = ls;
            HttpContext.Session["pidname"] = ls;
            return View();
        }
        //Displays the treatment details of a patient
        public ActionResult viewPatientTreatment()
        {
            Treatment treat = new Treatment(); 
            DataTable dt = null;
            string p_id = HttpContext.Session["Username"].ToString();
            dt = blayer.getAllTreatmentDetails(p_id);
            foreach(DataRow row in dt.Rows)
            {
                treat.d_name = row["E_Name"].ToString();
                treat.diagnosis = row["Diagnosis"].ToString();
                treat.scan = row["Scan"].ToString();
                treat.medicine = row["Medicine"].ToString();
                treat.ert = row["Recovery"].ToString();
            }
            return View(treat);
        }
        //Displays a set of questions so that the doctor can check progress based on these data
        [HttpGet]
        public ActionResult checkProgress()
        {
            questionnaire q = new questionnaire();
            try
            {   
                string[] readText = System.IO.File.ReadAllLines(@"C:\Users\swetha rao\documents\visual studio 2015\Projects\HospitalManagementSystem\HospitalManagementSystem\progress.txt");
                q.q_one = readText[0];
                q.q_two = readText[1];
                q.q_three = readText[2];
                q.q_four = readText[3];
                q.q_five = readText[4];
                q.q_six = readText[5];
                q.q_seven = readText[6];
                q.q_eight = readText[7];

            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return View(q);
        }
        //On submission of the form, the data is stored in Treatment table
        [HttpPost]
        public ActionResult checkProgress(questionnaire quest)
        {
            int total = 0;
            total = Convert.ToInt32(quest.a_one) + Convert.ToInt32(quest.a_two) + Convert.ToInt32(quest.a_three) + Convert.ToInt32(quest.a_four) + Convert.ToInt32(quest.a_five) + Convert.ToInt32(quest.a_six) + Convert.ToInt32(quest.a_seven) + Convert.ToInt32(quest.a_eight);
            string checkpr = (DateTime.Today).ToString() + "(" + total.ToString() + ")" ;
            string unameep = HttpContext.Session["Username"].ToString();
            blayer.updateprogress(checkpr,unameep);
            ViewBag.Message = "Progress sent to doctor";
            return RedirectToAction("checkProgress");
        }
        //Displays a list of patient details under that Doctor
        [HttpGet]
        public ActionResult DiagnosisPatientDetails()
        {
            System.Data.DataTable dt = blayer.getPatientName_id_progress(HttpContext.Session["Username"].ToString());
            List<SelectListItem> ls = new List<SelectListItem>();
            int count = 0;
            foreach (DataRow row in dt.Rows)
            {

                ls.Add(new SelectListItem { Text = row["patient"].ToString(), Value = count.ToString() });
                count++;
            }
            HttpContext.Session["progresspid"] = ls;
            ViewBag.det = ls;
            return View();
        }
        //On selection of a patient from the list, it redirects to BarChart.cshtml
        [HttpPost]
        public ActionResult DiagnosisPatientDetails(FormCollection frm)
        {
            Treatment treat = new Treatment();
            List<SelectListItem> lst = (List<SelectListItem>)HttpContext.Session["progresspid"];
            treat.pdid = lst[int.Parse(Request.Form[0])].Text.ToString().Split(':')[0];
            treat.name = lst[int.Parse(Request.Form[0])].Text.ToString().Split(':')[1];

            HttpContext.Session["progresspid"] = treat.pdid;

            return RedirectToAction("BarChart");
        }
        //BarChart displays the patient progress as a bar chart
        [HttpGet]
        public ActionResult BarChart()
        {

            _ICharts = blayer;
            try
            {
                string ppid = HttpContext.Session["progresspid"].ToString();
                string tempCount = string.Empty;
                string tempDate = string.Empty;
                _ICharts.progressChart(out tempCount, out tempDate, ppid);
                ViewBag.MobileCount_List = tempCount.Trim();
                ViewBag.Productname_List = tempDate.Trim();

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Search for symptoms shows a autocomplete textbox of symptoms already solved by the doctor
        public ActionResult searchForSymptoms()
        {
            return View();
        }
        //On selection of a symptom, the diagnosis details are fetched from the database and displayed.
        [HttpPost]
        public ActionResult searchForSymptoms(string prefix, FormCollection frm)
        {
            DataTable dt = blayer.getTreatmentForSearch();
            int count = 0;
            List<Treatment> objList = new List<Treatment>();
            Treatment t = new Treatment();
            foreach(DataRow row in dt.Rows)
            {
                if (prefix == null && Request.Form[1] == row["Diagnosis"].ToString())
                {
                    t.diagnosis = row["Diagnosis"].ToString();
                    t.scan = row["Scan"].ToString();
                    t.ert = row["Recovery"].ToString();
                    t.medicine = row["Medicine"].ToString();
                }
                else
                {
                    objList.Add(new Treatment { pdid = (count++).ToString(), diagnosis = row["Diagnosis"].ToString() });
                }
            }
            if (prefix == null)
            {
                return RedirectToAction("diagnosisResult","Home",t);
            }
            var CityName = (from N in objList
                            where N.diagnosis.StartsWith(prefix)
                            select new { N.diagnosis });
            return Json(CityName, JsonRequestBehavior.AllowGet);
        }

        //Displays the diagnosis results
        public ActionResult diagnosisResult(Treatment tr)
        {
            
            return View(tr);
        }
    }
    } 

