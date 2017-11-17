using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RepositoryFactory
/// </summary>
public class GenericFactory<T, I>  // I = interface
     where T : I // T implements I 
{
    GenericFactory()  // private constructor
    {
    }

    public static I CreateInstance(params object[] args)
    {
       // return new T();
        return (I)Activator.CreateInstance(typeof(T), args);
    }
}

