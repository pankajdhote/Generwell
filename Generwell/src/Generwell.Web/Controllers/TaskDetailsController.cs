using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;
using Generwell.Web.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class TaskDetailsController : Controller
    {
        /// <summary>
        /// Added by Rohit
        /// Date:- 16-11-2016
        /// Display Task Details for particular task.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]       
        public async Task<ActionResult> Index(string taskId, string taskName, string isFollow)
        {
            GenerwellConstants.Constants.TaskId = taskId;
            if (string.IsNullOrEmpty(taskId))
            {
                GenerwellConstants.Constants.TaskId = taskId;
            }
            TempData["TaskName"] = taskName;
            TempData["IsFollow"] = isFollow == "True" ? isFollow = "checked" : null;
            WebClient webClient = new WebClient();
            var taskDetailsList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Task, GenerwellConstants.Constants.AccessToken);
            List<TaskDetailsViewModel> taskDetailsViewModel = JsonConvert.DeserializeObject<List<TaskDetailsViewModel>>(taskDetailsList);
            return View(taskDetailsViewModel);
        }

        public async Task<string> Follow(string isFollow)
        {
            string id = GenerwellConstants.Constants.TaskId;
            WebClient webClient = new WebClient();
            if (isFollow == "true")
            {
                var getResponse = await webClient.PostWebApiData(GenerwellConstants.Constants.Task + "/" + id + "/follow", GenerwellConstants.Constants.AccessToken);
            }
            else
            {
                var getResponse = await webClient.DeleteWebApiData(GenerwellConstants.Constants.Task + "/" + id + "/unfollow", GenerwellConstants.Constants.AccessToken);
                return getResponse.ToString();
            }
            return string.Empty;
        }
    }
}
