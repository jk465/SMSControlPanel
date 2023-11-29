using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository.DataLayer
{
    public interface IMsgDetailsDL
    {
        Task<List<FailedMsgModel>> GetFailedMessages(DateTime fromDate, DateTime toDate, string empID);
        Task<List<MessageModel>> GetMessageDetails(string smsID);
    }
}
