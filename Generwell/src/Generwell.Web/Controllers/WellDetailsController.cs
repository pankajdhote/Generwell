﻿using System;
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
    public class WellDetailsController : Controller
    {
        /// <summary>
        /// Added by pankaj
        /// Date:- 15-11-2016
        /// Display well Details for particular well.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string reportId)
        {
            WebClient webClient = new WebClient();
            var wellDetailsList = await webClient.GetWebApiWithTimeZone(GenerwellConstants.Constants.Well+"/"+ GenerwellConstants.Constants.WellId + "/linereports/"+reportId, GenerwellConstants.Constants.AccessToken);
            LineReportsViewModel wellDetailsViewModel = JsonConvert.DeserializeObject<LineReportsViewModel>(wellDetailsList);
            return View(wellDetailsViewModel.fields);
        }
    }
}
