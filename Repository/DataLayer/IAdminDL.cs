using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository.DataLayer
{
    public interface IAdminDL
    {
        Task<List<AdminModel>> LoadUsersData();
        Task<List<AdminModel>> SearchFirstLastName(string first, string last);
        Task<bool> DeleteUser(string empid,string update_by);
        Task<List<TeamTemplateModel>> GetTeamDetails(string deptID);
        Task<List<RoleModel>> GetRoles();
        Task<bool> UpdateUser(AdminModel model, string insertBy);
        Task<List<ColleagueModel>> GetColleagues(string firtname, string lastname);
        Task<bool> InsertUser(AdminModel model, string insertBy);
        Task<bool> CheckExist(AdminModel model);
    }
}
