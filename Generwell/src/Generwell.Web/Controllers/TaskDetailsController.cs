using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.ViewModels;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Http;
using System.Text;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Generwell.Core.Model;
using Generwell.Modules.Management;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class TaskDetailsController : BaseController
    {

        private object numbers;
        private readonly ITaskManagement _taskManagement;
        public TaskDetailsController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices, ITaskManagement taskManagement) : base(appSettings, generwellServices)
        {
            _taskManagement = taskManagement;
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
                TaskDetailsViewModel taskdetailsViewModel = await _taskManagement.GetTaskDetails(HttpContext.Session.GetString("WellId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                taskdetailsViewModel.contactFields = await GetContactDetails();
                if (taskdetailsViewModel != null)
                {
                    HttpContext.Session.SetString("FieldLevelId", taskdetailsViewModel.fieldLevelId.ToString());
                    HttpContext.Session.SetString("KeyId", taskdetailsViewModel.keyId.ToString());
                }
                return View(taskdetailsViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Added by rohit
        /// Date:- 29-11-2016
        /// to save fields data
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //public async Task<ActionResult> UpdateTaskFields(string [] IdArray, string [] ValueArray, string Content)
        public async Task<ActionResult> UpdateTaskFields(string[] Content)
        {
            try
            {
                string taskDetailsRecord = await _taskManagement.UpdateTaskDetails(Content, HttpContext.Session.GetString("TaskId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                return View(taskDetailsRecord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
