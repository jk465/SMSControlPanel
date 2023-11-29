using Microsoft.VisualBasic;
using Serilog;
using SMSControlPanel.Repository;
using SMSControlPanel.Repository.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMSMSRestService;

namespace SMSControlPanel.BusinessLogic
{
    public class SendSMS : ISendSMS
    {
        private readonly IVirginmediasmsBL _vmsms;

        public SendSMS(IVirginmediasmsBL vmsms)
        {
            _vmsms = vmsms;
        }
        public async Task<string> SendSMSMessage(long CompanyID,
                               string CompanyName, //ref
                               string Message,
                               string SMSType,
                               string DeviceNr,
                               string Originator,
                               string AuthUser,
                               string RequestingIP,
                               bool DataSent,   //out
                               bool HasCredit,  //out
                               string DeliveryDate, //ref
                               string EmpID,
                               int templateID,
                               int SelectedTeamID,
                               int SelectedMsgTitleID,
                               bool Cttype,
                               string CostCentre,
                               string DeliveryMinutes = null,
                               long ServiceType = 0)
        {
            //string m_strDistList;
            string strRetVal = null;
            string strDistList, strParams;

            string[] vDLCount, vDLRetVal = null;


            long lngMessagesToSend, lngCreditsUsed, lngDLElementCount, rvCount;
            bool bDataSent, bHasCredit, bMultiSend;
            DateTime dDeliveryDate;
            string MsgId = null, MsgStatus; // getGUID, strHostSql;
            string NrSentTo, SmsID;

            int ptr;
            string[] vRetValArray;
            string[] GetSendRpt = null;
           // string UrlEncMessage;

            bMultiSend = false;
            lngMessagesToSend = 0;
            lngCreditsUsed = 0;
            //VirginmediasmsBL strDeliveryDate1 = new Virginmediasms();
            bool m_bDistList = false;
            dDeliveryDate = DateTime.Now;
            _ = VirginmediasmsBL.FormatDateAndTime(dDeliveryDate);

            if (DeliveryDate != null)
            {
                dDeliveryDate = DateTime.Now;
                _ = VirginmediasmsBL.FormatDateAndTime(dDeliveryDate);
            }
            if (m_bDistList == false)
            {
                strRetVal = _vmsms.ValidateDeviceNr(ref DeviceNr, ref lngMessagesToSend, ref m_bDistList, ref strRetVal);

                if (strRetVal != null)
                {
                    return strRetVal;
                }

                //bHasCredit = CheckCredit(ref CompanyName, ref DeviceNr, ref lngMessagesToSend, ref strRetVal, ref bHasCredit, ref SMSType, Message);
                //Credit Check has been removed as per advice
                bHasCredit = true;

                if (Strings.InStr(DeviceNr, ",") > 0)
                {
                    m_bDistList = true;
                   // m_strDistList = "Multi-Single";
                    bMultiSend = true;
                    vDLCount = Strings.Split(DeviceNr, ","); //// Added by PAS 10.10.02 to manage muli-single 
                    _ = Information.UBound(vDLCount) + 1; //// PAS}

                }

            }
            else
            {
                bHasCredit = true;
                vDLCount = Strings.Split(DeviceNr, ","); //// Added by PAS 10.10.02 to manage muli-single 
                _ = Information.UBound(vDLCount) + 1; //// PAS}
            }

            string strWhatSent;
            if ((bHasCredit == true) && (strRetVal == null))
            {
                //ErrorLog("bHasCredit", bHasCredit.ToString());
                if (Strings.Len(Originator) == 2)
                {
                    Originator = Originator + " ";
                }
                //************** Adding Flash Message Option
                if (SMSType == "F")
                {
                    strWhatSent = "FLASH_MSG";
                }
                else
                {
                    strWhatSent = "TEXT";
                }
                //  getGUID = _vmsms.GUID_Generate();

                SMSDetail sms = new();

                sms.Originator = Originator;
                sms.Message = Message;


                if (DeviceNr.Substring(0, 1) == "0")
                {

                    DeviceNr = "44" + DeviceNr.Substring(1, DeviceNr.Length);
                    sms.DestNumber = DeviceNr;

                }
                else
                {
                    sms.DestNumber = DeviceNr;
                }
                try
                {
                    string[] Isvalid = _vmsms.IsvalidData(ref Originator, ServiceType, Message, DeviceNr);
                    if (Isvalid == null)
                    {
                        GetSendRpt = await _vmsms.SendMessage(sms);

                        foreach (var msg in GetSendRpt)
                        {
                            Log.Logger.Information(msg);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex.Message);
                }


                int GetSendRptCount = Information.UBound(GetSendRpt);
                for (rvCount = 0; rvCount <= GetSendRptCount; rvCount++)
                {
                    strRetVal = strRetVal + GetSendRpt[rvCount] + ",";
                }

                if (Strings.Right(strRetVal, 1) == ",")
                {
                    strRetVal = Strings.Mid(strRetVal, 1, (Strings.Len(strRetVal) - 1));
                }

                strParams = Message + " sent to " + DeviceNr;
            }
            else
            {
                return Convert.ToString(bHasCredit);
            }

            if (Strings.Left(strRetVal, 2) == "01")
            {
                strRetVal = "Message Sent";
                lngCreditsUsed = lngMessagesToSend;
                // ErrorLog("strRetVal", strRetVal);

                bDataSent = true;
                //// to handle Mblox SMPP response
            }
            else if (Strings.InStr(strRetVal, "::") > 0)
            {
                if (m_bDistList || bMultiSend == true)
                {
                    vDLRetVal = Strings.Split(strRetVal, ",");
                }
                else
                {
                    MsgId = Strings.Replace(Strings.Mid(strRetVal, 24), "ID: ", " ");
                }
                if (Strings.InStr(Strings.LCase(strRetVal), "success") > 0)
                {
                    strRetVal = "Message Sent";
                }
                else
                {
                    strRetVal = "Message Failed";
                }

                lngCreditsUsed = lngMessagesToSend;

                bDataSent = true;
                //// to handle Clickatell response
            }
            else if (Strings.InStr(strRetVal, "ID:") > 0)
            {
                if (m_bDistList | bMultiSend == true)
                {
                    strRetVal = Strings.Replace(strRetVal, Constants.vbLf, "");
                    vDLRetVal = Strings.Split(strRetVal, "ID: ");
                }
                else
                {
                    MsgId = Strings.Replace(Strings.Mid(strRetVal, 4), "ID: ", " ");
                }
                strRetVal = "Message Sent";

                lngCreditsUsed = lngMessagesToSend;
                bDataSent = true;
            }
            else
            {
                bDataSent = false;
                lngMessagesToSend = 0;
                switch (Strings.Left(strRetVal, 2))
                {
                    case "97":
                        strRetVal = "Other CGI parameter syntax error";
                        break;
                    default:
                        strRetVal = strRetVal + " - unknown response";
                        break;

                }
            }

            string Result1;
            if (m_bDistList == true)
            {
                strWhatSent = "TEXT";
                bHasCredit = true;
                vDLCount = Strings.Split(DeviceNr, ","); //// Added by PAS 10.10.02 to manage muli-single 
                lngDLElementCount = Information.UBound(vDLCount) + 1;
                if (lngDLElementCount == 1)
                {
                    vDLRetVal[0] = vDLRetVal[0] + " To: " + DeviceNr;

                }
                ////         // DeviceNr = m_strDistList + " (part " + m_lngCurrDistListIdx + " of " + m_lngMaxDistList + ")";
                strDistList = "1";
                if (bMultiSend == true)
                {
                    ////              //  DeviceNr = m_strDistList + " (" + lngDLElementCount + " Numbers)";
                }
                if (bDataSent == true)
                {
                    lngCreditsUsed = lngMessagesToSend;
                    // ErrorLog("lngCreditsUsed", Convert.ToString(lngCreditsUsed));
                }

                Result1 = await _vmsms.InsertNewSms(CompanyName, strWhatSent, DateTime.Now, strRetVal
                    , AuthUser, RequestingIP, lngMessagesToSend, lngCreditsUsed, strDistList, DeviceNr, Message, templateID, SelectedTeamID,
                    SelectedMsgTitleID, EmpID, CompanyID, CostCentre, Cttype);

                long iWriteDLCount = lngDLElementCount;

                for (rvCount = 0; rvCount < iWriteDLCount; rvCount++)
                {
                    if (vDLRetVal[rvCount] != "")
                    {
                        vRetValArray = Strings.Split(vDLRetVal[rvCount], "::");
                        NrSentTo = vRetValArray[0];
                        if (Strings.InStr(Strings.LCase(vRetValArray[1]), "success") > 0)
                        {
                            MsgStatus = "Delivered to SMSC";
                            MsgId = vRetValArray[2];
                        }
                        else if (Strings.InStr(Strings.LCase(vRetValArray[1]), "failed") > 0)
                        {
                            MsgStatus = "Failed";
                            MsgId = null;
                        }
                        else
                        {
                            MsgStatus = "Unknown";
                        }

                        SmsID = Result1;
                        var DateTimeResp = Strings.Format(DateTime.Now, "MM-dd-yyyy HH:mm:ss");

                        try
                        {
                            await _vmsms.InsertSMSDetail(SmsID, NrSentTo, MsgId, MsgStatus, DateTimeResp, EmpID, templateID, SelectedTeamID, SelectedMsgTitleID, Message);
                            Log.Logger.Information("SMS Details inserted into SMSDetail Table");
                        }
                        catch (Exception e)
                        {
                            Log.Logger.Error(e, e.Message);
                        }
                    }
                }
            }
            else
            {
                strDistList = "0";
                ptr = Strings.InStr(MsgId, " To: ");
                MsgId = Strings.Left(Strings.Replace(MsgId, " ", ""), 32);
                NrSentTo = DeviceNr;
                DeviceNr = "Single";
                if (strRetVal == "Message Sent")
                {
                    MsgStatus = "Delivered to SMSC";
                }
                else
                {
                    MsgStatus = "Failed";
                }

                Result1 = await _vmsms.InsertNewSms(CompanyName, strWhatSent, DateTime.Now, strRetVal
                    , AuthUser, RequestingIP, lngMessagesToSend, lngCreditsUsed, strDistList, DeviceNr, Message, templateID, SelectedTeamID,
                    SelectedMsgTitleID, EmpID, CompanyID, CostCentre, Cttype);

                SmsID = Result1;

                var DateTimeResp = Strings.Format(DateTime.Now, "MM-dd-yyyy HH:mm:ss");

                try
                {
                    await _vmsms.InsertSMSDetail(SmsID, NrSentTo, MsgId, MsgStatus, DateTimeResp, EmpID, templateID, SelectedTeamID, SelectedMsgTitleID, Message);
                    Log.Logger.Information("SMS Details inserted into SMSDetail Table");
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e, e.Message);
                }
            }

            return strRetVal;

        }

    }
}
