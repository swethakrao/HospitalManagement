using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class WeatherDetails : IEntity
{
    public string WeatherID { get; set; }
    public decimal MinTemp { get; set; }
    public decimal MaxTemp { get; set; }

    public void SetFields(DataRow dr)
    {
        this.WeatherID = (string)dr["WeatherID"];
        this.MinTemp = (decimal)dr["MinTemp"];
        this.MaxTemp = (decimal)dr["MaxTemp"];
    }
}