using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository.DataLayer
{
    public interface IUserLogonDL
    {
        Task<UserDetails> GetDetails(string EmpId);
        Task<string> GetTrowbridgeEmpID(string EmpId);
        Task<int> GetOriginatorPrivilege(int DeptId, string DeptName, int CostCentre);
    }
}
