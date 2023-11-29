using Microsoft.Extensions.Configuration;
using Serilog;
using SMSControlPanel.Models;
using SMSControlPanel.Repository.BusinessLogic;
using SMSControlPanel.Repository.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.DataLayer
{
    public class GetDistListDL : IGetDistListDL
    {
        private readonly string dev_connectionString;
        private readonly string TeamTemplates;
        private readonly string TeamTemplate_for_Role_Id_3;
        private readonly string MessageTitle;
        private readonly string MsgTemplate;
        private readonly string PhoneNumbers;


        public GetDistListDL(IConfiguration config)
        {
            dev_connectionString = config.GetConnectionString("devConnection").ToString();
            TeamTemplates = config.GetSection("SqlQueries").GetValue<string>("GetTeamTemplate").ToString();
            TeamTemplate_for_Role_Id_3 = config.GetSection("SqlQueries")
                                            .GetValue<string>("GetTeamTemplate_for_Role_Id_3").ToString();
            MessageTitle = config.GetSection("SqlQueries").GetValue<string>("GetMessageTitle").ToString();
            MsgTemplate = config.GetSection("SqlQueries").GetValue<string>("GetMsgTemplate").ToString();
            PhoneNumbers = config.GetSection("SqlQueries").GetValue<string>("GetDLPhoneNumbers").ToString();
        }
        public async Task<List<TemplateModel>> GetTemplates(string companyID)
        {
            using SqlConnection connection = new(dev_connectionString);
            using SqlCommand cmd = new("SP_LoadTemplates_VMTP", connection);

            cmd.Parameters.AddWithValue("@CompanyID", companyID);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);            

            List<TemplateModel> distList = CommonClass.ConvertDataTable<TemplateModel>(dt);

            return distList;
        }

        public async Task<List<TeamTemplateModel>> GetTeamTemplates(int RoleID, int TeamID)
        {
            string query = TeamTemplate_for_Role_Id_3;
            if (RoleID != 3)
            {
                query = TeamTemplates;
            }

            using SqlConnection connection = new(dev_connectionString);
            using SqlCommand cmd = new(query, connection);

            if (RoleID != 3)
                cmd.Parameters.AddWithValue("@ID", TeamID);

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            List<TeamTemplateModel> distList = CommonClass.ConvertDataTable<TeamTemplateModel>(dt);

            return distList;
        }

        public async Task<List<MessageTitleModel>> GetMessageTitle(int teamID)
        {
            using SqlConnection connection = new(dev_connectionString);
            using SqlCommand cmd = new(MessageTitle, connection);
            
            cmd.Parameters.AddWithValue("@Team_ID", teamID);

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            DataTable dt = new();
            dt.Load(reader);

            List<MessageTitleModel> distList = CommonClass.ConvertDataTable<MessageTitleModel>(dt);

            return distList;
        }

        public async Task<List<string>> GetDL(string companyID)
        {
            
            using SqlConnection connection = new(dev_connectionString);
            using SqlCommand cmd = new("SP_BindDropdown_VMTP", connection);

            cmd.Parameters.AddWithValue("@CompanyID", companyID);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            List<string> distList = new();

            foreach (DataRow row in dt.Rows)  // need to declare DataRow in foreach loop explicitly
            {
                distList.Add(row["Listname"].ToString());
            }

            return distList;
        }
    
        public async Task<string> GetMsgTemplate(int msgTitleID)
        {
            using SqlConnection connection = new(dev_connectionString);
            using SqlCommand cmd = new(MsgTemplate, connection);
            cmd.Parameters.AddWithValue("@Msg_Title_ID", msgTitleID);

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            string Msgtemplate = dt.Rows[0]["Msg_template"].ToString();

            return Msgtemplate;
        }

        public async Task<List<string>> GetPhoneNumbers(string distlist,string companyId)
        {
            using SqlConnection connection = new(dev_connectionString);
            using SqlCommand cmd = new(PhoneNumbers, connection);

            cmd.Parameters.AddWithValue("@CompanyID", companyId);
            cmd.Parameters.AddWithValue("@DistList", distlist);

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            List<string> phoneNumbersList = new();
            foreach (DataRow row in dt.Rows)  // need to declare DataRow in foreach loop explicitly
            {
                phoneNumbersList.Add(row["PhoneNr"].ToString());
            }

            return phoneNumbersList;
        }

        public async Task<List<DLModel>> GetDLandEntries(string companyID)
        {
            using SqlConnection connection = new(dev_connectionString);
            using SqlCommand cmd = new("SP_GetDistList_VMTP", connection);

            cmd.Parameters.AddWithValue("@CompanyID", companyID);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            DataTable dt = new();
            dt.Load(reader);

            var distList = CommonClass.ConvertDataTable<DLModel>(dt);

            return distList;
        }

        public async Task<bool> DeleteDL(string listName, string companyID)
        {
            using SqlConnection connection = new(dev_connectionString);
            using SqlCommand cmd = new("SP_VMTP_deleterow", connection);

            cmd.Parameters.AddWithValue("@listname", listName);
            cmd.Parameters.AddWithValue("@CompanyID", companyID);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            int output = await cmd.ExecuteNonQueryAsync();

            bool result = output != 0;

            return result;
        }

        public async Task<bool> CheckExists(string listName, string company)
        {
            using SqlConnection connection = new(dev_connectionString);
            using SqlCommand cmd = new("SP_DistListExists_VMTP", connection);
            
            cmd.Parameters.AddWithValue("@value", listName);
            cmd.Parameters.AddWithValue("@company", company);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            bool result = bool.Parse(dt.Rows[0][0].ToString());

            return result;
        }

        public async Task<int> InsertDistList(string strSQL)
        {
            using SqlConnection connection = new(dev_connectionString);
            using SqlCommand cmd = new(strSQL, connection);

            await connection.OpenAsync();
            int output = await cmd.ExecuteNonQueryAsync();
            connection.Close();

            return output;
        }
    }
}
