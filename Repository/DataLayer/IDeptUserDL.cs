using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository.DataLayer
{
    public interface IDeptUserDL
    {
        Task<string> GetDeptDetails(int DeptId);
        Task<List<DepartmentModel>> GetDepartment(string DeptId);
        Task<List<DepartmentModel>> GetDepartmentsForAdmin(string DeptId);
        Task<List<DeptUserModel>> GetDeptUsers(string DeptId);
    }
}
