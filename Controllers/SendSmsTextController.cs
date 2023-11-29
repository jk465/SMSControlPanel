using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using SMSControlPanel.Repository.BusinessLogic;
using SMSControlPanel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using SMSControlPanel.Repository;
using Serilog;
using SMSControlPanel.Service;
using System.Threading.Tasks;

namespace SMSControlPanel.Controllers
{
    [RoleAuthorize("User", "Admin", "SuperAdmin")]
    public class SendSmsTextController : Controller
    {
        private readonly IDeptUserBL _dept;
        private readonly IGetDistListBL _distList;
        private readonly ISendSMS _sms;
        private readonly IAuthService _authService;
        private readonly IUserLogonBL _user;
        public SendSmsTextController(IDeptUserBL dept, IGetDistListBL distList, IAuthService authService, IUserLogonBL user,
             ISendSMS sms)
        {
            _dept = dept;
            _distList = distList;
            _sms = sms;
            _authService = authService;
            _user = user;
        }

        public IActionResult Index()
        {
            var userviewmodel = _authService.GetUserModel();

            return View(userviewmodel);
        }

        [HttpPost]
        public async Task<JsonResult> LoadTemplate()
        {
            try
            {
                var CompanyID = _authService.GetUserModel().DEPARTMENTID;

                if (CompanyID == null)
                    RedirectToAction("SessionExpired");

                var templates = await _distList.GetTemplates(CompanyID);

                return Json(templates);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, e.Message);
                return Json(e.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetDept(UserDetails user)
        {
            try
            {
                string Department;

                if (_authService.GetFullEmpID().Substring(0, 10).ToUpper() == "TROWBRIDGE")
                {
                    Department = user.CompanyName;
                }
                else
                {
                    var deptId = Convert.ToInt32(user.DEPARTMENTID);
                    Department = await _dept.GetDeptDetails(deptId);
                }

                return Json(Department);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, e.Message);
                return Json(e.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetOriginatorPrivilege(string DeptID, string DeptName, string CostCentre)
        {
            try
            {
                var OriginatorPrivilege = await _user.GetOriginatorPrivilege(DeptID, DeptName, CostCentre);
                return Json(OriginatorPrivilege);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, e.Message);
                return Json(e.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> LoadTeamTemplate(int RoleID, string TeamID)
        {
            try
            {
                List<TeamTemplateModel> TeamTemplate = await _distList.GetTeamTemplates(RoleID, TeamID);
                return Json(TeamTemplate);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, e.Message);
                return Json(e.Message);
            }
        }
        [HttpPost]
        public async Task<JsonResult> LoadMessageTitle(int TeamID)
        {
            try
            {
                List<MessageTitleModel> MessageTitle = await _distList.GetMessageTitle(TeamID);
                return Json(MessageTitle);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, e.Message);
                return Json(e.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> LoadMsgTemplate(int MsgTitleID)
        {
            try
            {
                string MsgTemplate = await _distList.GetMsgTemplate(MsgTitleID);
                return Json(MsgTemplate);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, e.Message);
                return Json(e.Message);
            }
        }
        [HttpPost]
        public async Task<JsonResult> SendSms(string message, string phnumber, string team, string messagetitle, string template, string originator, string radcommunication)
        {
            string response = "";
            string SMSMessage = message;
            int ServiceType;
            string RetVal = null;
            string SMSType = "S"; //Set for Short Message
            string DeliveryMins = null;
            bool bHasCredit = false;

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
            string DeviceNr = phnumber.Trim();
            int DeptidSMS = Convert.ToInt32(_authService.GetUserModel().DEPARTMENTID);

            int templateID;
            int SelectedTeamID;
            int SelectedMsgTitleID;
            string CommunicationType = radcommunication;
            bool Cttype = true;

            try
            {
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
                    Log.Logger.Error(ex.Message);
                    templateID = 5;
                    SelectedTeamID = 0;
                    SelectedMsgTitleID = 0;
                }

                if ((Username == "") || (SMSType == "") || (DeviceNr == ""))
                {
                    return Json(new { success = false, err = "Invalid User" });
                }
                ValidateDistList(DeviceNr, out string[] vMembers, out int lngMembers, out string DeviceNR);
                DeviceNr = DeviceNR;
                if (lngMembers > 100)
                {
                    response = "Too many recipient numbers provided, (" + Convert.ToString(lngMembers) + " provided) max 100";
                    return Json(new { success = false, err = response });
                }
                if (Strings.Right(DeviceNr, 1) == ",")
                {
                    DeviceNr = Strings.Left(DeviceNr, Strings.Len(DeviceNr) - 1);
                }
                switch (SMSType)
                {
                    case "S":
                        if (SMSMessage == "")
                        {
                            response = "No SMS Message provided";
                            return Json(new { success = false, err = response });

                        }

                        if (Originator == "")
                        {
                            response = "No Originator provided";
                            return Json(new { success = false, err = response });
                        }
                        break;
                    default:
                        break;
                }

                if (SMSMessage.Length > 0)
                {
                    switch (SMSType) //Other SMS types are not handled here
                    {
                        case "S":
                            ServiceType = 5510;
                            try
                            {
                                bool bDataSent = false;
                                RetVal = await _sms.SendSMSMessage(CompanyID, CompanyName, SMSMessage, SMSType, DeviceNr, Originator,
                                    Username, HttpContext.Connection.RemoteIpAddress.ToString(),
                                    bDataSent, bHasCredit, DeliveryDate, EmpID, templateID, SelectedTeamID,
                                    SelectedMsgTitleID, Cttype, CostCentre, DeliveryMins, ServiceType);
                            }

                            catch (Exception ex)
                            {
                                Log.Logger.Error(ex.Message);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            if (RetVal.Length > 0)
            {
                response = RetVal;
                Log.Logger.Information($"{response} from SendSMS");
                return Json(new { success = true, response });
            }
            return Json(new { success = false, err = "Message send to failed" });
        }
        public object ValidateDistList(string s, out string[] vMembers, out int lngMembers, out string DeviceNR)
        {
            int lng = 0;
            int lngCount = 0;
            string[] v1 = null;
            string[] v2 = null;
            int v2Count = 0;
            int ptr = 0;
            string DeviceNo = null;
            string t = null;
            try
            {
                if(s.EndsWith(","))
                    s = Strings.Left(s, Strings.Len(s) - 1);
                //// Remove trailing LF and CRLF 
                while (Strings.Asc(Strings.Right(s, 1)) == 10 | Strings.Asc(Strings.Right(s, 1)) == 13)
                {
                    s = Strings.Left(s, Strings.Len(s) - 1);
                }
                s = Strings.Replace(s, ",", Constants.vbCrLf);
                v1 = Strings.Split(s, Constants.vbCrLf);
                ptr = 0;
                s = ""; lngCount = Information.UBound(v1);
                for (lng = 0; lng <= lngCount; lng++)
                {
                    v1[lng] = Strings.Trim(v1[lng]);

                    //// copy to another array removing blank lines 
                    if (v1[lng] != null && v1[lng] != "")
                    {
                        v2Count++;
                        Array.Resize(ref v2, v2Count + 1);
                        ptr = Strings.InStr(v1[lng], ",");
                        if (ptr == 0)
                        {
                            v1[lng] = v1[lng].Trim();
                            DeviceNo = TidyUK(ref v1[lng]);
                            v2[v2Count] = DeviceNo + ",";
                        }
                        else
                        {
                            //// trim name and number 
                            v1[lng] = Strings.Mid(v1[lng], 1, ptr - 1).Trim();

                            DeviceNo = TidyUK(ref v1[lng]);
                            if (Strings.Right(v1[lng], 1) != ",")
                            {
                                t = DeviceNo + "," + Strings.Trim(Strings.Mid(v1[lng], ptr + 1, Strings.Len(v1[lng]) - ptr));
                            }
                            else
                            {
                                t = DeviceNo + ",";
                            }
                            v2[v2Count] = t;
                        }
                        s += v2[v2Count];
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            finally
            {
            }
            vMembers = v1;
            lngMembers = Information.UBound(v2);
            DeviceNR = s;
            return Information.UBound(v2);
        }
        public string TidyUK(ref string DeviceNo)
        {
            //// remove 0 from UK numbers
            try
            {
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
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            finally
            {
            }

            return DeviceNo;
        }

    }
}
