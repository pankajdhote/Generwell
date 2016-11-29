using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;
using Generwell.Web.ViewModels;
using Generwell.Modules.GenerwellEnum;

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
            try
            {
                //set previous page value for google map filteration
                GenerwellConstants.Constants.previousPage = PageOrder.WellDetails.ToString();

            WebClient webClient = new WebClient();
            var wellDetailsList = await webClient.GetWebApiWithTimeZone(GenerwellConstants.Constants.Well+"/"+ GenerwellConstants.Constants.WellId + "/linereports/"+reportId, GenerwellConstants.Constants.AccessToken);
            LineReportsViewModel wellDetailsViewModel = JsonConvert.DeserializeObject<LineReportsViewModel>(wellDetailsList);
            return View(wellDetailsViewModel.fields);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> Follow(string isFollow)
        {
            try
            {            
            string id = GenerwellConstants.Constants.WellId;
            WebClient webClient = new WebClient();
            if (isFollow == GenerwellConstants.Constants.trueState)
            {
                GenerwellConstants.Constants.IsFollow = GenerwellConstants.Constants.checkedState;
                var getResponse = await webClient.PostWebApiData(GenerwellConstants.Constants.Well + "/" + id + "/follow", GenerwellConstants.Constants.AccessToken);
            }
            else
            {
                GenerwellConstants.Constants.IsFollow = GenerwellConstants.Constants.uncheckedState;
                var getResponse = await webClient.DeleteWebApiData(GenerwellConstants.Constants.Well + "/" + id + "/unfollow", GenerwellConstants.Constants.AccessToken);
                return getResponse.ToString();
            }
            return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
