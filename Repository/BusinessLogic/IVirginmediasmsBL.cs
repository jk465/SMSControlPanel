using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMSMSRestService;

namespace SMSControlPanel.Repository.BusinessLogic
{
    public interface IVirginmediasmsBL
    {
        string ValidateDeviceNr(ref string DeviceNr, ref long ElementCount, ref bool DistList, ref string RetVal);

        Task<string> InsertNewSms(string CompanyName, string WhatSent, DateTime DateTimeSent, string Result,
            string Sentby, string IP, long MessagesSent, long CreditUsed, string DL, string DistListCount,
            string Message, int TemplateID, int SelectedTeamID, int SelectedMsgTitleID, string EmpId,
            long CompanyID, string CostCentre, bool Cttype);

        Task InsertSMSDetail(string smsID, string nrSentTo, string msgId, string msgStatus, string dateTimeResp,
            string empID, int templateID, int selectedTeamID, int selectedMsgTitleID, string message);

        Task<string[]> SendMessage(SMSDetail sms);
        string[] IsvalidData(ref string originator, long serviceType, string message, string deviceNr);
    }
}
