using System;
using System.Text;
using Generwell.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.ViewModels;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Http;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Generwell.Modules.Management;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class WellLineReportController : BaseController
    {
        private readonly IWellManagement _wellManagement;
        public WellLineReportController(IWellManagement wellManagement)
        {
            _wellManagement = wellManagement;
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
                    HttpContext.Session.SetString("IsFollow", Encoding.UTF8.GetString(Convert.FromBase64String(isFollow)).ToLower() == Constants.trueState ? Constants.checkedState : string.Empty);
                }
                List<WellLineReportViewModel> wellLineReportViewModel = await _wellManagement.GetWellLineReports(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
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
        [HttpGet]
        public async Task<string> Follow(string isFollow)
        {
            //Need to change later
            if (isFollow == Constants.trueState)
            {
                HttpContext.Session.SetString("IsFollow", Constants.checkedState);
            }
            else
            {
                HttpContext.Session.SetString("IsFollow", Constants.uncheckedState);
            }
            string id = HttpContext.Session.GetString("WellId");
            string response = await _wellManagement.SetFollowUnfollow(isFollow, id, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
            return response;
        }
    }
}
