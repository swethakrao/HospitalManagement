using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for RepositoryHelper
/// </summary>
public class RepositoryHelper
{
	public RepositoryHelper()
	{
	}

    public static List<T> ConvertDataTableToList<T>(DataTable dt)
        where T:IEntity, new()
    {
        List<T> TList = new List<T>();
        foreach (DataRow dr in dt.Rows)
        {
            T tp = new T();
            tp.SetFields(dr);
            TList.Add(tp);
        }
        return TList;
    }
}