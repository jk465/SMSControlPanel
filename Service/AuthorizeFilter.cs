using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using SMSControlPanel.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Service
{
    public class RoleAuthorizeAttribute : TypeFilterAttribute
    {
        public RoleAuthorizeAttribute(params string[] roles) : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { roles };
        }
    }
    public class AuthorizeFilter : IAuthorizationFilter
    {
        private readonly IAuthService _authService;
        private readonly string[] _roles;
        public AuthorizeFilter(IAuthService authService, params string[] roles)
        {
            _authService = authService;
            _roles = roles;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var UserModel = _authService.GetUserModel();

            if (UserModel != null)
            {
                var RoleName = _authService.GetRoleName(UserModel.Role);

                bool isAuthorized = _roles.Contains(RoleName);

                if (!isAuthorized)
                {
                    Log.Logger.Warning("UnAuthorized");
                    context.Result = new RedirectToActionResult("Unauthorised", "Base", null);
                }
            }
            else
            {
                Log.Logger.Warning("Either UnAuthorized or Session Expired");
                context.Result = new RedirectToActionResult("Unauthorised", "Base", null);
            }
        }
    }
}
