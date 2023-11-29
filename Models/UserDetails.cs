using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Models
{
    public class UserDetails
    {
        public string EmpID { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string DEPARTMENTID { get; set; }
        public string COSTCENTRE { get; set; }
        public string CompanyName { get; set; }
        public int Role { get; set; }
        public string Team { get; set; }
    }
}
