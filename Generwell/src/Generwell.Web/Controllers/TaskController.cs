using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class TaskController : Controller
    {
        /// <summary>
        /// Added by rohit
        /// Date:- 15-11-2016
        /// Fetch all task from web api and display on task list page.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string isWellId)
        {
            try
            {
                WebClient webClient = new WebClient();
                List<TaskViewModel> taskViewModel;
                if (!string.IsNullOrEmpty(isWellId))
                {
                    var getTaskList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Task+ "?keyId=" + GenerwellConstants.Constants.WellId, GenerwellConstants.Constants.AccessToken);
                    taskViewModel = JsonConvert.DeserializeObject<List<TaskViewModel>>(getTaskList);
                }
                else
                {
                    var getTaskList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Task, GenerwellConstants.Constants.AccessToken);
                    taskViewModel = JsonConvert.DeserializeObject<List<TaskViewModel>>(getTaskList);
                }
                return View(taskViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
