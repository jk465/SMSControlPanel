using SMSControlPanel.Models;
using SMSControlPanel.Repository.BusinessLogic;
using SMSControlPanel.Repository.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.BusinessLogic
{
    public class MsgDetailsBL : IMsgDetailsBL
    {
        private readonly IMsgDetailsDL _msgDetails;
        public MsgDetailsBL(IMsgDetailsDL msgDetails)
        {
            _msgDetails = msgDetails;
        }
        public async Task<List<FailedMsgModel>> GetFailedMessages(string fromDate, string toDate, string empID)
        {
            var ValidFromDate = DateTime.TryParse(fromDate, out DateTime FromDate);
            var ValidToDate = DateTime.TryParse(toDate, out DateTime ToDate);

            if (ValidFromDate && ValidToDate)
            {
                var FailedMessages = await _msgDetails.GetFailedMessages(FromDate, ToDate, empID);
                return FailedMessages;
            }

            return null;
        }

        public async Task<List<MessageModel>> GetMessageDetails(string smsID)
        {
            var MessageDctails = await _msgDetails.GetMessageDetails(smsID);
            return MessageDctails;
        }
    }
}
