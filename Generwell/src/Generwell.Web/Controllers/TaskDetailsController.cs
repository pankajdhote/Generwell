﻿using System;
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
    public class TaskDetailsController : Controller
    {
        /// <summary>
        /// Added by rohit
        /// Date:- 15-11-2016
        /// Fetch all task from web api and display on task list page.
        /// 
        /// </summary>
        /// <returns></returns>
        [Httpget]

        public async Task<ActionResult> Index(string taskId, string taskName, string isFollow)
        {

            if (!string.IsNullOrEmpty(taskId))
            {
                GenerwellConstants.Constants.TaskId = taskId;
                GenerwellConstants.Constants.TaskName = taskName;
                GenerwellConstants.Constants.IsFollow = isFollow == "True" ? isFollow = "checked" : null;
            }

            try
            {
                WebClient webClient = new WebClient();
                var getTaskDetailsList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.TaskDetails + "/" + taskId, GenerwellConstants.Constants.AccessToken);
                var getContactDetail = await webClient.GetWebApiDetails(GenerwellConstants.Constants.ContactDetails, GenerwellConstants.Constants.AccessToken);
                TaskDetailsViewModel taskdetailsViewModel = JsonConvert.DeserializeObject<TaskDetailsViewModel>(getTaskDetailsList);
                taskdetailsViewModel.contactFields = JsonConvert.DeserializeObject<ContactFieldsViewModel>(getContactDetail); ;
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
        //[HttpGet]
        //public async Task<PartialViewResult> ContactField()
        //{
        //    try
        //    {
        //        WebClient webClient = new WebClient();
        //        var getContactList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.ContactDetails, GenerwellConstants.Constants.AccessToken);
        //        ContactFieldsViewModel contactViewModel = JsonConvert.DeserializeObject<ContactFieldsViewModel>(getContactList);
        //        return PartialView("_ContactField", contactViewModel);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }



    internal class HttpgetAttribute : Attribute
    {
    }
}
