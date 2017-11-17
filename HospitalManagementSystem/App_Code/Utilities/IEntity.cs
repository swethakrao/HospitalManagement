using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for IEntity
/// </summary>
public interface IEntity
{
    void SetFields(DataRow dr);
}