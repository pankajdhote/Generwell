using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.ViewModels;
using Generwell.Modules.GenerwellEnum;
using Generwell.Modules.Global;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Generwell.Modules.Management;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class TaskController : BaseController
    {
        private readonly ITaskManagement _taskManagement;
        public TaskController(ITaskManagement taskManagement)
        {
            _taskManagement = taskManagement;
        }
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
                int previousPageValue = (int)Enum.Parse(typeof(PageOrder), HttpContext.Session.GetString("previousPage"));
                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.Tasklisting.ToString());

                List<TaskViewModel> taskViewModel = new List<TaskViewModel>();
                switch (previousPageValue)
                {
                    case 1:
                    case 4:
                    case 5:
                    case 6:
                        taskViewModel = await _taskManagement.GetTasks(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        break;
                    case 2:
                    case 3:
                        taskViewModel = await _taskManagement.GetTasksByWellId(HttpContext.Session.GetString("WellId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
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
