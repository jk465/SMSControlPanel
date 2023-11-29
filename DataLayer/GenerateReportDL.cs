using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class GenerateReportDL : IGenerateReportDL
    {
        private readonly string vm_connectionString;
        public GenerateReportDL(IConfiguration config)
        {
            vm_connectionString = config.GetConnectionString("DefaultConnection").ToString();
        }
        public async Task<JsonResult> GetReport(string empID, int role, string companyID, DateTime fromDate, DateTime toDate, string[] empList, int checkDetail)
        {
            string SPName = null;
            if (role == 1)
            {
                if (checkDetail == 0)
                {
                    SPName = "SP_GetReportDate_VMTP";
                }
                else if (checkDetail == 1)
                {
                    SPName = "SP_GeneralReport_VMTP";       
                }
            }

            if (SPName != null)
            {
                using SqlConnection connection = new(vm_connectionString);
                using SqlCommand cmd = new(SPName, connection);

                cmd.Parameters.AddWithValue("@EmpID", empID);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@CompanyID", companyID);  //companyID not used in SP
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);

                cmd.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                DataTable dt = new();
                dt.Load(reader);

                if (checkDetail == 0)
                {
                    var ResultReport = CommonClass.ConvertDataTable<DateReportModel>(dt);
                    return new JsonResult(ResultReport);
                }
                else if (checkDetail == 1)
                {
                    List<object> result = new();

                    foreach (DataRow row in dt.Rows)
                    {
                        var value = new
                        {
                            SentBy = row["Sent By"].ToString(),
                            Messages = row["Messages"].ToString(),
                            DateTimeSent = row["DateTimeSent"].ToString(),
                            SmsID = row["SmsID"].ToString()
                        };

                        result.Add(value);
                    }

                    return new JsonResult(result);
                }
            }

            return null;
        }

        public async Task<JsonResult> GetAdminReport(string empID, int role, string companyID, DateTime fromDate, DateTime toDate, string empList, int checkDetail)
        {
            //string temp = "H8815864,S7704073";
            string SPName = null;
            if (role == 2)
            {
                if (checkDetail == 0)
                    SPName = "SP_GetReportDateAdmin_VMTP";

            }
            if (role == 3)
            {
                if (checkDetail == 0)
                    SPName = "SP_GetReportDateSuperAdmin_VMTP";
            }

            if(SPName != null)
            {
                using SqlConnection connection = new(vm_connectionString);
                using SqlCommand cmd = new(SPName, connection);

                cmd.Parameters.AddWithValue("@EmpID", empList);     //List of emp ids
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@CompanyID", companyID);  //companyID not used in SP
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);

                cmd.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                DataTable dt = new();
                dt.Load(reader);

                if (checkDetail == 0)
                {
                    var ResultReport = CommonClass.ConvertDataTable<DateReportModel>(dt);
                    return new JsonResult(ResultReport);
                }
                else if (checkDetail == 1)
                {
                    var ResultReport = CommonClass.ConvertDataTable<DateTimeReportModel>(dt);
                    return new JsonResult(ResultReport);
                }
            }

            return null;
        }

        public async Task<JsonResult> GenerateDetailReportAdmins(string empID, int role, string companyID, DateTime fromDate, DateTime toDate, string empList, int checkDetail)
        {
            //string temp = "H8815864,S7704073";
            string SPName = null;
            if (role == 2)  
            {
                if (checkDetail == 1)
                    SPName = "SP_GetReportDateAdminDetail_VMTP";

            }
            if (role == 3)
            {
                if (checkDetail == 1)
                    SPName = "SP_GetReportDateSuperAdminDetail_VMTP";
            }

            if (SPName != null)
            {
                using SqlConnection connection = new(vm_connectionString);
                using SqlCommand cmd = new(SPName, connection);

                cmd.Parameters.AddWithValue("@EmpID", empList);     //List of emp ids
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@CompanyID", companyID);  //companyID not used in SP
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);

                cmd.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                DataTable dt = new();
                dt.Load(reader);

                List<object> result = new();

                foreach (DataRow row in dt.Rows)
                {
                    var value = new
                    {
                        SentBy = row["Sent By"].ToString(),
                        Messages = row["Messages"].ToString(),
                        DateTimeSent = row["DateTimeSent"].ToString(),
                        SmsID = row["SmsID"].ToString()
                    };

                    result.Add(value);
                }

                return new JsonResult(result);
            }

            return null;
        }

        public async Task<List<string>> GetMonth()
        {
            List<string> months = new();

            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("sp_MonthRecords", connection);

            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            DataTable dt = new();
            dt.Load(reader);

            foreach (DataRow row in dt.Rows)
            {
                months.Add(row[0].ToString());
            }

            return months;
        }

        public async Task<List<ExcelModel>> GetExcelData(int month, int year)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_MsgDetail_Monthly", connection);

            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            DataTable dt = new();
            dt.Load(reader);

            List<ExcelModel> result = CommonClass.ConvertDataTable<ExcelModel>(dt);

            return result;
        }
    }
}
