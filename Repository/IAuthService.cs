using Microsoft.AspNetCore.Mvc.Filters;
using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository
{
    public interface IAuthService 
    {
        string GetRoleName(int roleId);
        string GetFullEmpID();
        UserDetails GetUserModel();
        void LogUser();
        Task SetSessionValues();
    }
}
