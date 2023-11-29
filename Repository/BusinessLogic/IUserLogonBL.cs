using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository.BusinessLogic
{
    public interface IUserLogonBL
    {
        Task<UserDetails> GetDetails(string EmpId);
        Task<string> GetTrowbridgeEmpID(string EmpId);
        Task<int> GetOriginatorPrivilege(string _deptId, string DeptName, string _costCentre);

    }
}
