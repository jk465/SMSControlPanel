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
    public class MsgDetailsDL : IMsgDetailsDL
    {
        private readonly string vm_connectionString;
        public MsgDetailsDL(IConfiguration config)
        {
            vm_connectionString = config.GetConnectionString("DefaultConnection").ToString();
        }
        public async Task<List<FailedMsgModel>> GetFailedMessages(DateTime fromDate, DateTime toDate, string empID)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_FailureDetail", connection);

            cmd.Parameters.AddWithValue("@EmpID", empID);
            cmd.Parameters.AddWithValue("@fromDate", fromDate);
            cmd.Parameters.AddWithValue("@toDate", toDate);

            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);            

            var FailedMessages = CommonClass.ConvertDataTable<FailedMsgModel>(dt);
            return FailedMessages;
        }

        public async Task<List<MessageModel>> GetMessageDetails(string smsID)
        {
            using SqlConnection connection = new(vm_connectionString);
            using SqlCommand cmd = new("SP_MsgDetail", connection);

            cmd.Parameters.AddWithValue("@SmsID", smsID);
            cmd.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            DataTable dt = new();
            dt.Load(reader);

            var MessageDetails = CommonClass.ConvertDataTable<MessageModel>(dt);
            return MessageDetails;
        }
    }
}
