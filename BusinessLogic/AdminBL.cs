using SMSControlPanel.Models;
using SMSControlPanel.Repository.BusinessLogic;
using SMSControlPanel.Repository.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.BusinessLogic
{
    public class AdminBL : IAdminBL
    {
        private readonly IAdminDL _admin;

        public AdminBL(IAdminDL admin)
        {
            _admin = admin;
        }

        public async Task<List<AdminModel>> LoadUsersData()
        {
            var ManageUsers = await _admin.LoadUsersData();

            return ManageUsers;
        }

        public async Task<List<AdminModel>> SearchFirstLastName(string first, string last)
        {
            if (first == null){
                first = string.Empty;

            }
            if (last == null)
            {
                last = string.Empty;

            }
            var firstlast = await _admin.SearchFirstLastName(first, last);


            return firstlast;
        }

        public async Task<bool> DeleteUser(string empid,string update_by)
        {
           return await _admin.DeleteUser(empid, update_by);
        }

        public async Task<List<TeamTemplateModel>> GetTeamDetails(string deptID)
        {
            List<TeamTemplateModel> Teams = await _admin.GetTeamDetails(deptID);
            return Teams;
        }

        public async Task<List<RoleModel>> GetRoles()
        {
            List<RoleModel> Roles = await _admin.GetRoles();
            return Roles;
        }

        public async Task<bool> UpdateUser(AdminModel model, string insertBy)
        {
            if (model.costcentre == null)
                model.costcentre = "";
            if (model.phonenumber == null)
                model.phonenumber = "";

            bool result = await _admin.UpdateUser(model, insertBy);
            return result;
        }

        public async Task<List<ColleagueModel>> GetColleagues(string firtname, string lastname)
        {
            List<ColleagueModel> colleagues = await _admin.GetColleagues(firtname, lastname);
            return colleagues;
        }

        public async Task<bool> InsertUser(AdminModel model, string insertBy)
        {
            if (model.costcentre == null)
                model.costcentre = "";
            if (model.phonenumber == null)
                model.phonenumber = "";

            bool result = await _admin.InsertUser(model, insertBy);
            return result;
        }

        public async Task<bool> CheckExist(AdminModel model)
        {
            bool result = await _admin.CheckExist(model);
            return result;
        }
    }


}

