using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Utils
/// </summary>
public class Utils
{
	public Utils()
	{
	}

    public static string StripPunctuation(string s1)
    {
        string sout = s1.Replace("'", "");
        sout = sout.Replace("--", "");
        sout = sout.Replace(";", "");
        return sout;

    }
}