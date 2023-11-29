using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Models
{
    public class DateTimeReportModel
    {
        
        public string SentBy { get; set; }  
        public decimal Messages { get; set; }
        public DateTime DateTimeSent { get; set; }
        public int SmsID { get; set; }

    }

    public class DateReportModel
    {
        public string EmpID { get; set; }
        public string UserName { get; set; }
        public decimal TotalMessages { get; set; }
        public int FailedMessages { get; set; }
    }

    public class GenerateReportModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string[] EmpIds { get; set; }
        public string[] DeptIds { get; set; }
    }

    public class FailedMsgModel
    {
        public string Msg_Title { get; set; }
        public string PhoneNr { get; set; }
        public string Msgstatus { get; set; }
        public DateTime DateTimeResp { get; set; }
        public string MessageText { get; set; }

    }

    public class MessageModel
    {
        public string Msg_Title { get; set; }
        public string PhoneNr { get; set; }
        public string Msgstatus { get; set; }
        public DateTime DateTimeResp { get; set; }
        public string MsgContent { get; set; }

    }

    public class ExcelModel
    {
        public string Team { get; set; }
        public string TemplateName { get; set; }
        public string TemplateMessage { get; set; }
        public int Volumes { get; set; }
    }
}
