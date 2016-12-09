﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Web.ViewModels;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Http;
using System.Text;
using Generwell.Modules.Model;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class TaskDetailsController : BaseController
    {

        private object numbers;

        public TaskDetailsController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices, ILoggerFactory loggerFactory) : base(appSettings, generwellServices, loggerFactory)
        {
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
                TaskDetailsViewModel taskdetailsViewModel = await GetTaskDetails();
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
                string taskDetailsRecord = await UpdateTaskDetails(Content);
                return View(taskDetailsRecord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
