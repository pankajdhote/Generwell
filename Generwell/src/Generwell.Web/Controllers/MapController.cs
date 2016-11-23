using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules;
using Newtonsoft.Json;
using Generwell.Web.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class MapController : Controller
    {
        /// <summary>
        /// Added by pankaj
        /// Date:- 22-11-2016
        /// Fetch all wells from web api and display on map.
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        public async Task<ActionResult> Index()
        {
            // Change active menu class
            GenerwellConstants.Constants.MapActive = GenerwellConstants.Constants.Active;
            GenerwellConstants.Constants.TaskActive = string.Empty;
            GenerwellConstants.Constants.WellActive = string.Empty;

            WebClient webClient = new WebClient();
            var wellData = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GenerwellConstants.Constants.AccessToken);
            List<MapViewModel> wellRecords = JsonConvert.DeserializeObject<List<MapViewModel>>(wellData);

            return View(wellRecords);
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 22-11-2016
        /// plot markers for well
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        public async Task<JsonResult> plotMarker()
        {
            // Change active menu class
            GenerwellConstants.Constants.MapActive = GenerwellConstants.Constants.Active;
            GenerwellConstants.Constants.TaskActive = string.Empty;
            GenerwellConstants.Constants.WellActive = string.Empty;

            WebClient webClient = new WebClient();
            var wellData = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GenerwellConstants.Constants.AccessToken);
            List<MapViewModel> wellRecords = JsonConvert.DeserializeObject<List<MapViewModel>>(wellData);

            return Json(wellRecords);
        }

    }
}
