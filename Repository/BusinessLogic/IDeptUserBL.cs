using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository.BusinessLogic
{
    public interface IDeptUserBL
    {
        Task<string> GetDeptDetails(int DeptId);
        Task<List<DepartmentModel>> GetDepartments(string DeptId);
        Task<List<DepartmentModel>> GetDepartmentsForAdmin(string EmpId);
        Task<List<DeptUserModel>> GetDeptUsers(string DeptId);
    }
}
