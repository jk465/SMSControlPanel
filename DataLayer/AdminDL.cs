using Microsoft.Extensions.Configuration;
using SMSControlPanel.Models;
using SMSControlPanel.Repository.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SMSControlPanel.DataLayer
{
    public class AdminDL : IAdminDL
    {
        private readonly string vm_connectionString;
        public AdminDL(IConfiguration config)
        {
            vm_connectionString = config.GetConnectionString("DefaultConnection").ToString();
        }
        public async Task<List<AdminModel>> LoadUsersData()
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_VMTP_USER_SELECT", connection); //SP_VMTP_USER_SELECT

            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            List<AdminModel> resultList = CommonClass.ConvertDataTable<AdminModel>(dt);

            return resultList;
        }
        public async Task<List<AdminModel>> SearchFirstLastName(string first, string last)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_VMTP_USER_SELECT_SEARCHMANAGEUSERS", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FIRSTNAME", first);
            cmd.Parameters.AddWithValue("@LASTNAME", last);

            await connection.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            List<AdminModel> resultList = CommonClass.ConvertDataTable<AdminModel>(dt);

            return resultList;
        }
        public async Task<bool> DeleteUser(string empid, string update_by)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_VMTP_USER_DELETE", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@u_id", empid);
            cmd.Parameters.AddWithValue("@u_update_by", update_by);

            await connection.OpenAsync();
            int result = await cmd.ExecuteNonQueryAsync();
            connection.Close();

            bool output = result != 0;
            return output;

        }
        public async Task<List<TeamTemplateModel>> GetTeamDetails(string deptID)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_VMTP_Team_MANAGEUSERSSELECT", connection);
            cmd.Parameters.AddWithValue("@deptid", deptID);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            var Teams = CommonClass.ConvertDataTable<TeamTemplateModel>(dt);

            return Teams;
        }

        public async Task<List<RoleModel>> GetRoles()
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_VMTP_ROLE_SELECT", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            var Roles = CommonClass.ConvertDataTable<RoleModel>(dt);

            return Roles;
        }

        public async Task<bool> UpdateUser(AdminModel model, string insertBy)
        {
            string TeamID = await GetTeamID(model.Team_Name);
            string RoleID = await GetRoleID(model.rol_role);

            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_VMTP_USER_UPDATE", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@u_logon", model.logonid);
            cmd.Parameters.AddWithValue("@u_firstname", model.firstname);
            cmd.Parameters.AddWithValue("@u_lastname", model.lastname);
            cmd.Parameters.AddWithValue("@u_costcentre", model.costcentre);
            cmd.Parameters.AddWithValue("@u_departmentid", model.departmentid);
            cmd.Parameters.AddWithValue("@u_department", model.department);
            cmd.Parameters.AddWithValue("@u_phonenumber", model.phonenumber);
            cmd.Parameters.AddWithValue("@u_rol_id", RoleID);
            cmd.Parameters.AddWithValue("@Team_ID", TeamID);
            cmd.Parameters.AddWithValue("@u_insert_by", insertBy);

            await connection.OpenAsync();
            int result = await cmd.ExecuteNonQueryAsync();
            connection.Close();

            bool output = result != 0;
            return output;
        }

        private async Task<string> GetRoleID(string Rolename)
        {
            string query = "select rol_id as [ID] from tblRole_VMTP where rol_role = @RoleName";

            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new (query, connection);
            cmd.Parameters.AddWithValue("@RoleName", Rolename);

            await connection.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            string RoleID = dt.Rows[0][0].ToString();

            return RoleID;
        }

        private async Task<string> GetTeamID(string team_Name)
        {
            string query = "select ID from tblTeam  where Team_Name = @TeamName";

            using SqlConnection connection = new (vm_connectionString);
            using SqlCommand cmd = new (query, connection);
            cmd.Parameters.AddWithValue("@TeamName", team_Name);

            await connection.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            string TeamId = dt.Rows[0][0].ToString();

            return TeamId;
        }

        public async Task<List<ColleagueModel>> GetColleagues(string firstname, string lastname)
        {
            if (firstname == null)
            {
                firstname = "";
            }
            if (lastname == null)
            {
                lastname = "";
            }
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_VMTP_USER_SEARCH", connection);

            cmd.Parameters.AddWithValue("@firstname", firstname);
            cmd.Parameters.AddWithValue("@lastname", lastname);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);           

            var colleagues = CommonClass.ConvertDataTable<ColleagueModel>(dt);

            return colleagues;
        }

        public async Task<bool> InsertUser(AdminModel model, string insertBy)
        {
            using SqlConnection connection = new (vm_connectionString);
            using SqlCommand cmd = new ("SP_VMTP_USER_INSERT", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@u_logon", model.logonid);
            cmd.Parameters.AddWithValue("@u_firstname", model.firstname);
            cmd.Parameters.AddWithValue("@u_lastname", model.lastname);
            cmd.Parameters.AddWithValue("@u_costcentre", model.costcentre);
            cmd.Parameters.AddWithValue("@u_departmentid", model.departmentid);
            cmd.Parameters.AddWithValue("@u_department", model.department);
            cmd.Parameters.AddWithValue("@u_phonenumber", model.phonenumber);
            cmd.Parameters.AddWithValue("@u_rol_id", model.r_role);
            cmd.Parameters.AddWithValue("@u_team_id", model.Team_Name);
            cmd.Parameters.AddWithValue("@u_insert_by", insertBy);


            await connection.OpenAsync();
            int result = await cmd.ExecuteNonQueryAsync();
            connection.Close();

            bool output = result != 0;
            return output;
        }

        public async Task<bool> CheckExist(AdminModel model)
        {
            using SqlConnection connection = new (vm_connectionString);
            using SqlCommand cmd = new ("SP_UserExists_VMTP", connection);

            cmd.Parameters.AddWithValue("@value", model.logonid);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            bool result = bool.Parse(dt.Rows[0][0].ToString());

            return result;
        }

    }
}