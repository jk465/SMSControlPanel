using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Models
{
    public class TemplateModel
    {
        public int TemplateID { get; set; }
        public string TemplateDesc { get; set; }
    }

    public class TeamTemplateModel
    {
        public int ID { get; set; }
        public string Team_Name { get; set; }
    }

    public class MessageTitleModel
    {
        public int ID { get; set; }
        public string Msg_title { get; set; }
    }

    public class MsgTemplateModel
    {
        public int ID { get; set; }
        public string Msg_template { get; set; }
    }

    public class DepartmentModel
    {
        public string DEPARTMENTID { get; set; }
        public string DEPARTMENT  { get; set; }
    }

    public class DeptUserModel
    {
        public string EmpID { get; set; }
        public string FIRSTNAME { get; set; }
        public string DEPARTMENTID { get; set; }
        public string DEPARTMENT { get; set; }
    }

    public class RoleModel
    {
        public int rol_id { get; set; }
        public string rol_role { get; set; }
    }

}
