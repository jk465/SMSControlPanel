using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository.DataLayer
{
    public interface IGetDistListDL
    {
        Task<List<TemplateModel>> GetTemplates(string companyID);
        Task<List<TeamTemplateModel>> GetTeamTemplates(int roleID, int teamID);
        Task<List<MessageTitleModel>> GetMessageTitle(int teamID);
        Task<string> GetMsgTemplate(int msgTitleID);
        Task<List<string>> GetDL(string companyID);
        Task<List<string>> GetPhoneNumbers(string distlist, string companyID);
        Task<List<DLModel>> GetDLandEntries(string companyID);
        Task<bool> DeleteDL(string listName, string companyID);
        Task<bool> CheckExists(string listName, string company);
        Task<int> InsertDistList(string strSQL);
    }
}
