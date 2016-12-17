using AutoMapper;
using Generwell.Core.Model;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Modules.Services;
using Generwell.Modules.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generwell.Modules.Management
{
    public class TaskManagement : ITaskManagement
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IGenerwellServices _generwellServices;
        private readonly IMapper _mapper;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly TaskDetailsModel _objTaskDetails;
        private readonly List<TaskModel> _objTaskList;
        private readonly List<DictionaryModel> _objDictionary;
        private readonly List<ContactInformationModel> _objContactInfo;
        public TaskManagement(IOptions<AppSettingsModel> appSettings, 
            IGenerwellServices generwellServices, 
            IMapper mapper,
            IGenerwellManagement generwellManagement,
            TaskDetailsModel objTaskDetails,
            List<TaskModel> objTaskList,
            List<DictionaryModel> objDictionary,
            List<ContactInformationModel> objContactInfo)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
            _mapper = mapper;
            _objTaskDetails = objTaskDetails;
            _generwellManagement = generwellManagement;
            _objTaskList = objTaskList;
            _objDictionary = objDictionary;
            _objContactInfo = objContactInfo;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Get Task Details From web api by task id.
        /// </summary>
        /// <returns></returns>
        public async Task<TaskDetailsModel> GetTaskDetails(string taskId, string accessToken, string tokenType)
        {
            try
            {
                string taskDetailsList = await _generwellServices.GetWebApiDetails(_appSettings.TaskDetails + "/" + taskId, accessToken, tokenType);
                TaskDetailsModel taskdetailsModel = JsonConvert.DeserializeObject<TaskDetailsModel>(taskDetailsList);
                return taskdetailsModel;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskManagement GetTaskDetails method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objTaskDetails;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Update Task Details fields using patch api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdateTaskDetails(string Content, string taskId, string accessToken, string tokenType)
        {
            try
            {
                string taskDetailsReecord = await _generwellServices.UpdateTaskData(_appSettings.TaskDetails + "/" + taskId, accessToken, tokenType, Content);
                return taskDetailsReecord;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskManagement UpdateTaskDetails method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return response;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Fetch all tasks from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaskModel>> GetTasks(string accessToken, string tokenType)
        {
            try
            {
                string taskList = await _generwellServices.GetWebApiDetails(_appSettings.Task, accessToken, tokenType);
                List<TaskModel> taskModelList = JsonConvert.DeserializeObject<List<TaskModel>>(taskList);
                return taskModelList;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskManagement GetTasks method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objTaskList;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Fetch all tasks from web api by wellId.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaskModel>> GetTasksByWellId(string wellId, string accessToken, string tokenType)
        {
            try
            {
                string taskRecord = await _generwellServices.GetWebApiDetails(_appSettings.Well + "/" + wellId + "/tasks", accessToken, tokenType);
                List<TaskModel> taskModelList = JsonConvert.DeserializeObject<List<TaskModel>>(taskRecord);
                return taskModelList;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskManagement GetTasksByWellId method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objTaskList;
            }
        }

        /// <summary>
        /// Added by pankaj
        /// Date:-13-12-2016
        /// Fetch all dictionaries from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictionaryModel>> GetDictionaries(string accessToken, string tokenType)
        {
            try
            {
                string filterList = await _generwellServices.GetWebApiDetails(_appSettings.Dictionaries, accessToken, tokenType);
                List<DictionaryModel> dictionaryModel = JsonConvert.DeserializeObject<List<DictionaryModel>>(filterList);
                return dictionaryModel;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskManagement GetDictionaries method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objDictionary;
            }
        }

        /// <summary>
        /// Added by Rohit
        /// Date:-14-12-2016
        /// Fetch all contact details from web api.
        /// </summary>
        /// <returns></returns>

        public async Task<List<ContactInformationModel>> GetContactInformation(string accessToken, string tokenType)
        {
            try
            {
                string personnelRecord = await _generwellServices.GetWebApiDetails(_appSettings.ContactInfo, accessToken, tokenType);
                List<ContactInformationModel> ContactInformation = JsonConvert.DeserializeObject<List<ContactInformationModel>>(personnelRecord);
                return ContactInformation;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskManagement GetDictionaries method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, accessToken, tokenType, logContent);
                return _objContactInfo;
            }
        }
    }
}
