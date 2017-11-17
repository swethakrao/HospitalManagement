using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IMyCache
/// </summary>
// A cache provider needs to implement this interface
// via an adapter 
public interface IWebCache
{
    void Remove(string key);
    void Store(string key, object obj);
    T Retrieve<T>(string key);
}