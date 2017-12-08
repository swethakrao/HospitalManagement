using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Author: Swetha KrishnamurthyRao
//UBID: 1004265
// Interface for progress charts
namespace HospitalManagementSystem.Models
{
    public interface ICharts
    {
        void progressChart(out string progressValue, out string dateOfProgress,string ppid);
    }
}
