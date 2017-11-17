using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SessionFacade
/// </summary>
public class SessionFacade
{   // Facade is often implemented as a Singleton
    // but here due to static fields, we do not need Singleton
    //static readonly SessionFacade instance = new SessionFacade();
    
    //SessionFacade()
    //{
    //}
    //public static SessionFacade Instance
    //{
    //    get
    //    {
    //        return instance;
    //    }
    //}

    static readonly string _USERNAME = "USERNAME";
    public static string USERNAME
    {
        get
        {
            string res = null;
            if (HttpContext.Current.Session[_USERNAME] != null)
                res = (string)HttpContext.Current.Session[_USERNAME];
            return res;
        }
        set
        {
            HttpContext.Current.Session[_USERNAME] = value;
        }
    }

    static readonly string _CHECKINGACCTNUM = "CHECKINGACCOUNTNUM";
    public static string CHECKINGACCTNUM
    {
        get
        {
            string res = null;
            if (HttpContext.Current.Session[_CHECKINGACCTNUM] != null)
                res = (string)HttpContext.Current.Session[_CHECKINGACCTNUM];
            return res;
        }
        set
        {
            HttpContext.Current.Session[_CHECKINGACCTNUM] = value;
        }
    }

    static readonly string _PAGEREQUESTED = "PAGEREQUESTED";
    public static string PAGEREQUESTED
    {
        get
        {
            string res = null;
            if (HttpContext.Current.Session[_PAGEREQUESTED] != null)
                res = (string)HttpContext.Current.Session[_PAGEREQUESTED];
            return res;
        }
        set
        {
            HttpContext.Current.Session[_PAGEREQUESTED] = value;
        }
    }
}