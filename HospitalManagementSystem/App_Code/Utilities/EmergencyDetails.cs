using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class EmergencyDetails : IEntity
{
    public string EmergencyID { get; set; }
    public string Type { get; set; }
    public string Contact { get; set; }
    public string Address { get; set; }


    public void SetFields(DataRow dr)
    {
        this.EmergencyID = (string)dr["EmergencyID"];
        this.Type = (string)dr["Type"];
        this.Contact = (string)dr["Contact"];
        this.Address = (string)dr["Address"];
    }
}