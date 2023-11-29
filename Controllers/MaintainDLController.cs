using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
using static System.Net.WebRequestMethods;

namespace SMSControlPanel.Controllers
{
    [RoleAuthorize("Admin", "SuperAdmin")]
    public class MaintainDLController : Controller
    {
        private readonly IGetDistListBL _distlist;
        private readonly IHostEnvironment _environment;
        private readonly IAuthService _authService;
        public MaintainDLController(IGetDistListBL distlist, IAuthService authService, IHostEnvironment environment)
        {
            _distlist = distlist;
            _environment = environment;
            _authService = authService;
        }
        public IActionResult DistList()
        {
            var userviewmodel = _authService.GetUserModel();
            return View(userviewmodel);
        }

        public IActionResult CreateDL()
        {
            var userviewmodel = _authService.GetUserModel();
            return View(userviewmodel);
        }

        public IActionResult UpdateDL(string ListName)
        {
            ViewData["ListName"] = ListName;
            var userviewmodel = _authService.GetUserModel();
            return View(userviewmodel);
        }

        public IActionResult ViewDL(string ListName)
        {
            ViewData["ListName"] = ListName;
            var userviewmodel = _authService.GetUserModel();
            return View(userviewmodel);
        }

        public async Task<JsonResult> LoadDL()
        {
            try
            {
                var CompanyID = _authService.GetUserModel().DEPARTMENTID;

                List<DLModel> result = await _distlist.GetDLandEntries(CompanyID);

                return Json(result);

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteDL(string ListName)
        {
            try
            {
                var CompanyID = _authService.GetUserModel().DEPARTMENTID;
                bool result = await _distlist.DeleteDL(ListName, CompanyID);

                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CheckExists(string ListName)
        {
            try
            {
                var Company = _authService.GetUserModel().CompanyName;
                bool IsExists = await _distlist.CheckExists(ListName, Company);
                return Json(new { success=IsExists });
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UploadFile()
        {
            try
            {
                string response = null;

                var username = _authService.GetUserModel().EmpID;
                var company = _authService.GetUserModel().CompanyName;
                var companyID = _authService.GetUserModel().DEPARTMENTID;

                var file = Request.Form.Files[0];
                var listname = Request.Form["text"].ToString();

                string DirectoryPath = _environment.ContentRootPath + @"\Uploads";
                var fileName = file.FileName;
                var filePath = Path.Combine(DirectoryPath, fileName);

                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);

                    fileStream.Close();
                    fileStream.Dispose();
                }
                    

                if (fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    response = await _distlist.UploadTxtFile(filePath, listname, username, company, companyID);
                }
                else
                {
                    response = await _distlist.UploadCSVFile(filePath, listname, username, company, companyID);
                }

                return Json(response);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetPhoneNumbers(string ListName)
        {
            try
            {
                var companyID = _authService.GetUserModel().DEPARTMENTID;

                List<string> PhNumbers = await _distlist.GetPhonenumbers(ListName, companyID); //retreiving from query instead of actual SP

                return Json(PhNumbers);

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> ExportDL()
        {
            try
            {
                string SheetName = "DL List";
                var CompanyID = _authService.GetUserModel().DEPARTMENTID;

                List<DLModel> result = await _distlist.GetDLandEntries(CompanyID);

                var excelPackage = CommonClass.ListToExcel<DLModel>(result,SheetName);

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
