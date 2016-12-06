
using Generwell.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Generwell.Modules.Model;
using System.Collections.Generic;
using Generwell.Modules.Services;
using Generwell.Modules.GenerwellConstants;
using System;
using System.Text;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IGenerwellServices _generwellServices;
        public BaseController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 15-11-2016
        /// Authenticate user for successfull login
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AccessTokenViewModel> AuthenticateUser(string userName, string password, string webApiUrl)
        {
            try
            {
                string url = webApiUrl + "/" + _appSettings.Token;
                string webApiDetails = await _generwellServices.ProcessRequest(userName, password, url);
                AccessTokenViewModel accessTokenViewModel = JsonConvert.DeserializeObject<AccessTokenViewModel>(webApiDetails);
                return accessTokenViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<WellViewModel>> GetWells(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string getWellList = await _generwellServices.GetWebApiDetails(_appSettings.WellFilter + "=" + id, HttpContext.Session.GetString("AccessToken"));
                List<WellViewModel> wellViewModel = JsonConvert.DeserializeObject<List<WellViewModel>>(getWellList);
                return wellViewModel;
            }
            else
            {
                string getWellList = await _generwellServices.GetWebApiDetails(_appSettings.Well, HttpContext.Session.GetString("AccessToken"));
                List<WellViewModel> wellViewModel = JsonConvert.DeserializeObject<List<WellViewModel>>(getWellList);
                return wellViewModel;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<WellViewModel> GetWellById(string id)
        {
            string getWellList = await _generwellServices.GetWebApiDetails(_appSettings.Well + "/" + id, HttpContext.Session.GetString("AccessToken"));
            WellViewModel wellViewModel = JsonConvert.DeserializeObject<WellViewModel>(getWellList);
            return wellViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api by filterId.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MapViewModel>> GetWellsByFilterId()
        {
            string wellRecordByFilter = await _generwellServices.GetWebApiDetails(_appSettings.WellFilter + "=" + HttpContext.Session.GetString("defaultFilter"), HttpContext.Session.GetString("AccessToken"));
            List<MapViewModel> wellViewModel = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecordByFilter);
            return wellViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api by filterId.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MapViewModel>> GetWellsWithoutFilterId()
        {
            string wellRecordByFilter = await _generwellServices.GetWebApiDetails(_appSettings.WellFilter + "=" + HttpContext.Session.GetString("defaultFilter"), HttpContext.Session.GetString("AccessToken"));
            List<MapViewModel> wellViewModel = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecordByFilter);
            return wellViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well line reports from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<WellLineReportViewModel>> GetWellLineReports()
        {
            string wellLineReportList = await _generwellServices.GetWebApiDetails(_appSettings.WellLineReports, HttpContext.Session.GetString("AccessToken"));
            List<WellLineReportViewModel> wellLineReportViewModel = JsonConvert.DeserializeObject<List<WellLineReportViewModel>>(wellLineReportList);
            return wellLineReportViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all filters from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<FilterViewModel>> GetFilters()
        {
            string filterList = await _generwellServices.GetWebApiDetails(_appSettings.Filters, HttpContext.Session.GetString("AccessToken"));
            List<FilterViewModel> filterViewModel = JsonConvert.DeserializeObject<List<FilterViewModel>>(filterList);
            return filterViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all contact details from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<ContactFieldsViewModel> GetContactDetails()
        {
            string personnelRecord = await _generwellServices.GetWebApiDetails(_appSettings.ContactDetails, HttpContext.Session.GetString("AccessToken"));
            ContactFieldsViewModel contactFieldRecord = JsonConvert.DeserializeObject<ContactFieldsViewModel>(personnelRecord);
            return contactFieldRecord;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Follow and UnFollow Wells.
        /// </summary>
        /// <returns></returns>
        public async Task<string> SetFollowUnfollow(string isFollow, string id)
        {
            if (isFollow == GenerwellConstants.Constants.trueState)
            {
                HttpContext.Session.SetString("IsFollow", GenerwellConstants.Constants.checkedState);
                string response = await _generwellServices.PostWebApiData(_appSettings.Well + "/" + id + "/follow", HttpContext.Session.GetString("AccessToken"));
                return response;
            }
            else
            {
                HttpContext.Session.SetString("IsFollow", GenerwellConstants.Constants.uncheckedState);
                string response = await _generwellServices.DeleteWebApiData(_appSettings.Well + "/" + id + "/unfollow", HttpContext.Session.GetString("AccessToken"));
                return response;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        ///Get well details from reportId.
        /// </summary>
        /// <returns></returns>
        public async Task<LineReportsViewModel> GetWellDetailsByReportId(string reportId)
        {
            string wellDetailsList = await _generwellServices.GetWebApiWithTimeZone(_appSettings.Well + "/" + HttpContext.Session.GetString("WellId") + "/linereports/" + Encoding.UTF8.GetString(Convert.FromBase64String(reportId)), HttpContext.Session.GetString("AccessToken"));
            LineReportsViewModel wellDetailsViewModel = JsonConvert.DeserializeObject<LineReportsViewModel>(wellDetailsList);
            return wellDetailsViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Get Task Details From web api by task id.
        /// </summary>
        /// <returns></returns>
        public async Task<TaskDetailsViewModel> GetTaskDetails()
        {
            string taskDetailsList = await _generwellServices.GetWebApiDetails(_appSettings.TaskDetails + "/" + HttpContext.Session.GetString("TaskId"), HttpContext.Session.GetString("AccessToken"));
            TaskDetailsViewModel taskdetailsViewModel = JsonConvert.DeserializeObject<TaskDetailsViewModel>(taskDetailsList);
            return taskdetailsViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Update Task Details fields using patch api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdateTaskDetails(string[] Content)
        {
            string taskDetailsReecord = await _generwellServices.UpdateTaskData(_appSettings.TaskDetails + "/" + HttpContext.Session.GetString("TaskId"), HttpContext.Session.GetString("AccessToken"), Content);
            return taskDetailsReecord;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Fetch all tasks from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaskViewModel>> GetTasks()
        {
            string taskList = await _generwellServices.GetWebApiDetails(_appSettings.Task, HttpContext.Session.GetString("AccessToken"));
            List<TaskViewModel> taskViewModelList = JsonConvert.DeserializeObject<List<TaskViewModel>>(taskList);
            return taskViewModelList;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Fetch all tasks from web api by wellId.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaskViewModel>> GetTasksByWellId()
        {
            string taskRecord = await _generwellServices.GetWebApiDetails(_appSettings.Well + "/" + HttpContext.Session.GetString("WellId") + "/tasks", HttpContext.Session.GetString("AccessToken"));
            List<TaskViewModel> taskViewModelList = JsonConvert.DeserializeObject<List<TaskViewModel>>(taskRecord);
            return taskViewModelList;
        }
    }
}
