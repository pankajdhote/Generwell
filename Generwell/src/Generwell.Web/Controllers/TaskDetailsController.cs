using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.GenerwellEnum;



// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class TaskDetailsController : Controller
    {
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
                GenerwellConstants.Constants.previousPage = PageOrder.TaskDetails.ToString();

                if (!string.IsNullOrEmpty(taskId))
                {
                    GenerwellConstants.Constants.TaskId = taskId;
                    GenerwellConstants.Constants.TaskName = taskName;
                    GenerwellConstants.Constants.IsFollow = isFollow.ToLower() == "true" ? isFollow = "checked" : null;
                }
                WebClient webClient = new WebClient();
                var getTaskDetailsList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.TaskDetails + "/" + GenerwellConstants.Constants.TaskId, GenerwellConstants.Constants.AccessToken);
                var getContactDetail = await webClient.GetWebApiDetails(GenerwellConstants.Constants.ContactDetails, GenerwellConstants.Constants.AccessToken);
                TaskDetailsViewModel taskdetailsViewModel = JsonConvert.DeserializeObject<TaskDetailsViewModel>(getTaskDetailsList);
                taskdetailsViewModel.contactFields = JsonConvert.DeserializeObject<ContactFieldsViewModel>(getContactDetail);
                if (taskdetailsViewModel != null)
                {
                    GenerwellConstants.Constants.FieldLevelId = taskdetailsViewModel.fieldLevelId;
                    GenerwellConstants.Constants.KeyId = taskdetailsViewModel.keyId;
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
        public async Task<ActionResult> UpdateTaskFields(int fieldId, string value)
        {
           
            try
            {              
                WebClient webClient = new WebClient();
                var taskDetailsReecord = await webClient.UpdateTaskData(GenerwellConstants.Constants.TaskDetails + "/" + GenerwellConstants.Constants.TaskId, GenerwellConstants.Constants.AccessToken, value, fieldId);
                return View(taskDetailsReecord);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}