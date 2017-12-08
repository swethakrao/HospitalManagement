using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using HospitalManagementSystem.Models;
using System.Data.Common;
using System.Data.SqlClient;

/// <summary>
/// Summary description for BusinessLayer
/// </summary>
/// Author: Swetha KrishnamurthyRao
/// UBID: 1004265
public class BusinessLayer : ICharts
{
    DataAccess da = new DataAccess();
    //Authentication of the login credentials.
   public string authentication(Value v)
    {
        string sql = "";
        string res = "";
        if(v.category == "Patient")
        {
            string uname, pwd;
            uname = v.usrname;
            pwd = v.password;
            sql = "select PID from Patient where PID =  @uname and Password = @pwd";
            List<DbParameter> PList = new List<DbParameter>();
            SqlParameter p1 = new SqlParameter("@uname", SqlDbType.VarChar, 10);
            p1.Value = uname;
            PList.Add(p1);
            SqlParameter p2 = new SqlParameter("@pwd", SqlDbType.VarChar, 50);
            p2.Value = pwd;
            PList.Add(p2);
            object obj = da.GetSingleAnswer(sql, PList);
            if (obj != null)
                res = obj.ToString();

        }
        else if(v.category == "Employee")
        {
            string uname, pwd;
            uname = v.usrname;
            pwd = v.password;
            sql = "select EID from Employee where EID =  @uname and Password = @pwd";
            List<DbParameter> PList = new List<DbParameter>();
            SqlParameter p1 = new SqlParameter("@uname", SqlDbType.VarChar, 10);
            p1.Value = uname;
            PList.Add(p1);
            SqlParameter p2 = new SqlParameter("@pwd", SqlDbType.VarChar, 50);
            p2.Value = pwd;
            PList.Add(p2);
            object obj = da.GetSingleAnswer(sql, PList);
            if (obj != null)
                res = obj.ToString();
        }
        


        return res;
    }
    // Gets the doctor's name and specialization
    public string getDoctorName(string docid)
    {
        string res = null;
        string sql = "SELECT (E_Name + ' ' + specialist) AS namespec FROM Employee where EID =@docid; ";
        List<DbParameter> PList = new List<DbParameter>();
        SqlParameter p1 = new SqlParameter("@docid", SqlDbType.VarChar, 10);
        p1.Value = docid;
        PList.Add(p1);
        res = (string)da.GetSingleAnswer(sql, PList);

        return res;
    }
    // Get all the personal details from Database.
    public DataTable getAllAppointment(string Pdetails)
    {
        DataTable dt = null;
        string sql = "Select * from PDetails where PDetails=@pde;";
        List<DbParameter> PList = new List<DbParameter>();
        SqlParameter p1 = new SqlParameter("@pde", SqlDbType.VarChar, 50);
        p1.Value = Pdetails;
        PList.Add(p1);
        dt = da.GetDataTable(sql, PList);
        return dt;
    }
    // Get all the list of Doctors based on location.
    public DataTable getAllDoctors(string location)
    {
        DataTable dt = null;
        string sql = "Select * from Employee where specialist <> 'Nurse' and location=@lo";
        List<DbParameter> PList = new List<DbParameter>();
        SqlParameter p1 = new SqlParameter("@lo", SqlDbType.VarChar, 50);
        p1.Value = location;
        PList.Add(p1);
        dt = da.GetDataTable(sql, PList);
        return dt;
    }
    // Get all the locations of the hospital's location.
    public DataTable getAllBranchLocations()
    {
        DataTable dt = null;
        try
        {
            string sql = "Select * from Locations";
            List<DbParameter> PList = new List<DbParameter>();
            dt = da.GetDataTable(sql, PList);
        }
        catch(Exception e)
        {
            throw e;
        };
        return dt;
    }
    // Get all the details of Patient using PID
    public DataTable getAllInformation(string username,string category)
    {
        DataTable dt = null;
        try
        {
            if (category.Equals("Patient"))
            {
                string sql = "select * from Patient where " +
                    "PID=@username";
                List<DbParameter> PList = new List<DbParameter>();
                SqlParameter p1 = new SqlParameter("@username", SqlDbType.VarChar, 10);
                p1.Value = username;
                PList.Add(p1);
                dt = da.GetDataTable(sql, PList);
            }
            else
            {
                string sql = "select * from Employee where " +
                   "EID=@username";
                List<DbParameter> PList = new List<DbParameter>();
                SqlParameter p1 = new SqlParameter("@username", SqlDbType.VarChar, 10);
                p1.Value = username;
                PList.Add(p1);
                dt = da.GetDataTable(sql, PList);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        };
        return dt;
    }
    // Insert an appointment details into the table
    public string insertAppointment(BookAnAppointment ba, string email)
    {
        string result = null;
        try
        {
            string sql = "INSERT INTO PDetails (PDID, PDetails, date_admitted, status, doctor_id, admit_time, location) VALUES (@pdid, @pdetails, @dateadmit, @stat, @docid, @time, @loc);";
            List<DbParameter> PList = new List<DbParameter>();
            SqlParameter p1 = new SqlParameter("@pdid", SqlDbType.VarChar, 10);
            p1.Value = ba.pdid;
            PList.Add(p1);
            SqlParameter p2 = new SqlParameter("@pdetails", SqlDbType.VarChar, 10);
            p2.Value = ba.PDetails;
            PList.Add(p2);
            SqlParameter p3 = new SqlParameter("@dateadmit", SqlDbType.Date);
            p3.Value = ba.date_admitted;
            PList.Add(p3);
            SqlParameter p4 = new SqlParameter("@stat", SqlDbType.VarChar, 50);
            p4.Value = ba.status;
            PList.Add(p4);
            SqlParameter p5 = new SqlParameter("@docid", SqlDbType.VarChar, 10);
            p5.Value = ba.doctor_id;
            PList.Add(p5);
            SqlParameter p6 = new SqlParameter("@time", SqlDbType.VarChar, 20);
            p6.Value = ba.admit_time;
            PList.Add(p6);
            SqlParameter p7 = new SqlParameter("@loc", SqlDbType.VarChar, 50);
            p7.Value = ba.location;
            PList.Add(p7);
            result = (da.InsOrUpdOrDel(sql, PList)).ToString();
            if( result != null)
            {
                string emailPatient = "select email from Patient where PID=@patientid;";
                List<DbParameter> Patientlist = new List<DbParameter>();
                SqlParameter para1 = new SqlParameter("@patientid", SqlDbType.VarChar, 10);
                para1.Value = email;
                Patientlist.Add(para1);
                string email_id = (string) da.GetSingleAnswer(emailPatient, Patientlist);
                result = email_id;
            }
        }
        catch(Exception e)
        {
            throw e;
        }

        return result;
    }
    // Update the change in appointment to the Database
    public string updateAppointment(BookAnAppointment ba)
    {
        string result = null;
        try
        {
            DateTime admit_date = new DateTime(Int32.Parse(ba.year), Int32.Parse(ba.month), Int32.Parse(ba.day));
            string sql = "update PDetails set status=@status, date_admitted = @da, doctor_id = @docid, admit_time = @admittime, location = @location where PDID = @pdid";
            List<DbParameter> PList = new List<DbParameter>();
            SqlParameter p1 = new SqlParameter("@status", SqlDbType.VarChar, 50);
            p1.Value = ba.status;
            PList.Add(p1);
            SqlParameter p2 = new SqlParameter("@da", SqlDbType.Date);
            p2.Value = admit_date;
            PList.Add(p2);
            SqlParameter p3 = new SqlParameter("@docid", SqlDbType.VarChar, 10);
            p3.Value = ba.doctor_id;
            PList.Add(p3);
            SqlParameter p4 = new SqlParameter("@admittime", SqlDbType.VarChar, 20);
            p4.Value = ba.admit_time;
            PList.Add(p4);
            SqlParameter p5 = new SqlParameter("@location", SqlDbType.VarChar, 50);
            p5.Value = ba.location;
            PList.Add(p5);
            SqlParameter p6 = new SqlParameter("@pdid", SqlDbType.VarChar, 10);
            p6.Value = ba.pdid;
            PList.Add(p6);
            result = (da.InsOrUpdOrDel(sql, PList)).ToString();
        }
        catch(Exception e)
        {
            throw (e);
        }
        return result;
    }
    // On edit of personal details, Update the details to Database
   public string updatePersonalDetails(EmployeePatient ep, string uname, string category)
    {
        string name = ep.name;
        string sex = ep.sex;
        string address = ep.address;
        string contact = ep.contactinfo;
        string result = null;
        try
        {
            if (category.Equals("Patient"))
            {
                string sql = "update Patient set Name=@name, Sex = @sex, Address = @address, contact_no = @contact where PID = @uname";
                List<DbParameter> PList = new List<DbParameter>();
                SqlParameter p1 = new SqlParameter("@name", SqlDbType.VarChar, 80);
                p1.Value = name;
                PList.Add(p1);
                SqlParameter p2 = new SqlParameter("@sex", SqlDbType.VarChar, 20);
                p2.Value = sex;
                PList.Add(p2);
                SqlParameter p3 = new SqlParameter("@address", SqlDbType.VarChar, 50);
                p3.Value = address;
                PList.Add(p3);
                SqlParameter p4 = new SqlParameter("@contact", SqlDbType.VarChar, 10);
                p4.Value = contact;
                PList.Add(p4);
                SqlParameter p5 = new SqlParameter("@uname", SqlDbType.VarChar, 10);
                p5.Value = uname;
                PList.Add(p5);
                result = (da.InsOrUpdOrDel(sql, PList)).ToString();
            }
            else
            {
                string sql = "update Employee set E_Name=@name , sex = @sex, E_address = @address, contact_info = @contact where EID = @uname";
                List<DbParameter> PList = new List<DbParameter>();
                SqlParameter p1 = new SqlParameter("@name", SqlDbType.VarChar, 80);
                p1.Value = name;
                PList.Add(p1);
                SqlParameter p2 = new SqlParameter("@sex", SqlDbType.VarChar, 20);
                p2.Value = sex;
                PList.Add(p2);
                SqlParameter p3 = new SqlParameter("@address", SqlDbType.VarChar, 50);
                p3.Value = address;
                PList.Add(p3);
                SqlParameter p4 = new SqlParameter("@contact", SqlDbType.VarChar, 10);
                p4.Value = contact;
                PList.Add(p4);
                SqlParameter p5 = new SqlParameter("@uname", SqlDbType.VarChar, 10);
                p5.Value = uname;
                PList.Add(p5);
                result = (da.InsOrUpdOrDel(sql, PList)).ToString();
            }
        }
        catch(Exception ex)
        {
            throw ex;
        };
        return result;
    }
    // Delete the appointment on Cancellation 
    public int deleteAppointment(string id)
    {
        int result = 0;
        try
        {
            string sql = "DELETE FROM PDetails WHERE PDID = @id ";
            List<DbParameter> PList = new List<DbParameter>();
            SqlParameter p1 = new SqlParameter("@id", SqlDbType.VarChar, 10);
            p1.Value = id;
            PList.Add(p1);
            result = da.InsOrUpdOrDel(sql, PList);
        }
        catch(Exception e)
        {
            throw e;
        }

        return result;
    }
    // Get all the details of a Patient
    public DataTable getAllPatientDetails(string doctor_id)
    {
        string sql = "SELECT p.Name, p.Sex, p.email, d.date_admitted," +
                     "d.admit_time, d.status, d.pdid FROM Patient p, PDetails d " +
                     "WHERE p.PDetails = d.PDetails AND d.doctor_id = @doc; ";
        List<DbParameter> PList = new List<DbParameter>();
        SqlParameter p1 = new SqlParameter("@doc", SqlDbType.VarChar, 10);
        p1.Value = doctor_id;
        PList.Add(p1);
        DataTable dt = da.GetDataTable(sql, PList);
        return dt;
    }
    // Get all the treatment details using PID
    public DataTable getAllTreatmentDetails(string patient_id)
    {
        string sql = "SELECT t.Diagnosis, t.scan, t.Medicine, t.Recovery," +
                     "e.E_Name FROM Treatment t, Employee e " +
                     "WHERE t.EID = e.EID AND t.PID = @pid; ";
        List<DbParameter> PList = new List<DbParameter>();
        SqlParameter p1 = new SqlParameter("@pid", SqlDbType.VarChar, 10);
        p1.Value = patient_id;
        PList.Add(p1);
        DataTable dt = da.GetDataTable(sql, PList);
        return dt;
    }
    // Get progress of a Patient using the Patient ID
    public DataTable getPatientName_id_progress(string doctor_id)
    {
        DataTable dt = null;
        string sql = "SELECT (p.PID + ':' + p.Name) AS patient FROM Patient p, Treatment t " +
                     "WHERE (p.PID = t.PID and EID=@doc) and t.Progress <> '';";
        List<DbParameter> PList = new List<DbParameter>();
        SqlParameter p1 = new SqlParameter("@doc", SqlDbType.VarChar, 10);
        p1.Value = doctor_id;
        PList.Add(p1);
        dt = da.GetDataTable(sql, PList);
        return dt;
    }
    // Obtain Patient ID which is then used to obtain the patient progress
    public DataTable getPatientName_id(string doctor_id)
    {
        DataTable dt = null;
        string sql = "SELECT p.PID, p.Name FROM Patient p, PDetails pd " +
                     "WHERE p.PDetails = pd.PDetails AND pd.doctor_id = @doc";
        List<DbParameter> PList = new List<DbParameter>();
        SqlParameter p1 = new SqlParameter("@doc", SqlDbType.VarChar, 10);
        p1.Value = doctor_id;
        PList.Add(p1);
        dt = da.GetDataTable(sql, PList);
        return dt;
    }
    // The answers of the questionnaire is calculated and updated in a database.
    public int updateprogress(string checkpr,string unameep)
    {
        int res = 0;
        string dte = null;
        string val = null;
        string sql;
        string sql_select = "SELECT Progress FROM Treatment WHERE PID=@pidd;";
        List<DbParameter> PList1 = new List<DbParameter>();
        SqlParameter sp1 = new SqlParameter("@pidd", SqlDbType.VarChar, 10);
        sp1.Value = unameep;
        PList1.Add(sp1);
        string progress = (da.GetSingleAnswer(sql_select, PList1)).ToString();
        if (progress != "")
        {
            string[] ch = progress.Split(',');
            foreach(string c in ch){
                dte = c.Split('(')[0];
                if(dte == checkpr.Split('(')[0])
                {
                    val = dte + "(" + checkpr.Split('(')[1];
                }
                else
                {
                    val = progress + "," + checkpr;
                }
            }

        }
        else
        {
            val = checkpr;
            
        }
        sql = "UPDATE Treatment SET Progress=@prog WHERE PID = @pid";
        List<DbParameter> PList = new List<DbParameter>();
        SqlParameter p1 = new SqlParameter("@prog", SqlDbType.VarChar, 1000);
        p1.Value = val;
        PList.Add(p1);
        SqlParameter p2 = new SqlParameter("@pid", SqlDbType.VarChar, 10);
        p2.Value = unameep;
        PList.Add(p2);
        res = da.InsOrUpdOrDel(sql, PList);
        return res;
    }
    // Update the treatment given by the doctor to the table Treatment
    public int updateTreatmentDetails(Treatment treat, string unameep)
    {
        int res = 0;
        Random r_no = new Random();
        string tid = (r_no.Next(1000, 9999)).ToString();
        string eid = unameep;
        try
        {
            string sql = "INSERT INTO Treatment (TID, PID, EID, Diagnosis, Scan, Medicine, Recovery) " +
                         "VALUES (@tid, @pid, @eid, @diag, @sc, @med, @rec);";
            List<DbParameter> PList = new List<DbParameter>();
            SqlParameter p1 = new SqlParameter("@tid", SqlDbType.VarChar, 10);
            p1.Value = tid;
            PList.Add(p1);
            SqlParameter p2 = new SqlParameter("@pid", SqlDbType.VarChar, 10);
            p2.Value = treat.pdid;
            PList.Add(p2);
            SqlParameter p3 = new SqlParameter("@eid", SqlDbType.VarChar, 10);
            p3.Value = eid;
            PList.Add(p3);
            SqlParameter p4 = new SqlParameter("@diag", SqlDbType.VarChar, 1000);
            p4.Value = treat.diagnosis;
            PList.Add(p4);
            SqlParameter p5 = new SqlParameter("@sc", SqlDbType.VarChar, 90);
            p5.Value = treat.scan;
            PList.Add(p5);
            SqlParameter p6 = new SqlParameter("@med", SqlDbType.VarChar, 2000);
            p6.Value = treat.medicine;
            PList.Add(p6);
            SqlParameter p7 = new SqlParameter("@rec", SqlDbType.VarChar, 50);
            p7.Value = treat.ert;
            PList.Add(p7);
            res = da.InsOrUpdOrDel(sql, PList);
        }
        catch(Exception e)
        {
            throw (e);
        }
        return res;
    }
    //Used to obtain the data for displaying a patient's progress to the doctor.

    public void progressChart(out string progressValue, out string dateOfProgress, string ppid)
    {
        string sql_select = "SELECT Progress FROM Treatment WHERE PID=@pidd;";
        List<DbParameter> PList1 = new List<DbParameter>();
        SqlParameter sp1 = new SqlParameter("@pidd", SqlDbType.VarChar, 10);
        sp1.Value = ppid;
        PList1.Add(sp1);
        string progress = (da.GetSingleAnswer(sql_select, PList1)).ToString();
        string[] temp = progress.Split(',');
        string tempDate = null, tempCount = null;
        foreach(string te in temp)
        {
            tempDate = tempDate + (Convert.ToDateTime(te.Split('(')[0])).Date.Day + "," ;
            tempCount = tempCount + (te.Split('(')[1]).Replace(')', ' ') + ",";
        }

        progressValue = tempCount.TrimEnd(',');
        dateOfProgress = tempDate.TrimEnd(',');
    }
    // Get various symptoms to display them as an autocomplete to aid the doctor in finding similar symptoms.
    public DataTable getTreatmentForSearch()
    {
        DataTable dt = null;
        string sql = "SELECT Diagnosis, Scan, Medicine, Recovery FROM Treatment;";
        List<DbParameter> PList1 = new List<DbParameter>();
        dt = da.GetDataTable(sql, PList1);
        return dt;
    }
}