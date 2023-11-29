using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMSMSRestService;

namespace SMSControlPanel.Repository.DataLayer
{
    public interface IVirginmediasmsDL
    {
        Task<string> GetCompanyName();
        Task<string> InsertNewSms(string CompanyName, string WhatSent, string DateTimeSent, string Result,
            string Sentby, string IP, string MessagesSent, string CreditUsed, string DL, string DistListCount,
            string Message, int TemplateID, int SelectedTeamID, int SelectedMsgTitleID, string EmpId,
            string CompanyID, string CostCentre, string Cttype);
        Task InsertSMSDetail(string smsID, string nrSentTo, string msgId, string msgStatus, string dateTimeResp,
            string empID, int templateID, int selectedTeamID, int selectedMsgTitleID, string message);
        Task<ConfigurationDetails> GetSMSConfigurations();
    }
}
