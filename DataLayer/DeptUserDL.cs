using Microsoft.Extensions.Configuration;
using Serilog;
using SMSControlPanel.Models;
using SMSControlPanel.Repository.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.DataLayer
{
    public class DeptUserDL : IDeptUserDL
    {
        private readonly string vm_connectionString;
        public DeptUserDL(IConfiguration config)
        {
            vm_connectionString = config.GetConnectionString("DefaultConnection").ToString();
        }
        public async Task<string> GetDeptDetails(int DeptId)
        {
            string DepartmentName = null;
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("usp_getdeptusers", connection);

            cmd.Parameters.AddWithValue("@deptid", DeptId);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            DataTable dt = new();
            dt.Load(reader);

            if (dt.Rows.Count != 0)
            {
                DepartmentName = dt.Rows[0]["Department"].ToString();
            }
            return DepartmentName;
        }

        public async Task<List<DepartmentModel>> GetDepartment(string DeptId)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_GetDept_VMTP", connection);

            cmd.Parameters.AddWithValue("@DepartmentID", DeptId);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            List<DepartmentModel> resultList = CommonClass.ConvertDataTable<DepartmentModel>(dt);
            
            return resultList;
        }

        public async Task<List<DepartmentModel>> GetDepartmentsForAdmin(string EmpId)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_GetDeptForAdmin_VMTP", connection);

            cmd.Parameters.AddWithValue("@EmpID", EmpId);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            List<DepartmentModel> resultList = CommonClass.ConvertDataTable<DepartmentModel>(dt);

            return resultList;
        }

        public async Task<List<DeptUserModel>> GetDeptUsers(string DeptId)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_GetDeptUsers_VMTP", connection);
            
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter("@DepartmentID", SqlDbType.VarChar); 
            param.Value = DeptId;
            cmd.Parameters.Add(param);

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);           

            List<DeptUserModel> resultList = CommonClass.ConvertDataTable<DeptUserModel>(dt);

            return resultList;
        }
    }
}
