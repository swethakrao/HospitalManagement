using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class EventDetails : IEntity
{
    public string EventID { get; set; }
    public string EventType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Name { get; set; }

    public void SetFields(DataRow dr)
    {
        this.EventID = (string)dr["EventID"];
        this.EventType = (string)dr["EventType"];
        this.StartDate = (DateTime)dr["StartDate"];
        this.EndDate = (DateTime)dr["EndDate"];
        this.Name = (string)dr["Name"];
    }
}