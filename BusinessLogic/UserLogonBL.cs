
using SMSControlPanel.Models;
using SMSControlPanel.Repository.DataLayer;
using SMSControlPanel.Repository.BusinessLogic;
using System;
using System.Threading.Tasks;

namespace SMSControlPanel.BusinessLogic
{
    public class UserLogonBL : IUserLogonBL
    {
        private readonly IUserLogonDL _user;
        public UserLogonBL(IUserLogonDL user)
        {
            _user = user;
        }
        public async Task<string> GetTrowbridgeEmpID(string EmpId)
        {
            var Trowbridge_EmpId = await _user.GetTrowbridgeEmpID(EmpId);
            return Trowbridge_EmpId;
        }
        public async Task<UserDetails> GetDetails(string EmpId)
        {
            UserDetails user = await _user.GetDetails(EmpId);
            return user;
        }

        public async Task<int> GetOriginatorPrivilege(string _deptId, string DeptName, string _costCentre)
        {
            if (DeptName == null)
            {
                DeptName = string.Empty;
            }

            int DeptId = string.IsNullOrEmpty(_deptId) ? 0 : Convert.ToInt32(_deptId);

            int CostCentre = string.IsNullOrEmpty(_costCentre) ? 0 : Convert.ToInt32(_costCentre);

            int OriginatorPrivilege = await _user.GetOriginatorPrivilege(DeptId, DeptName, CostCentre);

            return OriginatorPrivilege;
        }
    }
}
