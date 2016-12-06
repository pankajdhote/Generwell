using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Web.ViewModels;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Http;
using System.Text;
using Generwell.Modules.Model;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class WellLineReportController : BaseController
    {
        public WellLineReportController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices) : base(appSettings, generwellServices)
        {
        }

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
                HttpContext.Session.SetString("previousPage", PageOrder.WellLineReports.ToString());

                if (!string.IsNullOrEmpty(wellId))
                {
                    HttpContext.Session.SetString("WellId", Encoding.UTF8.GetString(Convert.FromBase64String(wellId)));
                    HttpContext.Session.SetString("WellName", Encoding.UTF8.GetString(Convert.FromBase64String(wellName)));
                    HttpContext.Session.SetString("IsFollow", Encoding.UTF8.GetString(Convert.FromBase64String(isFollow)).ToLower() == GenerwellConstants.Constants.trueState ? GenerwellConstants.Constants.checkedState : string.Empty);
                }
                List<WellLineReportViewModel> wellLineReportViewModel = await GetWellLineReports();
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
            string id = HttpContext.Session.GetString("WellId");
            string response = await SetFollowUnfollow(isFollow, id);
            return response;
        }
    }
}
