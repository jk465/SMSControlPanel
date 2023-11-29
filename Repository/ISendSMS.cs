using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository
{
    public interface ISendSMS
    {
        Task<string> SendSMSMessage(long CompanyID,
                               string CompanyName,
                               string Message,
                               string SMSType,
                               string DeviceNr,
                               string Originator,
                               string AuthUser,
                               string RequestingIP,
                               bool DataSent,
                               bool HasCredit,
                               string DeliveryDate,
                               string EmpID,
                               int templateID,
                               int SelectedTeamID,
                               int SelectedMsgTitleID,
                               bool Cttype,
                               string CostCentre,
                               string DeliveryMinutes = null,
                               long ServiceType = 0);
    }
}
