using Microsoft.AspNetCore.Mvc;
using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository.DataLayer
{
    public interface IGenerateReportDL
    {
        Task<JsonResult> GetReport(string empID, int role, string companyID,
            DateTime fromDate, DateTime toDate, string[] empList, int checkDetail);
        Task<JsonResult> GetAdminReport(string empID, int role, string companyID,
            DateTime fromDate, DateTime toDate, string empList, int checkDetail);
        Task<JsonResult> GenerateDetailReportAdmins(string empID, int role, string companyID,
            DateTime fromDate, DateTime toDate, string empList, int checkDetail);
        Task<List<string>> GetMonth();
        Task<List<ExcelModel>> GetExcelData(int month, int year);
    }
}
