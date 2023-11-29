using SMSControlPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.Repository.BusinessLogic
{
    public interface IGetDistListBL
    {
        Task<List<TemplateModel>> GetTemplates(string companyID);
        Task<List<TeamTemplateModel>> GetTeamTemplates(int? roleID, string teamID);
        Task<List<MessageTitleModel>> GetMessageTitle(int teamID);
        Task<string> GetMsgTemplate(int msgTitleID);
        Task<List<string>> GetDL(string companyID);
        Task<List<string>> GetPhonenumbers(string distlist, string companyID);
        Task<List<DLModel>> GetDLandEntries(string companyID);
        Task<bool> DeleteDL(string listName, string companyID);
        Task<bool> CheckExists(string listName, string company);
        Task<string> UploadTxtFile(string fileName, string username, string company, string companyID, string companyID1);
        Task<string> UploadCSVFile(string filePath, string listname, string username, string company, string companyID);
    }
}
