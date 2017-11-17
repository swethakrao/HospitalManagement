using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MemCacheAdapter
/// </summary>
public class MemCachedAdapter : IWebCache
{
    // for Memcached product
    #region IWebCache Members

    public void Remove(string key)
    {
        //throw new NotImplementedException();
    }

    public void Store(string key, object obj)
    {
        //throw new NotImplementedException();
    }

    public T Retrieve<T>(string key)
    {
        return default(T);
        //throw new NotImplementedException();
    }

    #endregion
}