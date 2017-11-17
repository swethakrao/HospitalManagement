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
public class BusinessLayer 
{
    DataAccess da = new DataAccess();
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
    public string insertAppointment(BookAnAppointment ba)
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
                string email = HttpContext.Current.Application["Username"].ToString();
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
   public string updatePersonalDetails(EmployeePatient ep)
    {
        string category = HttpContext.Current.Application["Category"].ToString();
        string uname = HttpContext.Current.Application["Username"].ToString();
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

    public int updateTreatmentDetails(Treatment treat)
    {
        int res = 0;
        Random r_no = new Random();
        string tid = (r_no.Next(1000, 9999)).ToString();
        return res;
    }
}