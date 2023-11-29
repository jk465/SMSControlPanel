using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository.BusinessLogic
{
    public interface IMsgDetailsBL
    {
        Task<List<FailedMsgModel>> GetFailedMessages(string fromDate, string toDate, string empID);
        Task<List<MessageModel>> GetMessageDetails(string smsID);
    }
}
