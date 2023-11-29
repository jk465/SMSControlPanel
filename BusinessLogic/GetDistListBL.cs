using Microsoft.VisualBasic;
using SMSControlPanel.Models;
using SMSControlPanel.Repository.BusinessLogic;
using SMSControlPanel.Repository.DataLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SMSControlPanel.BusinessLogic
{
    public class GetDistListBL : IGetDistListBL
    {
        private readonly IGetDistListDL _distList;
        public GetDistListBL(IGetDistListDL distList)
        {
            _distList = distList;
        }
        public async Task<List<TemplateModel>> GetTemplates(string companyID)
        {
            List<TemplateModel> distList = await _distList.GetTemplates(companyID);
            return distList;
        }

        public async Task<List<TeamTemplateModel>> GetTeamTemplates(int? roleID, string teamID)
        {
            
            int TeamID = string.IsNullOrEmpty(teamID) ? 0 : Convert.ToInt32(teamID);
            int RoleID = string.IsNullOrEmpty(roleID.ToString()) ? 0 : Convert.ToInt32(roleID);

            List<TeamTemplateModel>  distList = await _distList.GetTeamTemplates(RoleID, TeamID);

            return distList;
        }

        public async Task<List<MessageTitleModel>> GetMessageTitle(int teamID)
        {
            List<MessageTitleModel> distList = await _distList.GetMessageTitle(teamID);
            return distList;
        }

        public async Task<string> GetMsgTemplate(int msgTitleID)
        {
            string Msgtemplate = await _distList.GetMsgTemplate(msgTitleID);
            return Msgtemplate;
        }

        public async Task<List<string>> GetDL(string companyID)
        {
            var distList = await _distList.GetDL(companyID);
            return distList;
        }

        public async Task<List<string>> GetPhonenumbers(string distlist, string companyID)
        {
            var PhoneNumbers = await _distList.GetPhoneNumbers(distlist, companyID);
            return PhoneNumbers;
        }

        public async Task<List<DLModel>> GetDLandEntries(string companyID)
        {
            List<DLModel> result = await _distList.GetDLandEntries(companyID);
            return result;
        }

        public async Task<bool> DeleteDL(string listName, string companyID)
        {
            bool result = await _distList.DeleteDL(listName, companyID);
            return result;
        }

        public async Task<bool> CheckExists(string listName, string company)
        {
            bool IsExists = await _distList.CheckExists(listName, company);
            return IsExists;
        }

        public async Task<string> UploadTxtFile(string fileName, string listname, string username, string company, string companyID)
        {
            string line, Response = "";
            using (StreamReader sr = new(fileName))
            {

                int strlen;
                int numgood = 0,
                    numlenbad = 0, numcodebad = 0;
                // string Distlist = txtDistlistName.Text;
                // Read and display lines from the file until 
                // the end of the file is reached. 
                while ((line = sr.ReadLine()) != null)
                {
                    strlen = (line.Trim()).Length;
                    line = line.Trim();
                    if (strlen > 0)
                    {
                        if (strlen == 12 || strlen == 11)
                        {
                            if (((line.Substring(0, 2) == "44") && (line.Substring(0, 3) != "440")) || (line.Substring(0, 1) == "0"))
                            {
                                if (line.Substring(0, 1) == "0")
                                {

                                    line = "44" + line.Substring(1, line.Length - 1);


                                }
                                string strSQL = "insert into distlistmembers (username, listname, PhoneNr, PhoneNrDisplay, Name, CompanyName,CompanyID) values ('" + Strings.LCase(username) + "','" + Strings.LCase(Strings.Replace(listname, "'", "''")) + "','" + line + "', '" + line + "','" + Strings.LCase(username) + "','" + Strings.LCase(company) + "','" + companyID + "');";
                                int insertedCount = await _distList.InsertDistList(strSQL);
                                if (insertedCount > 0)
                                    numgood += 1;
                            }
                            else
                            {
                                numcodebad = numcodebad + 1;
                            }
                        }
                        else
                        {
                            numlenbad = numlenbad + 1;
                        }
                    }
                    else
                    {

                    }
                }
                Response = +numgood + "";
            }
            return Response;
        }

        public async Task<string> UploadCSVFile(string fileName, string listname, string username, string company, string companyID)
        {
            string Response = "";
            int numgood = 0, numlenbad = 0, numcodebad = 0;
            using (StreamReader sr = new (fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] DeviceNr;
                    line = line.Trim();
                    DeviceNr = line.Split(',');
                    int i = 0;
                    for (i = 0; i < DeviceNr.Count(); i++)
                    {
                        DeviceNr[i] = DeviceNr[i].Trim();
                        if (DeviceNr[i].Length > 0)
                        {
                            if (DeviceNr[i].Length == 12 || DeviceNr[i].Length == 11)
                            {

                                if ((DeviceNr[i].Substring(0, 2) == "44") && (DeviceNr[i].Substring(0, 3) != "440"))
                                {
                                    if (DeviceNr[i].Substring(0, 1) == "0")
                                    {

                                        DeviceNr[i] = "44" + DeviceNr[i].Substring(1, DeviceNr[i].Length - 1);


                                    }

                                    string strSQL = "insert into distlistmembers (username, listname, PhoneNr, PhoneNrDisplay, Name, CompanyName,CompanyID) values ('" + Strings.LCase(username) + "','" + Strings.LCase(Strings.Replace(listname, "'", "''")) + "','" + DeviceNr[i] + "', '" + DeviceNr[i] + "','" + Strings.LCase(username) + "','" + Strings.LCase(company) + "','" + companyID + "');";
                                    
                                    int output = await _distList.InsertDistList(strSQL);
                                    if(output > 0)
                                    {
                                        numgood += 1;
                                    }
                                }
                                else
                                {
                                    numcodebad = numcodebad + 1;
                                }
                            }
                            else
                            {
                                numlenbad = numlenbad + 1;
                            }
                        }
                        else
                        {

                        }
                    }
                }
                //string Response = +numgood + " Records updated and " + (numcodebad + numlenbad) + " Records failed due to wrong length and Wrong Code";

                Response = +numgood + "";
                return Response;
            }
        }
    }
}
