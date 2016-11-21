using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class TaskDetailsController : Controller
    {
        /// <summary>
        /// Added by rohit
        /// Date:- 15-11-2016
        /// Fetch all task from web api and display on task list page.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string taskId, string taskName, string isFollow)
        {
            try
            {
                if (!string.IsNullOrEmpty(taskId))
                {
                    GenerwellConstants.Constants.TaskId = taskId;
                    GenerwellConstants.Constants.TaskName = taskName;
                    GenerwellConstants.Constants.IsFollow = isFollow == "True" ? isFollow = "checked" : null;
                }
                WebClient webClient = new WebClient();
                var getTaskDetailsList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.TaskDetails + "/" + GenerwellConstants.Constants.TaskId, GenerwellConstants.Constants.AccessToken);
                TaskDetailsViewModel taskdetailsViewModel = JsonConvert.DeserializeObject<TaskDetailsViewModel>(getTaskDetailsList);
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
        /// Date:- 18-11-2016
        /// Fetch Contact details from web api and display on task details page.
        /// 
        /// </summary>
        /// <returns></returns>
    //    [HttpGet]
    //    public async Task<PartialViewResult> ContactField()
    //    {
    //        try
    //        {
    //            WebClient webClient = new WebClient();
    //            var getContactList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.ContactDetails, GenerwellConstants.Constants.AccessToken);
    //            ContactFieldsViewModel contactViewModel = JsonConvert.DeserializeObject <ContactFieldsViewModel>(getContactList);
    //            return PartialView("_ContactField", contactViewModel);

    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    }
}
