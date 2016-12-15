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
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Generwell.Core.Model;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class TaskDetailsController : BaseController
    {
        private readonly ITaskManagement _taskManagement;
        private readonly IGenerwellManagement _generwellManagement;
        public TaskDetailsController(ITaskManagement taskManagement, IGenerwellManagement generwellManagement)
        {
            _taskManagement = taskManagement;
            _generwellManagement = generwellManagement;
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

                TaskDetailsViewModel taskdetailsViewModel = await _taskManagement.GetTaskDetails(HttpContext.Session.GetString("TaskId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                taskdetailsViewModel.contactFields = await _generwellManagement.GetContactDetails(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));

                List<ContactInformationViewModel> ContactInformationViewModel = await _generwellManagement.GetContactInformation(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                ViewBag.ContactInfo = ContactInformationViewModel;
                // taskdetailsViewModel.contactInformation = await _generwellManagement.GetContactInformation(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                if (taskdetailsViewModel != null)
                {
                    HttpContext.Session.SetString("FieldLevelId", taskdetailsViewModel.fieldLevelId.ToString());
                    HttpContext.Session.SetString("KeyId", taskdetailsViewModel.keyId.ToString());
                }
                //fill Dictionaries dropdown list
                List<DictionaryViewModel> filterViewModel = await _taskManagement.GetDictionaries(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                ViewBag.Dictionaries = filterViewModel;

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
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<string> UpdateTaskFields(string Content)
        {
            try
            {
                string replacedContent = Content.Replace("\\","");
                string taskDetailsResponse = await _taskManagement.UpdateTaskDetails(replacedContent, HttpContext.Session.GetString("TaskId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                return taskDetailsResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
