using Microsoft.Extensions.Configuration;
using Serilog;
using SMSControlPanel.Repository.DataLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using VMSMSRestService;

namespace SMSControlPanel.DataLayer
{
    public class VirginmediasmsDL : IVirginmediasmsDL
    {
        private readonly string vm_connectionString;

        private readonly string Environment;
        private readonly string ApplicationName;
        private readonly string InsertSmsDetailQuery;
        public VirginmediasmsDL(IConfiguration config)
        {
            vm_connectionString = config.GetConnectionString("DefaultConnection").ToString();
            Environment = config.GetSection("Smsconfig").GetValue<string>("Environment").ToString();
            ApplicationName = config.GetSection("Smsconfig").GetValue<string>("ApplicationName").ToString();
            InsertSmsDetailQuery = config.GetSection("SqlQueries").GetValue<string>("InsertSMSDetails").ToString();
        }

        public async Task<string> GetCompanyName()
        {
            string queryString = "Select smsc_id, companyname from smscentres where flag = 'A';";

            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new(queryString, connection);

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            string companyName = dt.Rows[0]["companyname"].ToString();

            return companyName;
        }

        public async Task<string> InsertNewSms(string CompanyName, string WhatSent, string DateTimeSent, string Result, string Sentby, 
            string IP, string MessagesSent, string CreditUsed, string DL, string DistListCount, string Message, 
            int TemplateID, int SelectedTeamID, int SelectedMsgTitleID, string EmpId, string CompanyID,
            string CostCentre, string Cttype)
        {
            string response = null;
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("INSERTNEWMSG", connection);
            try
            {
                await connection.OpenAsync();

                cmd.Parameters.Add("@t_return", SqlDbType.VarChar, 50);
                cmd.Parameters["@t_return"].Direction = ParameterDirection.Output;

                cmd.Parameters.AddWithValue("@CompanyName", CompanyName);
                cmd.Parameters.AddWithValue("@WhatSent", WhatSent);
                cmd.Parameters.AddWithValue("@DateTimeSent", DateTimeSent);
                cmd.Parameters.AddWithValue("@Result", Result);
                cmd.Parameters.AddWithValue("@SentBy",Sentby);
                cmd.Parameters.AddWithValue("@IP", IP);
                cmd.Parameters.AddWithValue("@MessagesSent", MessagesSent);
                cmd.Parameters.AddWithValue("@CreditsUsed", CreditUsed);
                cmd.Parameters.AddWithValue("@DL", DL);
                cmd.Parameters.AddWithValue("@DistListCount", DistListCount);
                cmd.Parameters.AddWithValue("@MsgContent", Message);
                cmd.Parameters.AddWithValue("@TemplateID", TemplateID == 0 ? DBNull.Value : TemplateID.ToString());
                cmd.Parameters.AddWithValue("@SelectedTeamID", SelectedTeamID == 0 ? DBNull.Value : SelectedTeamID.ToString());
                cmd.Parameters.AddWithValue("@SelectedMsgTitleID", SelectedMsgTitleID == 0 ? DBNull.Value : SelectedMsgTitleID.ToString());
                cmd.Parameters.AddWithValue("@EmpID", EmpId);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@CostCentre", CostCentre);
                cmd.Parameters.AddWithValue("@Cttype", Cttype);

                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();

                response = cmd.Parameters["@t_return"].Value.ToString();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return response;
        }

        public async Task InsertSMSDetail(string smsID, string nrSentTo, string msgId, string msgStatus, string dateTimeResp,
            string empID, int templateID, int selectedTeamID, int selectedMsgTitleID, string message)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new(InsertSmsDetailQuery, connection);

            cmd.Parameters.AddWithValue("@smsID", smsID);
            cmd.Parameters.AddWithValue("@phoneNr", nrSentTo);
            cmd.Parameters.AddWithValue("@msgID", msgId);
            cmd.Parameters.AddWithValue("@msgStatus", msgStatus);
            cmd.Parameters.AddWithValue("@dateTimeResp", dateTimeResp);
            cmd.Parameters.AddWithValue("@empID", empID);
            cmd.Parameters.AddWithValue("@templateID", templateID == 0 ? DBNull.Value : templateID.ToString());
            cmd.Parameters.AddWithValue("@selectedTeamID", selectedTeamID == 0 ? DBNull.Value : selectedTeamID.ToString());
            cmd.Parameters.AddWithValue("@selectedMsgTitleID", selectedMsgTitleID == 0 ? DBNull.Value : selectedMsgTitleID.ToString());
            cmd.Parameters.AddWithValue("@messageText", message);

            try
            {
                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<ConfigurationDetails> GetSMSConfigurations()
        {
            ConfigurationDetails config = new();
            try
            {
                using SqlConnection connection = new(vm_connectionString);
                using SqlCommand cmd = new("usp_GetSMSRestConfigDetails", connection);

                cmd.Parameters.AddWithValue("@Environment", Environment);
                cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
                cmd.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                DataTable dt = new();
                dt.Load(reader);

                config.BaseUrl = dt.Rows[0][0].ToString();
                config.ServicePlanId = dt.Rows[0][1].ToString();
                config.ApiToken = dt.Rows[0][2].ToString();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            return config;
        }

       
    }
}
