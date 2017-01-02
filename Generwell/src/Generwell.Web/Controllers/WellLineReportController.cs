using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.ViewModels;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Generwell.Modules.Management;
using Generwell.Modules.Global;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Core.Model;
using AutoMapper;
using Microsoft.Extensions.Options;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class WellLineReportController : BaseController
    {
        private readonly IWellManagement _wellManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;
        private readonly AppSettingsModel _appSettings;
        public WellLineReportController(IWellManagement wellManagement, IGenerwellManagement generwellManagement, IMapper mapper, IOptions<AppSettingsModel> appSettings) : base(generwellManagement)
        {
            _wellManagement = wellManagement;
            _generwellManagement = generwellManagement;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 15-11-2016
        /// Display well line reports for particular well.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string wellId, string wellName, string isFollow, string latitude, string longitude)
        {
            try
            {
                //Create License for well
                if (HttpContext.Session.GetString("ModuleId") != "1")
                {
                    await ApplyLicense(_appSettings.Well);
                }
                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.WellLineReports.ToString());
                //change active menu class
                GlobalFields.SetMenu(Menu.Well.ToString());
                if (!string.IsNullOrEmpty(wellId))
                {
                    HttpContext.Session.SetString("WellId", Encoding.UTF8.GetString(Convert.FromBase64String(wellId)));
                    HttpContext.Session.SetString("WellName", Encoding.UTF8.GetString(Convert.FromBase64String(wellName)));
                    HttpContext.Session.SetString("IsFollow", Encoding.UTF8.GetString(Convert.FromBase64String(isFollow)).ToLower() == Constants.trueState ? Constants.checkedState : string.Empty);
                    HttpContext.Session.SetString("Latitude", Encoding.UTF8.GetString(Convert.FromBase64String(latitude != null ? latitude : "bnVsbA==")));
                    HttpContext.Session.SetString("Longitude", Encoding.UTF8.GetString(Convert.FromBase64String(longitude != null ? longitude : "bnVsbA==")));
                }
                List<WellLineReportModel> wellLineReportModel = await _wellManagement.GetWellLineReports(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                List<WellLineReportViewModel> wellLineReportViewModel = _mapper.Map<List<WellLineReportViewModel>>(wellLineReportModel);
                return View(wellLineReportViewModel);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellLineReports Controller Index action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Error", "Accounts");
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
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellLineReports Controller Follow action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return string.Empty;
            }

        }
    }
}
