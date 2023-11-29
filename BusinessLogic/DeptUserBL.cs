using SMSControlPanel.Repository.DataLayer;
using SMSControlPanel.Repository.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMSControlPanel.Models;

namespace SMSControlPanel.BusinessLogic
{
    public class DeptUserBL : IDeptUserBL
    {
        private readonly IDeptUserDL _dept;
        public DeptUserBL(IDeptUserDL dept)
        {
            _dept = dept;
        }

        public async Task<string> GetDeptDetails(int DeptId)
        {
            string DepartmentName = await _dept.GetDeptDetails(DeptId);

            return DepartmentName;
        }

        public async Task<List<DepartmentModel>> GetDepartments(string DeptId)
        {
            var Departments = await _dept.GetDepartment(DeptId);

            return Departments;
        }

        public async Task<List<DepartmentModel>> GetDepartmentsForAdmin(string EmpId)
        {
            var Departments = await _dept.GetDepartmentsForAdmin(EmpId);

            return Departments;
        }

        public async Task<List<DeptUserModel>> GetDeptUsers(string DeptId)
        {
            var users = await _dept.GetDeptUsers(DeptId);
            return users;
        }
    }
}
