using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;
using Microsoft.AspNetCore.JsonPatch;


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
                //for update data
                var getFieldsDetailsList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.TaskDetails + "/" + GenerwellConstants.Constants.TaskId, GenerwellConstants.Constants.AccessToken);
                var getContactDetail = await webClient.GetWebApiDetails(GenerwellConstants.Constants.ContactDetails, GenerwellConstants.Constants.AccessToken);
                TaskDetailsViewModel taskdetailsViewModel = JsonConvert.DeserializeObject<TaskDetailsViewModel>(getTaskDetailsList);
               
                taskdetailsViewModel.contactFields = JsonConvert.DeserializeObject<ContactFieldsViewModel>(getContactDetail);

                taskdetailsViewModel.TaskFields = JsonConvert.DeserializeObject<TaskFieldsUpdateViewModel>(getFieldsDetailsList);


                //if (taskdetailsViewModel != null)
                //{
                //    GenerwellConstants.Constants.FieldLevelId = taskdetailsViewModel.fieldLevelId;
                //    GenerwellConstants.Constants.KeyId = taskdetailsViewModel.keyId;
                //}
                return View(taskdetailsViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Added by rohit
        /// Date:- 23-11-2016
        /// to save fields data
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult UpdateContactFields(int fieldId, string displayValue)
        {
            
            //WebClient webClient = new WebClient();
            //var getTaskDetailsList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.TaskDetails + "/" + GenerwellConstants.Constants.TaskId, GenerwellConstants.Constants.AccessToken);
            return Json("Success");

        }

        //private TaskFieldsUpdateViewModel fields = new TaskFieldsUpdateViewModel
        //{
        //    fieldId = 6,
        //    displayValue = "Directional2",          
          
        //};

        //[HttpPatch]
        //public IActionResult Patch([FromBody]JsonPatchDocument<TaskFieldsUpdateViewModel> patch)
        //{
        //    //var patched = fields.Copy();
        //    patch.ApplyTo(patched, ModelState);

        //    if (!ModelState.IsValid)
        //    {
        //        return new BadRequestObjectResult(ModelState);
        //    }

        //    var model = new
        //    {
        //        original = fields,
        //        patched = patched
        //    };

        //    return Ok(model);
        //}

    }
}
