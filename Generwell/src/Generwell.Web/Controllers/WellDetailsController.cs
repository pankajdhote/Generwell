using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.ViewModels;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Http;
using Generwell.Core.Model;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Generwell.Modules.Management;
using Generwell.Modules.GenerwellConstants;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class WellDetailsController : BaseController
    {
        private readonly IWellManagement _wellManagement;
        public WellDetailsController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices, IWellManagement wellManagement) : base(appSettings, generwellServices)
        {
            _wellManagement = wellManagement;
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 15-11-2016
        /// Display well Details for particular well.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string reportId)
        {
            try
            {
                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.WellDetails.ToString());
                LineReportsViewModel wellDetailsViewModel = await _wellManagement.GetWellDetailsByReportId(reportId, HttpContext.Session.GetString("WellId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
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
        /// </summary>
        /// <returns></returns>
        public async Task<string> Follow(string isFollow)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
