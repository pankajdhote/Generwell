using System;
using System.Collections.Generic;
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
                //set menu
                GenerwellConstants.Constants.setMenu(Menu.Task.ToString());
                WebClient webClient = new WebClient();
                List<TaskViewModel> taskViewModel=new List<TaskViewModel>();
                int previousPageValue = (int)Enum.Parse(typeof(PageOrder), GenerwellConstants.Constants.previousPage);
                //set previous page value for google map filteration
                GenerwellConstants.Constants.previousPage = PageOrder.Tasklisting.ToString();

                switch (previousPageValue)
                {
                    case 1:
                    case 4:
                    case 5:
                    case 6:
                        var taskFromWellListing = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Task, GenerwellConstants.Constants.AccessToken);
                        taskViewModel = JsonConvert.DeserializeObject<List<TaskViewModel>>(taskFromWellListing);
                        break;
                    case 2:
                    case 3:
                        var taskFromWellDetails = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well+"/"+GenerwellConstants.Constants.WellId+"/tasks", GenerwellConstants.Constants.AccessToken);
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
