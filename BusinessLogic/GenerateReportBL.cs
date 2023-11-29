using Microsoft.AspNetCore.Mvc;
using SMSControlPanel.Models;
using SMSControlPanel.Repository.BusinessLogic;
using SMSControlPanel.Repository.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.BusinessLogic
{
    public class GenerateReportBL : IGenerateReportBL
    {
        private readonly IGenerateReportDL _report;
        public GenerateReportBL(IGenerateReportDL report)
        {
            _report = report;
        }

        public async Task<JsonResult> GetReport(string empID, int role, string companyID, string fromDate, string toDate, string[] empList, int checkDetail)
        {
            if (companyID == null)
                companyID = "";

            var ValidFromDate = DateTime.TryParse(fromDate, out DateTime FromDate);
            var ValidToDate = DateTime.TryParse(toDate, out DateTime ToDate);

            if(ValidFromDate && ValidToDate)
            {
                var ResultReport = await _report.GetReport(empID, role, companyID, FromDate, ToDate, empList, checkDetail);

                return ResultReport;
            }

            return null;
        }

        public async Task<JsonResult> GetAdminReport(string empID, int role, string companyID, string fromDate, string toDate, string empList, int checkDetail)
        {
            var ValidFromDate = DateTime.TryParse(fromDate, out DateTime FromDate);
            var ValidToDate = DateTime.TryParse(toDate, out DateTime ToDate);

            if (ValidFromDate && ValidToDate)
            {
                var ResultReport = await _report.GetAdminReport(empID, role, companyID, FromDate, ToDate, empList, checkDetail);

                return ResultReport;
            }
            return null;
        }

        public async Task<JsonResult> GenerateDetailReportAdmins(string empID, int role, string companyID, string fromDate, string toDate, string empList, int checkDetail)
        {
            var ValidFromDate = DateTime.TryParse(fromDate, out DateTime FromDate);
            var ValidToDate = DateTime.TryParse(toDate, out DateTime ToDate);

            if (ValidFromDate && ValidToDate)
            {
                var ResultReport = await _report.GenerateDetailReportAdmins(empID, role, companyID, FromDate, ToDate, empList, checkDetail);

                return ResultReport;
            }
            return null;
        }

        public async Task<List<string>> GetMonth()
        {
           List<string> months = await _report.GetMonth();
            return months;
        }

        public async Task<List<ExcelModel>> GetExcelData(int month, int year)
        {

            List<ExcelModel> result = await _report.GetExcelData(month, year);
            return result;
        }
    }
}
