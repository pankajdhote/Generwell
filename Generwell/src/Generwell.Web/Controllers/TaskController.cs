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
using Microsoft.AspNetCore.Http;
using Generwell.Modules.Model;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class TaskController : BaseController
    {
        public TaskController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices, ILoggerFactory loggerFactory) : base(appSettings, generwellServices, loggerFactory)
        {
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
                        taskViewModel = await GetTasks();
                        break;
                    case 2:
                    case 3:
                        taskViewModel = await GetTasksByWellId();
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
