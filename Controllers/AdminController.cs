using Microsoft.AspNetCore.Mvc;
using SMSControlPanel.Repository.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using SMSControlPanel.Models;
using Microsoft.AspNetCore.Http;
using SMSControlPanel.Repository;
using SMSControlPanel.Service;

namespace SMSControlPanel.Controllers
{
    [RoleAuthorize("SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly IAdminBL _admin;
        private readonly IAuthService _authService;
        public AdminController(IAdminBL admin, IAuthService authService)
        {
            _admin = admin;
            _authService = authService;

        }

        public async Task<IActionResult> ManageUsers()
        {
            var userviewmodel = _authService.GetUserModel();

            ViewBag.UserData = await _admin.LoadUsersData();

            return View(userviewmodel);
        }

        public async Task<JsonResult> LoadUsersData()
        {
            try
            {
                List<AdminModel> userData = await _admin.LoadUsersData();

                return Json(userData);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }

        }

        //public JsonResult LoadUsersData()
        //{
        //    try
        //    {
        //        List<AdminModel> userData = _admin.LoadUsersData();

        //        //var pagedData = ApplyPagination(userData, data);

        //        //var response = new {
        //        //    Draw = data.Draw,
        //        //    recordsTotal = data.Length,
        //        //    recordsFiltered = userData.Count,
        //        //    data = pagedData
        //        //};

        //        return Json(userData);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.Error(ex.Message);
        //        return Json(new { success = false, err = ex.Message });
        //    }

        //}

        //private static dynamic[] ApplyPagination(List<AdminModel> data, DataTableParameters parameters)
        //{
        //    var StartIndex = parameters.Start;
        //    var Length = parameters.Length;

        //    return data.Skip(StartIndex).Take(Length).ToArray();
        //}
        public async Task<JsonResult> SearchFirstLastName(string first, string last)
        {
            try
            {
                List<AdminModel> firstlast = await _admin.SearchFirstLastName(first, last);
                return Json(firstlast);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteUser(string empid, string update_by)
        {
            try
            {
                // Delete the user with the specified ID
                var result = await _admin.DeleteUser(empid, update_by);
                return Json(new { success = result });
            }
            catch (Exception ex)  //[SP_VMTP_USER_DELETE]
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateUser(AdminModel model) {
            try
            {
                var InsertBy = _authService.GetUserModel().EmpID;
                bool result = await _admin.UpdateUser(model,InsertBy);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> InsertUser(AdminModel model)
        {
            try
            {
                bool IsExist = await _admin.CheckExist(model);
                if (IsExist)
                    return Json(new { success = false });

                var InsertBy = _authService.GetUserModel().EmpID;
                bool result = await _admin.InsertUser(model, InsertBy);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }

        //[HttpPost]
        //public JsonResult CheckExist(AdminModel model)
        //{
        //    try
        //    {
        //        var InsertBy = GetUserViewModel().User.EmpID;
        //        bool result = _admin.CheckExist(model);
        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.Error(ex.Message);
        //        return Json(new { success = false, err = ex.Message });
        //    }
        //}

        [HttpPost]
        public async Task<JsonResult> GetTeamDetails(string DeptID)
        {
            try
            {
                List<TeamTemplateModel> Teams = await _admin.GetTeamDetails(DeptID);

                return Json(Teams);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);

                return Json(new { success = false, err = ex.Message });
            }
        }

        public async Task<JsonResult> GetRoleDetails()
        {
            try
            {
                List<RoleModel> Roles = await _admin.GetRoles();
                return Json(Roles);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }   

        [HttpPost]
        public async Task<JsonResult> GetColleagues(string firstname, string lastname)
        {
            try
            {
                List<ColleagueModel> Colleagues = await _admin.GetColleagues(firstname, lastname);
                return Json(Colleagues);

            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return Json(new { success = false, err = ex.Message });
            }
        }
    }
}

