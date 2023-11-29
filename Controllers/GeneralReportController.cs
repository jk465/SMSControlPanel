using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using Serilog;
using SMSControlPanel.DataLayer;
using SMSControlPanel.Models;
using SMSControlPanel.Repository;
using SMSControlPanel.Repository.BusinessLogic;
using SMSControlPanel.Service;
using SMSControlPanel.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SMSControlPanel.Controllers
{
    [RoleAuthorize("User", "Admin", "SuperAdmin")]
    public class GeneralReportController : Controller
    {
        private readonly IDeptUserBL _dept;
        private readonly IGenerateReportBL _report;
        private readonly IMsgDetailsBL _msgDetails;
        private readonly IAuthService _authService;
      
        public GeneralReportController(IDeptUserBL dept, IMsgDetailsBL msgDetails, IGenerateReportBL report, IAuthService authService)
        {
            _dept = dept;
            _report = report;
            _msgDetails = msgDetails;
            _authService = authService;
        }
        public IActionResult Report()
        {
            var userviewmodel = _authService.GetUserModel();
            return View(userviewmodel);
        }

        [HttpPost]
        public async Task<JsonResult> GetDepartments(string DeptId)
        {
            try
            {
                var Departments = await _dept.GetDepartments(DeptId);
                return Json(Departments);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetDepartmentsForAdmin(string EmpId)
        {
            try
            {
                var Departments = await _dept.GetDepartmentsForAdmin(EmpId);
                return Json(Departments);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }
        

        [HttpPost]
        public async Task<JsonResult> GetDeptUsers(string DeptId)
        {
            try
            {
                var Users = await _dept.GetDeptUsers(DeptId);
                return Json(Users);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetAllDeptUsers([FromBody] dynamic DeptIds)
        {
            try
            {
                var JsonArray = JsonConvert.DeserializeObject<object>(DeptIds.ToString());

                var DeptArray = JsonArray.DeptIds;

                string JoinedDeptId = string.Join("','", DeptArray);
                JoinedDeptId += "'";

                string ValidString = JoinedDeptId.Substring(0, 3).Equals("','") ? JoinedDeptId[2..] : JoinedDeptId;

                var Users = await _dept.GetDeptUsers(ValidString);
                return Json(Users);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GenerateReport([FromBody] GenerateReportModel JsonData)
        {
            string CompanyIDStr = null;
            string EmpIDStr = null;
            try
            {
                //var JsonArray = JsonConvert.DeserializeObject<object>(JsonData.ToString());
                string FromDate = JsonData.FromDate;
                string ToDate = JsonData.ToDate;
                string[] EmpIds = JsonData.EmpIds;
                string[] DeptIds = JsonData.DeptIds;

                var EmpId = _authService.GetUserModel().EmpID;
                var Role = _authService.GetUserModel().Role;

                if (Role == 3 || Role == 2)
                {
                    CompanyIDStr = string.Join(",", DeptIds);
                    EmpIDStr = string.Join(",", EmpIds);
                }

                if (Role == 1)
                {
                    CompanyIDStr = "";
                    var ResultReport = await _report.GetReport(EmpId, Role, CompanyIDStr, FromDate, ToDate, EmpIds, 0);  //companyIDstr not used in sp

                    if (ResultReport != null)
                        return Json(ResultReport.Value);
                }
                if (Role == 2 || Role == 3)
                {
                    var ResultReport = await _report.GetAdminReport(EmpId, Role, CompanyIDStr, FromDate, ToDate, EmpIDStr, 0); //companyIDstr not used in sp
                    if (ResultReport != null)
                        return Json(ResultReport.Value);
                }

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }

            return Json(new { success = false, err = "Null reference error" });
        }

        [HttpPost]
        public async Task<JsonResult> GenerateSmsDetailReport([FromBody] GenerateReportModel JsonData)
        {
            try
            {
                //var JsonArray = JsonConvert.DeserializeObject<object>(JsonData.ToString());
                string FromDate = JsonData.FromDate;
                string ToDate = JsonData.ToDate;
                string[] EmpIds = JsonData.EmpIds;
                string[] DeptIds = JsonData.DeptIds;

                var EmpId = _authService.GetUserModel().EmpID;
                var Role = _authService.GetUserModel().Role;

                string CompanyIDStr;
                if (Role == 1)
                {
                    CompanyIDStr = "";
                    var ResultReport = await _report.GetReport(EmpId, Role, CompanyIDStr, FromDate, ToDate, EmpIds, 1);  //companyIDstr not used in sp

                    if (ResultReport != null)
                        return Json(ResultReport.Value);
                }
                if (Role == 2 || Role == 3)
                {
                    CompanyIDStr = string.Join(",", DeptIds);
                    string EmpIDStr = string.Join(",", EmpIds);

                    var ResultReport = await _report.GenerateDetailReportAdmins(EmpId, Role, CompanyIDStr, FromDate, ToDate, EmpIDStr, 1); //companyIDstr not used in sp
                    if (ResultReport != null)
                        return Json(ResultReport.Value);
                }

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
            return Json(new { success = false, err = "Null reference error" });
        }

        [HttpPost]
        public async Task<JsonResult> GetFailedMsgDetails(string FromDate, string ToDate, string EmpId)
        {
            try
            {
                var result = await _msgDetails.GetFailedMessages(FromDate, ToDate, EmpId);
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);

            }
            return Json(new { success = false, err = "Failed" });
        }

        [HttpPost]
        public async Task<JsonResult> GetMessageDetails(string smsID)
        {
            try
            {
                var result = await _msgDetails.GetMessageDetails(smsID);
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            return Json(new { success = false, err = "Failed" });
        }

        [HttpPost]
        public async Task<IActionResult> ExportSmsReport([FromBody] GenerateReportModel JsonData)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage excelPackage = new();
            string CompanyIDStr = null;
            string EmpIDStr = null;
            string SheetName = "SMS Usage";
            try
            {
                //var JsonArray = JsonConvert.DeserializeObject<object>(JsonData.ToString());
                string FromDate = JsonData.FromDate;
                string ToDate = JsonData.ToDate;
                string[] EmpIds = JsonData.EmpIds;
                string[] DeptIds = JsonData.DeptIds;

                var EmpId = _authService.GetUserModel().EmpID;
                var Role = _authService.GetUserModel().Role;

                if (Role == 3 || Role == 2)
                {
                    CompanyIDStr = string.Join(",", DeptIds);
                    EmpIDStr = string.Join(",", EmpIds);
                }

                if (Role == 1)
                {
                    var ResultReport = await _report.GetReport(EmpId, Role, CompanyIDStr, FromDate, ToDate, EmpIds, 0);  //companyIDstr not used in sp

                    if (ResultReport != null)
                    {
                        var temp = CommonClass.ConvertJsonToDataTable(ResultReport);
                        excelPackage = CommonClass.DatatableToExcel(temp, SheetName);
                    }

                }
                if (Role == 2 || Role == 3)
                {
                    var ResultReport = await _report.GetAdminReport(EmpId, Role, CompanyIDStr, FromDate, ToDate, EmpIDStr, 0); //companyIDstr not used in sp
                    if (ResultReport != null)
                    {
                        var temp = CommonClass.ConvertJsonToDataTable(ResultReport);
                        excelPackage = CommonClass.DatatableToExcel(temp, SheetName);
                    }

                }

                var stream = new MemoryStream(excelPackage.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportSmsDetailReport([FromBody] GenerateReportModel JsonData)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage excelPackage = new();
            string CompanyIDStr = null;
            string EmpIDStr = null;
            string SheetName = "SMS Usage Detail";
            try
            {
                //var JsonArray = JsonConvert.DeserializeObject<object>(JsonData.ToString());
                string FromDate = JsonData.FromDate;
                string ToDate = JsonData.ToDate;
                string[] EmpIds = JsonData.EmpIds;
                string[] DeptIds = JsonData.DeptIds;

                var EmpId = _authService.GetUserModel().EmpID;
                var Role = _authService.GetUserModel().Role;

                if (Role == 3 || Role == 2)
                {
                    CompanyIDStr = string.Join(",", DeptIds);
                    EmpIDStr = string.Join(",", EmpIds);
                }

                if (Role == 1)
                {
                    var ResultReport = await _report.GetReport(EmpId, Role, CompanyIDStr, FromDate, ToDate, EmpIds, 1);  //companyIDstr not used in sp

                    if (ResultReport != null)
                    {
                        var temp = CommonClass.ConvertJsonToDataTable(ResultReport);
                        excelPackage = CommonClass.DatatableToExcel(temp, SheetName);
                    }
                }
                if (Role == 2 || Role == 3)
                {
                    var ResultReport = await _report.GenerateDetailReportAdmins(EmpId, Role, CompanyIDStr, FromDate, ToDate, EmpIDStr, 1); //companyIDstr not used in sp
                    if (ResultReport != null)
                    {
                        var temp = CommonClass.ConvertJsonToDataTable(ResultReport);
                        excelPackage = CommonClass.DatatableToExcel(temp, SheetName);
                    }
                        
                }

                var stream = new MemoryStream(excelPackage.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }
    }
}
