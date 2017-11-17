using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.Common;

/// <summary>
/// Summary description for DataAccess
/// </summary>
public class DataAccess : IDataAccess  
{
    public static
        string CONNSTR = ConfigurationManager.ConnectionStrings["HospitalManagementSystem"].ConnectionString;

    public DataAccess()
	{
	}

    #region IDataAccess Members

    public object GetSingleAnswer(string sql, List<DbParameter> PList)
    {
        object obj = null;
        SqlConnection conn = new SqlConnection(CONNSTR);
        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            foreach (SqlParameter p in PList)
                cmd.Parameters.Add(p);
            obj = cmd.ExecuteScalar(); 
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            conn.Close();
        }
        return obj;
    }

    

    public System.Data.DataTable GetDataTable(string sql, List<DbParameter> PList)
    {
        DataTable dt = new DataTable();
        SqlConnection conn = new SqlConnection(CONNSTR);
        try
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sql, conn);
            foreach (SqlParameter p in PList)
                cmd.Parameters.Add(p);
            da.SelectCommand = cmd;
            da.Fill(dt);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            conn.Close();
        }
        return dt;
    }

    public int InsOrUpdOrDel(string sql, List<DbParameter> PList)
    {
        int rows = 0;
        SqlConnection conn = new SqlConnection(CONNSTR);
        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            foreach (SqlParameter p in PList)
                cmd.Parameters.Add(p);
            rows = cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            conn.Close();
        }
        return rows;
    }

    #endregion
}