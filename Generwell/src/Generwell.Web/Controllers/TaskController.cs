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
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Core.Model;
using AutoMapper;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class TaskController : BaseController
    {
        private readonly ITaskManagement _taskManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;
        public TaskController(ITaskManagement taskManagement, 
            IGenerwellManagement generwellManagement, IMapper mapper) : base(generwellManagement)
        {
            _taskManagement = taskManagement;
            _generwellManagement = generwellManagement;
            _mapper = mapper;
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
                string checkboxStatus = "Unchecked";
                if (previousPageValue > 1)
                {
                    ViewBag.myTask = checkboxStatus;
                    //return View(new Index);
                }
                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.Tasklisting.ToString());

                List<TaskViewModel> taskViewModel = new List<TaskViewModel>();
                List<TaskModel> taskModel = new List<TaskModel>();

                switch (previousPageValue)
                {
                    case 1:
                    case 4:
                    case 5:
                    case 6:
                        taskModel = await _taskManagement.GetTasks(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        break;
                    case 2:
                    case 3:
                        taskModel = await _taskManagement.GetTasksByWellId(HttpContext.Session.GetString("WellId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        break;
                    default:
                        break;
                }
                taskViewModel = _mapper.Map<List<TaskViewModel>>(taskModel);
                return View(taskViewModel);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Tasks Controller Index action method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Error", "Accounts");
            }
        }
    }
}
