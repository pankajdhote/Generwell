using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Generwell.Modules.GenerwellConstants;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellEnum;
using Generwell.Modules.Global;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class WellLineReportController : Controller
    {
        /// <summary>
        /// Added by pankaj
        /// Date:- 15-11-2016
        /// Display well line reports for particular well.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string wellId, string wellName, string isFollow)
        {
            try
            {
                //set previous page value for google map filteration
                GlobalFields.previousPage = PageOrder.WellLineReports.ToString();
                if (!string.IsNullOrEmpty(wellId))
                {
                    GlobalFields.WellId = wellId;
                    GlobalFields.WellName = wellName;
                    GlobalFields.IsFollow = isFollow.ToLower() == GenerwellConstants.Constants.trueState ? isFollow = GenerwellConstants.Constants.checkedState : null;
                }
                WebClient webClient = new WebClient();
                var wellLineReportList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.WellLineReports, GlobalFields.AccessToken);
                List<WellLineReportViewModel> wellLineReportViewModel = JsonConvert.DeserializeObject<List<WellLineReportViewModel>>(wellLineReportList);
                return View(wellLineReportViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 14-11-2016
        /// follow or unfollow well by id
        /// </summary>
        /// <returns></returns>
        public async Task<string> Follow(string isFollow)
        {
            string id = GlobalFields.WellId;
            WebClient webClient = new WebClient();
            if (isFollow == GenerwellConstants.Constants.trueState)
            {
                GlobalFields.IsFollow = GenerwellConstants.Constants.checkedState;
                var getResponse = await webClient.PostWebApiData(GenerwellConstants.Constants.Well + "/" + id + "/follow", GlobalFields.AccessToken);
            }
            else
            {
                GlobalFields.IsFollow = GenerwellConstants.Constants.uncheckedState;
                var getResponse = await webClient.DeleteWebApiData(GenerwellConstants.Constants.Well + "/" + id + "/unfollow", GlobalFields.AccessToken);
                return getResponse.ToString();
            }
            return string.Empty;
        }
    }
}
