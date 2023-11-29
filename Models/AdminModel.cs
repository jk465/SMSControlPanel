using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Models
{
    public class AdminModel
    {
        public int ID { get; set; }
        public string logonid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string costcentre { get; set; }
        public string departmentid { get; set; }

        public string department { get; set; }

        public string phonenumber { get; set; }
        public int r_role { get; set; }
        public string Team_Name { get; set; }
        public string rol_role { get; set; }
    }

    public class ColleagueModel
    {
        public string fname { get; set; }
        public string lname { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string phonenumber { get; set; }
        public string costcentre { get; set; }
        public double departmentid { get; set; }
        public string department { get; set; }
    }

}
