using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.GenerwellEnum;
using Generwell.Modules.Global;
using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class TaskDetailsController : Controller
    {
        private object numbers;

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
                GlobalFields.previousPage = PageOrder.TaskDetails.ToString();

                if (!string.IsNullOrEmpty(taskId))
                {
                    GlobalFields.TaskId = taskId;
                    GlobalFields.TaskName = taskName;
                    GlobalFields.IsFollow = isFollow.ToLower() == "true" ? isFollow = "checked" : null;
                }
                WebClient webClient = new WebClient();
                var getTaskDetailsList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.TaskDetails + "/" + GlobalFields.TaskId, GlobalFields.AccessToken);
                var getContactDetail = await webClient.GetWebApiDetails(GenerwellConstants.Constants.ContactDetails, GlobalFields.AccessToken);
                TaskDetailsViewModel taskdetailsViewModel = JsonConvert.DeserializeObject<TaskDetailsViewModel>(getTaskDetailsList);
                taskdetailsViewModel.contactFields = JsonConvert.DeserializeObject<ContactFieldsViewModel>(getContactDetail);
                if (taskdetailsViewModel != null)
                {
                    GlobalFields.FieldLevelId = taskdetailsViewModel.fieldLevelId;
                    GlobalFields.KeyId = taskdetailsViewModel.keyId;
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
       
        //public async Task<ActionResult> UpdateTaskFields(string [] IdArray, string [] ValueArray, string Content)
        public async Task<ActionResult> UpdateTaskFields(string [] Content)
        {
            WebClient webClient = null;
            try
            {                
                webClient = new WebClient();               
                string valueTest = GlobalFields.AccessToken;
                var taskDetailsReecord = await webClient.UpdateTaskData(GenerwellConstants.Constants.TaskDetails + "/" + GlobalFields.TaskId, GlobalFields.AccessToken, Content/*, fieldId, value*/);
                return View(taskDetailsReecord);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
