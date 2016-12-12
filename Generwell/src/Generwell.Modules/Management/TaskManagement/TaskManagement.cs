using AutoMapper;
using Generwell.Core.Model;
using Generwell.Modules.Services;
using Generwell.Modules.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generwell.Modules.Management
{
    public class TaskManagement : ITaskManagement
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IGenerwellServices _generwellServices;
        private readonly IMapper _mapper;
        public TaskManagement(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
            _mapper = mapper;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Get Task Details From web api by task id.
        /// </summary>
        /// <returns></returns>
        public async Task<TaskDetailsViewModel> GetTaskDetails(string taskId, string accessToken, string tokenType)
        {
            string taskDetailsList = await _generwellServices.GetWebApiDetails(_appSettings.TaskDetails + "/" + taskId, accessToken, tokenType);
            TaskDetailsModel taskdetailsModel = JsonConvert.DeserializeObject<TaskDetailsModel>(taskDetailsList);
            TaskDetailsViewModel taskdetailsViewModel = _mapper.Map<TaskDetailsViewModel>(taskdetailsModel);
            return taskdetailsViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Update Task Details fields using patch api.
        /// </summary>
        /// <returns></returns>
        public async Task<string> UpdateTaskDetails(string[] Content, string taskId, string accessToken, string tokenType)
        {
            string taskDetailsReecord = await _generwellServices.UpdateTaskData(_appSettings.TaskDetails + "/" + taskId, accessToken, tokenType, Content);
            return taskDetailsReecord;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Fetch all tasks from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaskViewModel>> GetTasks(string accessToken, string tokenType)
        {
            string taskList = await _generwellServices.GetWebApiDetails(_appSettings.Task, accessToken, tokenType);
            List<TaskModel> taskModelList = JsonConvert.DeserializeObject<List<TaskModel>>(taskList);
            List<TaskViewModel> taskdetailsViewModel = _mapper.Map<List<TaskViewModel>>(taskModelList);
            return taskdetailsViewModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-05-12-2016
        /// Fetch all tasks from web api by wellId.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TaskViewModel>> GetTasksByWellId(string wellId, string accessToken, string tokenType)
        {
            string taskRecord = await _generwellServices.GetWebApiDetails(_appSettings.Well + "/" + wellId + "/tasks", accessToken, tokenType);
            List<TaskModel> taskModelList = JsonConvert.DeserializeObject<List<TaskModel>>(taskRecord);
            List<TaskViewModel> taskdetailsViewModel = _mapper.Map<List<TaskViewModel>>(taskModelList);
            return taskdetailsViewModel;
        }
    }
}
