using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Generwell.Modules.Management;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Modules.ViewModels;
using System.Collections.Generic;
using Generwell.Modules.GenerwellConstants;
using AutoMapper;
using Generwell.Core.Model;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class TaskDetailsController : BaseController
    {
        private readonly ITaskManagement _taskManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;

        public TaskDetailsController(ITaskManagement taskManagement, 
            IGenerwellManagement generwellManagement, IMapper mapper) : base(generwellManagement)
        {
            _taskManagement = taskManagement;
            _generwellManagement = generwellManagement;
            _mapper = mapper;
        }
        /// <summary>
        /// Added by rohit
        /// Date:- 15-11-2016
        /// Fetch all task from web api and display on task list page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string taskId, string taskName)
        {
            try
            {
                //set previous page value for google map filteration                
                HttpContext.Session.SetString("previousPage", PageOrder.TaskDetails.ToString());
                if (!string.IsNullOrEmpty(taskId))
                {
                    HttpContext.Session.SetString("TaskId", Encoding.UTF8.GetString(Convert.FromBase64String(taskId)));
                    HttpContext.Session.SetString("TaskName", Encoding.UTF8.GetString(Convert.FromBase64String(taskName)));
                }
                TaskDetailsModel taskdetailsModel = await _taskManagement.GetTaskDetails(HttpContext.Session.GetString("TaskId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                TaskDetailsViewModel taskdetailsViewModel = _mapper.Map<TaskDetailsViewModel>(taskdetailsModel);
                ContactFieldsModel contactData = await _generwellManagement.GetContactDetails(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                taskdetailsViewModel.contactFields = _mapper.Map<ContactFieldsViewModel>(contactData);

                if (taskdetailsViewModel != null)
                {
                    HttpContext.Session.SetString("FieldLevelId", taskdetailsViewModel.fieldLevelId.ToString());
                    HttpContext.Session.SetString("KeyId", taskdetailsViewModel.keyId.ToString());
                }
                //fill Dictionaries dropdown list
                List<DictionaryModel> filterModel = await _taskManagement.GetDictionaries(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                List<DictionaryViewModel> filterViewModel = _mapper.Map<List<DictionaryViewModel>>(filterModel);
                ViewBag.Dictionaries = filterViewModel;

                return View(taskdetailsViewModel);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskDetails Controller Index action method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Error", "Accounts");
            }
        }
        /// <summary>
        /// Added by rohit
        /// Date:- 29-11-2016
        /// to save fields data
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<string> UpdateTaskFields(string Content)
        {
            try
            {
                    string taskDetailsResponse = await _taskManagement.UpdateTaskDetails(Content, HttpContext.Session.GetString("TaskId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                    return taskDetailsResponse;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskDetails Controller UpdateTaskFields action method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return response;
            }
        }
    }
}
