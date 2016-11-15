using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Generwell.Modules.GenerwellConstants;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class WellLineReportController : Controller
    {
        /// <summary>
        /// Added by pankaj
        /// Date:- 15-11-2016
        /// Display well line reports for particular well.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string wellId, string wellName, string isFollow)
        {
            GenerwellConstants.Constants.WellId = wellId;
            TempData["WellName"] = wellName;
            TempData["IsFollow"] = isFollow=="True"? isFollow="checked": null;
            WebClient webClient = new WebClient();
            var wellLineReportList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.WellLineReports, GenerwellConstants.Constants.AccessToken);
            List<WellLineReportViewModel> wellLineReportViewModel = JsonConvert.DeserializeObject<List<WellLineReportViewModel>>(wellLineReportList);
            return View(wellLineReportViewModel);
        }


        public async Task<string> Follow(string id, string isFollow)
        {
            WebClient webClient = new WebClient();
            if (isFollow == "true")
            {
                var getResponse = await webClient.PostWebApiData(GenerwellConstants.Constants.Well + "/" + id + "/follow", GenerwellConstants.Constants.AccessToken);
            }
            else
            {
                var getResponse = await webClient.DeleteWebApiData(GenerwellConstants.Constants.Well + "/" + id + "/unfollow", GenerwellConstants.Constants.AccessToken);
                return getResponse.ToString();
            }
            return string.Empty;
        }

    }
}
