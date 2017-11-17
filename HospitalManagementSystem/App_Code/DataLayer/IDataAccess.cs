using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

/// <summary>
/// Summary description for IDataAccess
/// </summary>
public interface IDataAccess
{
	object GetSingleAnswer(string sql,List<DbParameter> PList);
    DataTable GetDataTable(string sql, List<DbParameter> PList);
    int InsOrUpdOrDel(string sql, List<DbParameter> PList);
}