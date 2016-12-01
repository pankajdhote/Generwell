using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.GenerwellEnum;
using Generwell.Modules.Global;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class TaskController : Controller
    {
        /// <summary>
        /// Added by rohit
        /// Date:- 15-11-2016
        /// Fetch all task from web api and display on task list page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                //Active menu on top for current selected tab
                GlobalFields.SetMenu(Menu.Task.ToString());
                WebClient webClient = new WebClient();
                List<TaskViewModel> taskViewModel = new List<TaskViewModel>();
                int previousPageValue = (int)Enum.Parse(typeof(PageOrder), GlobalFields.previousPage);
                //set previous page value for google map filteration
                GlobalFields.previousPage = PageOrder.Tasklisting.ToString();

                switch (previousPageValue)
                {
                    case 1:
                    case 4:
                    case 5:
                    case 6:
                        var taskFromWellListing = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Task, GlobalFields.AccessToken);
                        taskViewModel = JsonConvert.DeserializeObject<List<TaskViewModel>>(taskFromWellListing);
                        break;
                    case 2:
                    case 3:
                        var taskFromWellDetails = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well + "/" + GlobalFields.WellId + "/tasks", GlobalFields.AccessToken);
                        taskViewModel = JsonConvert.DeserializeObject<List<TaskViewModel>>(taskFromWellDetails);
                        break;
                    default:
                        break;
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
