using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class ShoppingDetails : IEntity
{
    public string ShoppingID { get; set; }
    public string Category { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }


    public void SetFields(DataRow dr)
    {
        this.ShoppingID = (string)dr["ShoppingID"];
        this.Category = (string)dr["Category"];
        this.Name = (string)dr["Name"];
        this.Location = (string)dr["Location"];
    }
}