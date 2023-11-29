using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SMSControlPanel.Models;
using SMSControlPanel.Repository.DataLayer;
using Serilog;

namespace SMSControlPanel.DataLayer
{
    public class UserLogonDL : IUserLogonDL
    {
        private readonly string vm_connectionString;
        private readonly string collegues_connectionString;
        public UserLogonDL(IConfiguration config)
        {
            vm_connectionString = config.GetConnectionString("DefaultConnection").ToString();
            collegues_connectionString = config.GetConnectionString("ColleguesConnection").ToString();
        }

        public async Task<UserDetails> GetDetails(string EmpId)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_GetDetails_VMTP", connection);

            cmd.Parameters.AddWithValue("@EmpID", EmpId);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            DataTable dt = new();
            dt.Load(reader);

            List<UserDetails> resultList = CommonClass.ConvertDataTable<UserDetails>(dt);
            UserDetails userDetails = resultList.FirstOrDefault();

            return userDetails;
        }

        public async Task<string> GetTrowbridgeEmpID(string EmpId)
        {
            using SqlConnection connection = new(collegues_connectionString);
            using SqlCommand cmd = new("SP_TrowbridgeUser_VMTP", connection);

            cmd.Parameters.AddWithValue("@logon", EmpId);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            DataTable dt = new();
            dt.Load(reader);

            string Trowbridge_EmpID = dt.Rows[0]["ID"].ToString();

            return Trowbridge_EmpID;
        }

        public async Task<int> GetOriginatorPrivilege(int DeptId, string DeptName, int CostCentre)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_check_OriginatorPriviliges", connection);

            cmd.Parameters.AddWithValue("@user_dept", DeptName);
            cmd.Parameters.AddWithValue("@dept_id", DeptId);
            cmd.Parameters.AddWithValue("@cost_centre", CostCentre);
            cmd.CommandType = CommandType.StoredProcedure;
            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            DataTable dt = new();
            dt.Load(reader);

            var result = int.TryParse(dt.Rows[0]["outdata"].ToString(), out int OriginatorPrivilege);

            return OriginatorPrivilege;
        }

    }
}
