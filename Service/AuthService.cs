using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using SMSControlPanel.Models;
using SMSControlPanel.Repository;
using SMSControlPanel.Repository.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserLogger;

namespace SMSControlPanel.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserLogonBL _user;
        private readonly string _FullEmpID;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserLogger _UserLogger;

        private readonly Dictionary<int, string> _roleMapping = new()
        {
            { 1, "User" },
            { 2, "Admin" },
            { 3, "SuperAdmin" },
        };
        public AuthService(IUserLogonBL user, IHttpContextAccessor httpContextAccessor, IUserLogger userLogger)
        {
            _user = user;
            _httpContextAccessor = httpContextAccessor;
            _UserLogger = userLogger;

             _FullEmpID = _httpContextAccessor.HttpContext.User.Identity.Name;
            _FullEmpID = @"SYSTEMS\s7905240";//k7905713
        }

        public string GetRoleName(int roleId)
        {
            var roleName = _roleMapping.GetValueOrDefault(roleId);
            return roleName;
        }

        public string GetFullEmpID()
        {
            return _FullEmpID;
        }
        public UserDetails GetUserModel()
        {
            var UserModel = new UserDetails();

            try
            {
                if (_httpContextAccessor.HttpContext.Session != null)
                {
                    var context = _httpContextAccessor.HttpContext.Session.GetString("User");
                    UserModel = JsonConvert.DeserializeObject<UserDetails>(context);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, e.Message);
            }

            return UserModel;
        }
        public void LogUser()
        {
            try
            {
                Log.Logger.Information($"Login User Details: {_FullEmpID}");

                var Request = _httpContextAccessor.HttpContext.Request;
                _ = _UserLogger.AddUserDetailsToDB(_FullEmpID, $"SMSControlPanel: {Request.Scheme}:/{Request.Host}", DateTime.Now.ToString());
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, e.Message);
            }
        }
        public async Task SetSessionValues()
        {
            try
            {
                var EmpID = _FullEmpID;
                if(_FullEmpID.Substring(0, 10).ToUpper() == "TROWBRIDGE")
                {
                    EmpID = await _user.GetTrowbridgeEmpID(_FullEmpID);
                }
                UserDetails user = await _user.GetDetails(EmpID);
                _httpContextAccessor.HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, e.Message);
            }
        }
    }
}
