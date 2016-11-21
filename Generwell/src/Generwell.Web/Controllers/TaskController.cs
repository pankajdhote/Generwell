using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;
using System.Globalization;

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
        public async Task<ActionResult> Index()
        {
            try
            {
                //change active menu class
                GenerwellConstants.Constants.TaskActive = GenerwellConstants.Constants.Active;
                GenerwellConstants.Constants.WellActive = string.Empty;
                WebClient webClient = new WebClient();               
                var getTaskList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Task, GenerwellConstants.Constants.AccessToken);
                //if (getTaskList!=null)
                //{
                //    CultureInfo provider = CultureInfo.InvariantCulture;
                //    string dateString = "08082010";
                //    string format = "MMddyyyy";
                //    DateTime result = DateTime.ParseExact(dateString, format, provider);
                //}
                List<TaskViewModel> taskViewModel = JsonConvert.DeserializeObject<List<TaskViewModel>>(getTaskList);
                return View(taskViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<PartialViewResult> FilterTask(string id)
        {
            try
            {
                WebClient webClient = new WebClient();
                var getTaskList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.TaskFilter + "=" + id, GenerwellConstants.Constants.AccessToken);
                List<TaskViewModel> taskViewModel = JsonConvert.DeserializeObject<List<TaskViewModel>>(getTaskList);
                return PartialView("_FilterTask", taskViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
