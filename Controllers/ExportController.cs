using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Serilog;
using SMSControlPanel.DataLayer;
using SMSControlPanel.Models;
using SMSControlPanel.Repository;
using SMSControlPanel.Repository.BusinessLogic;
using SMSControlPanel.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Controllers
{
    [RoleAuthorize("SuperAdmin")]
    public class ExportController : Controller
    {
        private readonly IGenerateReportBL _report;
        private readonly IAuthService _authService;
        public ExportController( IGenerateReportBL report,IAuthService authService)
        {
            _report = report;
            _authService = authService;
        }
        public IActionResult ExportMonthly()
        {
            var userviewmodel = _authService.GetUserModel();
            return View(userviewmodel);
        }

        public async Task<JsonResult> GetMonths()
        {
            try
            {
                List<string> months = await _report.GetMonth();
                return Json(months);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExcelExport(int Month, int Year)
        {
            try
            {
                string SheetName = "SMS Usage";
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage excelPackage = new();

                List<ExcelModel> result = await _report.GetExcelData(Month, Year);

                excelPackage = CommonClass.ListToExcel<ExcelModel>(result,SheetName);

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
