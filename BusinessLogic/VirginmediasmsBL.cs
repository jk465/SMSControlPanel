using Microsoft.VisualBasic;
using SMSControlPanel.Repository.BusinessLogic;
using SMSControlPanel.Repository.DataLayer;
using System;
using System.Linq;
using System.Threading.Tasks;
using VMSMSRestService;

namespace SMSControlPanel.BusinessLogic
{
    public class VirginmediasmsBL : IVirginmediasmsBL
    {
        private readonly IVirginmediasmsDL _vmsms;
        public VirginmediasmsBL(IVirginmediasmsDL vmsms) 
        {
            _vmsms = vmsms;
        }

        public const string TELEWEST_SMTP = "192.168.0.40";
        public const string TELEWEST_WAP_GATEWAY = "192.168.0.35";
        public const string TELEWEST_DIAL_POP = "448450110061";
        public const string TELEWEST_SUPPORT = "support@Telewestdev.com";
        public const string TELEWEST_INFO = "info@Telewestdev.com";
        public const string TELEWEST_WAP_PHONE_INFO = "wapphoneinfo@Telewestdev.com";
        public const string TELEWEST_REGISTRATION = "registration@Telewestdev.com";
        public const string TELEWEST_NEW_CUSTOMER = "newcustomer@Telewestdev.com";
        public const string TELEWEST_RADIUS_FAILED = "RadiusRegFailure@Telewestdev.com";
        public const string TELEWEST_OTA_CONFIG_FAILED = "OTAConfigFailure@Telewestdev.com";
        public const string TELEWEST_RADIUS_SUCCESS = "RadiusRegSuccess@Telewestdev.com";
        public const string TELEWEST_OTA_CONFIG_SUCCESS = "OTAConfigSuccess@Telewestdev.com";
        public const string TELEWEST_WML_HOME = "http://www.Telewestdev.com/myprofile.asp";
        public const string TELEWEST_WML_ENGINE = "/engine.asp?";

        //            Private m_oCtxt              As ObjectContext;

        // Used to distinguish between DB Items and non-DB Items
        private bool m_bDBRequest;
        private string m_strDataToSend;

        // Distribution List
        private bool m_bDistList;
        private string m_strDistList;
        private long m_lngCurrDistListIdx;
        private long m_lngMaxDistList;
        private long m_lngDevices;
        //private string  m_strDevices();
        private const string OP_HEADER_1 = "|010706050415820000|";
        private const string OP_HEADER_2 = "00480E01";
        private const string GRP_HEADER = "|010706050415830000|00480E01";
        Guid g;
        private void Class_Initialize()
        {
            m_bDistList = false;
            m_bDBRequest = true;
        }

        public string FormatDate(ref DateTime DateIn)
        {

            // Get date-only portion of date, without its time.
            DateTime dateOnly = DateIn.Date;

            string s = null;
            s = dateOnly.ToString("MM-dd-yyyy");


            return s;

        }


        public static string FormatDateAndTime(DateTime DateIn)
        {

            // Get date-only portion of date, without its time.
            DateTime dateOnly = DateIn.Date;

            string s = null;
            s = DateIn.ToString("MM-dd-yyyy HH:mm:ss");


            return s;
        }


        public string LeadingZeros(int ExpectedLen, int ActualLen)
        {

            string pattern = "0";
            int leadzero = (ExpectedLen - ActualLen);
            string result = String.Concat(Enumerable.Repeat(pattern, leadzero).ToArray());
            result = result.Substring(0, leadzero);
            return result;
        }
        public string GUID_Generate()
        {
            string getGUID;
            try
            {
                Guid lResult;
                string MyguidString = null;
                string MyGuidString1;
                string MyGuidString2;
                string MyGuidString3;

                int DataLen;
                int StringLen;
                int i = 0;
                int Data1;
                short Data2;
                short Data3;
                byte[] Data4;
                string data1str;

                lResult = Guid.NewGuid();
                byte[] gBytes = lResult.ToByteArray();
                Data1 = BitConverter.ToInt32(gBytes, 0);
                Data2 = BitConverter.ToInt16(gBytes, 4);
                Data3 = BitConverter.ToInt16(gBytes, 6);
                Data4 = new Byte[8];
                Buffer.BlockCopy(gBytes, 8, Data4, 0, 8);

                MyGuidString1 = String.Format("{0:X}", Data1);
                StringLen = MyGuidString1.Length;
                data1str = Data1.ToString();
                DataLen = data1str.Length;
                MyGuidString1 = LeadingZeros(2 * DataLen, StringLen) + MyGuidString1;


                MyGuidString2 = String.Format("{0:X}", Data2);
                StringLen = MyGuidString2.Length;
                string data2str;
                data2str = Data2.ToString();
                MyGuidString2 = LeadingZeros(2 * DataLen, StringLen) + MyGuidString2.Trim();

                MyGuidString3 = String.Format("{0:X}", Data3);
                StringLen = MyGuidString3.Length;
                string data3str;
                data3str = Data3.ToString();
                MyGuidString3 = LeadingZeros(2 * DataLen, StringLen) + MyGuidString3.Trim();


                getGUID = MyGuidString1 + MyGuidString2 + MyGuidString3;

                for (i = 0; i <= 7; i++)
                {
                    MyguidString = MyguidString + string.Format(String.Format("{0:X}", Data4), "00");
                }

                getGUID = getGUID + MyguidString;
                getGUID = getGUID.ToLower();
                return getGUID;
            }
            catch
            {
                getGUID = "00000000";
                return getGUID;
            }


        }
        int IDFromHex(string HexID)
        {
            return int.Parse(HexID, System.Globalization.NumberStyles.HexNumber);
        }

        public string ValidateDeviceNr(ref string DeviceNr, ref long ElementCount, ref bool DistList, ref string RetVal)
        {
            string[] vDevices;
            string VarRecord2;
            int iLen;
            long lng;
            long lngCount;
            string invalidNumbers = null;
            DeviceNr = TidyDeviceNr(DeviceNr);
            DeviceNr = DeviceNr.Replace(";", ",");
            DeviceNr = DeviceNr.Replace("\r\n", ",");
            string checklast;


            checklast = DeviceNr.Substring(DeviceNr.Length - 1, 1);

            if (checklast == ",")
            {
                DeviceNr = DeviceNr.Substring(0, DeviceNr.Length - 1);
            }
            vDevices = DeviceNr.Split(",".ToCharArray());
            ElementCount = vDevices.Count();
            if (DistList = false && ElementCount > 500)
            {
                RetVal = "Too many Phone numbers, max. 500 per request";
                return RetVal;
            }

            lngCount = vDevices.Count();



            //string connectionString = @"Data Source=WBV-SQL-T1V16.presystems.private\inst16;Initial Catalog=TelewestSMS_new;Integrated Security=False;User ID=vmtpuser;Password=1qaz2wsx3edc";
            //SqlConnection objconn = Connection.OpenConnection();

            //ErrorLog("query string", queryString);
            
                VarRecord2 =  _vmsms.GetCompanyName().Result;

                if (VarRecord2 == "mblox")
                {
                    for (lng = 0; lng < lngCount; lng++)
                    {
                        if (vDevices[lng].Substring(0, 2) != "00")
                        {

                            RetVal = "00 Missing from Number, " + vDevices[lng];
                            return RetVal;
                        }
                        // ErrorLog("vDevices[lng]", Convert.ToString(vDevices[lng]));
                        iLen = vDevices[lng].Length;
                        //  ErrorLog("ilen", Convert.ToString(iLen));
                        if (iLen <= 10 || iLen > 14)
                        {

                            RetVal = "Invalid mobile device number found " + vDevices[lng];
                            return RetVal;
                        }
                    }

                }

                else
                {
                    for (lng = 0; lng < lngCount; lng++)
                    {

                        iLen = vDevices[lng].Length;
                        if (iLen <= 10 || iLen > 14)
                        {
                            invalidNumbers += Convert.ToString(vDevices[lng]) + ",";
                        }
                    }
                    if (invalidNumbers != null)
                    {
                        if (Strings.Right(invalidNumbers, 1) == ",")
                        {
                            invalidNumbers = Strings.Left(invalidNumbers, Strings.Len(invalidNumbers) - 1);
                        }
                        RetVal = "Invalid mobile device number found: " + invalidNumbers;
                        return RetVal;
                    }

                }
                
              //  ErrorLog("RetVal", RetVal);
                return RetVal;
            
        }

        public static string TidyDeviceNr(string DeviceNr)
        {
            string strNr;
            strNr = DeviceNr.Trim();
            strNr = strNr.Replace(" ", "");
            strNr = strNr.Replace("+", "");
            strNr = strNr.Replace("-", "");
            strNr = strNr.Replace(".", "");

            return strNr;

        }

        public async Task<string> InsertNewSms(string CompanyName, string WhatSent, DateTime _DateTimeSent, string Result, string Sentby, string IP,
            long _MessagesSent, long _CreditUsed, string DL, string DistListCount, string _Message, int TemplateID, int SelectedTeamID,
            int SelectedMsgTitleID, string EmpId, long _CompanyID, string CostCentre, bool _Cttype)
        {
            string DateTimeSent = _DateTimeSent.ToString("MM - dd - yyyy HH: mm:ss");
            string MessagesSent = _MessagesSent.ToString();
            string CreditUsed = _CreditUsed.ToString();
            string CompanyID = _CompanyID.ToString();
            string Cttype = _Cttype.ToString();
            string Message = _Message.Replace("'", "");

            string response = await _vmsms.InsertNewSms(CompanyName, WhatSent, DateTimeSent, Result
                    , Sentby, IP, MessagesSent, CreditUsed, DL, DistListCount, Message, TemplateID, SelectedTeamID,
                    SelectedMsgTitleID, EmpId, CompanyID, CostCentre, Cttype);


            return response;
        }

        public async Task<string[]> SendMessage(SMSDetail sms)
        {
            ConfigurationDetails smsconfig = await _vmsms.GetSMSConfigurations();

            var response = await SMSSender.SendMessage(sms, smsconfig);

            return response;
        }

        public string[] IsvalidData(ref string originator, long serviceType, string message, string deviceNr)
        {
            string[] result = new string[1];

            if (originator == null | originator == "")
            {
                originator = "TMM";
            }

            if(serviceType == 0)
            {
                result[0] = "ERROR No Service_Type specified";
                return result;
            }
            
            if(message == null | message == "")
            {
                result[0] = "ERROR - No message specified";
                return result;
            }
            
            if(deviceNr == null | deviceNr == "")
            {
                result[0] = "ERROR - No destination number specified";
                return result;
            }

            return null;
        }

        public async Task InsertSMSDetail(string smsID, string nrSentTo, string msgId, string msgStatus, string dateTimeResp, string empID,
            int templateID, int selectedTeamID, int selectedMsgTitleID, string message)
        {
           await _vmsms.InsertSMSDetail(smsID, nrSentTo, msgId, msgStatus, dateTimeResp, empID, templateID, selectedTeamID, selectedMsgTitleID, message);
        }
    }
}
