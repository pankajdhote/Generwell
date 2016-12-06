using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.GenerwellEnum;
using Generwell.Modules.Global;
using Microsoft.AspNetCore.Http;
using System.Text;
using Generwell.Modules.Model;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class TaskDetailsController : BaseController 
    {
        public TaskDetailsController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices) : base(appSettings, generwellServices)
        {
        }

        /// <summary>
        /// Added by rohit
        /// Date:- 15-11-2016
        /// Fetch all task from web api and display on task list page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string taskId, string taskName, string isFollow)
        {
            try
            {
                //set previous page value for google map filteration                
                HttpContext.Session.SetString("previousPage", PageOrder.TaskDetails.ToString());

                if (!string.IsNullOrEmpty(taskId))
                {
                    HttpContext.Session.SetString("TaskId", Encoding.UTF8.GetString(Convert.FromBase64String(taskId)));
                    HttpContext.Session.SetString("TaskName", Encoding.UTF8.GetString(Convert.FromBase64String(taskName)));
                    HttpContext.Session.SetString("IsFollow", Encoding.UTF8.GetString(Convert.FromBase64String(isFollow)).ToLower() == GenerwellConstants.Constants.trueState ? GenerwellConstants.Constants.checkedState : string.Empty);
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
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateTaskFields(int fieldId, string value)
        {
            try
            {
                string taskDetailsRecord = await UpdateTaskDetails(fieldId,value);
                return View(taskDetailsRecord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
