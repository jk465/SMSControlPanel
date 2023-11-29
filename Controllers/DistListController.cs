using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Serilog;
using SMSControlPanel.Repository;
using SMSControlPanel.Repository.BusinessLogic;
using SMSControlPanel.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Controllers
{
    [RoleAuthorize("User", "Admin", "SuperAdmin")]
    public class DistListController : Controller
    {
        private readonly IGetDistListBL _distList;
        private readonly ISendSMS _sms;
        private readonly IAuthService _authService;
        public DistListController(IGetDistListBL distList, ISendSMS sms, IAuthService authService)
        {
            _distList = distList;
            _sms = sms;
            _authService = authService;
        }
        public IActionResult SendToDL()
        {
            var userviewmodel = _authService.GetUserModel();

            return View(userviewmodel);
        }

        [HttpPost]
        public async Task<JsonResult> LoadDL()
        {
            try
            {
                var CompanyID = _authService.GetUserModel().DEPARTMENTID;
                var DL = await _distList.GetDL(CompanyID);

                return Json(DL);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, e.Message);
                return Json(e.Message);
            }
        }
        [HttpPost]
        public async Task<JsonResult> SendSms(string message, string distlist, string team, string messagetitle, string template, string originator, string radcommunication)
        {
            string response = "";
            string SMSMessage = message;
            int ServiceType;
            string RetVal = null;
            string SMSType = "S"; //Set for Short Message
            string DeliveryMins = null;
            bool bHasCredit = false;
            bool bDataSent = false;

            string[] phArray = null;
            bool isValidOrig;

            string Originator = originator;
            string DeliveryDate = Convert.ToString(DateTime.Now);

            string EmpID = _authService.GetUserModel().EmpID;
            int Role = Convert.ToInt16(_authService.GetUserModel().Role);
            string test = template;
            string teamID = team;
            string msgtitle = messagetitle;


            string Username = _authService.GetUserModel().FIRSTNAME;
            string CompanyName = _authService.GetUserModel().CompanyName;
            int CompanyID = Convert.ToInt32(_authService.GetUserModel().DEPARTMENTID);
            string CostCentre = _authService.GetUserModel().COSTCENTRE.Trim();
            int DeptidSMS = Convert.ToInt32(_authService.GetUserModel().DEPARTMENTID);

            int templateID;
            int SelectedTeamID;
            int SelectedMsgTitleID;
            string CommunicationType = radcommunication;
            bool Cttype = true;

            if (test == "5")
            {
                templateID = 5;
                SelectedTeamID = 0;
                SelectedMsgTitleID = 0;
            }

            try
            {
                _ = int.TryParse(template, out templateID);
                _ = int.TryParse(team, out SelectedTeamID);
                _ = int.TryParse(messagetitle, out SelectedMsgTitleID);
            }
            catch (Exception ex)
            {
                templateID = 5;
                SelectedTeamID = 0;
                SelectedMsgTitleID = 0;
                Log.Logger.Error(ex.Message);
            }

            try
            {
                List<string> phlist = await _distList.GetPhonenumbers(distlist, CompanyID.ToString());
                if(phlist.Count == 0)
                {
                    response = "Distribution Lists is empty";
                    return Json(new { success = false, err = response });
                }
                phArray = phlist.ToArray();
                isValidOrig = ValidateDistListOriginator(phArray, Originator);
                if (isValidOrig == false)
                {
                    response = "One or more of the Phone No(s) in the distribution list - '" + distlist + "' is a landline number. Please enter only digits in the 'originator' field";
                    return Json(new { success = false, err = response });
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            if (SMSMessage.Length > 0)
            {
                switch (SMSType) //Other SMS types are not handled here
                {
                    case "S":
                        ServiceType = 5510;
                        try
                        {
                            RetVal = await SendSMSToDL(CompanyID, CompanyName, SMSMessage, SMSType, distlist, phArray, Originator,
                                Username, HttpContext.Connection.RemoteIpAddress.ToString(),
                                bDataSent, bHasCredit, EmpID, templateID, SelectedTeamID,
                                SelectedMsgTitleID, Cttype, CostCentre, ServiceType);

                            response = RetVal;
                        }

                        catch (Exception ex)
                        {
                            Log.Logger.Error(ex.Message);
                        }
                        break;
                }
            }
            if (RetVal.Length > 0)
            {
                response = RetVal;
                Log.Logger.Information($"{response} from SendSMS To DL");
                return Json(new { success = true, response });
            }
            return Json(new { success = false, err = "Message send to failed" });
        }

        private async Task<string> SendSMSToDL(long companyID, string companyName, string message, string smsType, string distlist, string[] phArray, string originator, 
                                    string username, string IP, bool DataSent, bool HasCredit, string empID, int templateID, 
                                    int selectedTeamID, int selectedMsgTitleID, bool cttype, string costCentre, int serviceType)
        {
            bool bHasCredit = true;
            bool m_bDistList = true;
            long lngMessagesToSend;
            string strRetVal = null;
            string m_strDistList = Strings.Replace(distlist, "'", "''");
            string DeliveryDate = null;
            string DeliveryMins = null;
            
            string DeviceNr = string.Join(",", phArray);



            if (bHasCredit)
            {
                strRetVal = await _sms.SendSMSMessage(companyID, companyName, message, smsType, DeviceNr, originator,
                                    username, HttpContext.Connection.RemoteIpAddress.ToString(),
                                     DataSent, HasCredit, DeliveryDate, empID, templateID, selectedTeamID,
                                    selectedMsgTitleID, cttype, costCentre, DeliveryMins, serviceType);


                if (Strings.InStr(Strings.LCase(strRetVal), "success") > 0)
                {
                    strRetVal = "Message sent";
                    return strRetVal;
                }
            }
            return strRetVal;
        }

        private bool ValidateDistListOriginator(string[] phArray, string originator)
        {
            int lng = 0, lCnt, lngCount;
            string DeviceNo;
            bool validNo;
            lngCount = Information.UBound(phArray);
            validNo = true;
            for (lng = 0; lng < lngCount; lng++)
            {
                DeviceNo = TidyUK(Strings.Trim(phArray[lng]));
                if ((DeviceNo.Substring(0, 2) != "447"))
                {
                    for (lCnt = 0; lCnt < originator.Length; lCnt++)
                    {
                        validNo = DeviceNo.All(char.IsDigit);
                    }
                }
            }
            return validNo;
        }

        private string TidyUK(string DeviceNo)
        {
            //// remove 0 from UK numbers 
            if (Strings.Len(DeviceNo) > 3)
            {
                if (Strings.Left(DeviceNo, 3) == "440")
                {
                    DeviceNo = "44" + Strings.Mid(DeviceNo, 4, Strings.Len(DeviceNo) - 3);
                }
                //// allow for thick UK users you can't add a country code
                if (Strings.Left(DeviceNo, 1) == "0")
                {
                    DeviceNo = "44" + Strings.Mid(DeviceNo, 2, Strings.Len(DeviceNo) - 1);
                }
            }
            return DeviceNo;
        }
    }
}
