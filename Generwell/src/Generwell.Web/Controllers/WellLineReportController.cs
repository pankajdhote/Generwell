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
        [HttpGet]
        public async Task<ActionResult> Index(string wellId,string wellName,string isFollow)
        {
            TempData["WellId"] = wellId;
            TempData["WellName"] = wellName;
            TempData["IsFollow"] = isFollow;
            WebClient webClient = new WebClient();
            var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GenerwellConstants.Constants.AccessToken);
            List<WellViewModel> wellViewModel = JsonConvert.DeserializeObject<List<WellViewModel>>(getWellList);
            return View(wellViewModel);
        }


        public async Task<string> Follow(string id, string isFollow)
        {
            WebClient webClient = new WebClient();
            if (isFollow == "true")
            {
                var getResponse = await webClient.PostWebApiData(GenerwellConstants.Constants.Well + id + "/follow", GenerwellConstants.Constants.AccessToken);
            }
            else
            {
                var getResponse = await webClient.DeleteWebApiData(GenerwellConstants.Constants.Well + id + "/unfollow", GenerwellConstants.Constants.AccessToken);
                return getResponse.ToString();
            }
            return string.Empty;
        }

    }
}
