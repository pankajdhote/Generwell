using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Modules.Management.FacilityManagement;
using AutoMapper;
using Generwell.Core.Model;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Generwell.Modules.GenerwellEnum;
using Generwell.Modules.Global;
using System.Text;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class FacilityLineReportController : BaseController
    {
        private readonly IFacilityManagement _facilityManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;
        private readonly AppSettingsModel _appSettings;
        public FacilityLineReportController(IFacilityManagement facilityManagement,
        IGenerwellManagement generwellManagement,
        IMapper mapper, 
        IOptions<AppSettingsModel> appSettings) : base(generwellManagement)
        {
            _facilityManagement = facilityManagement;
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
        public async Task<ActionResult> Index(string facilityId, string facilityName, string isFollow, string latitude, string longitude)
        {
            try
            {
                int previousPageValue = (int)Enum.Parse(typeof(PageOrder), HttpContext.Session.GetString("previousPage"));
                //Create License for facility
                if (HttpContext.Session.GetString("ModuleId") != previousPageValue.ToString())
                {
                    await ApplyLicense(_appSettings.Facilities);
                }
                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.FacilityLineReport.ToString());
                if (!string.IsNullOrEmpty(facilityId))
                {
                    HttpContext.Session.SetString("FacilityId", Encoding.UTF8.GetString(Convert.FromBase64String(facilityId)));
                    HttpContext.Session.SetString("FacilityName", Encoding.UTF8.GetString(Convert.FromBase64String(facilityName)));
                    HttpContext.Session.SetString("IsFollow", Encoding.UTF8.GetString(Convert.FromBase64String(isFollow)).ToLower() == Constants.trueState ? Constants.checkedState : string.Empty);
                    HttpContext.Session.SetString("Latitude", Encoding.UTF8.GetString(Convert.FromBase64String(latitude != null ? latitude : Constants.setLatitude)));
                    HttpContext.Session.SetString("Longitude", Encoding.UTF8.GetString(Convert.FromBase64String(longitude != null ? longitude : Constants.setLongitude)));
                }
                List<FacilityLineReportModel> facilityLineReportModel = await _facilityManagement.GetFacilityLineReports(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                List<FacilityLineReportViewModel> facilityLineReportViewModel = _mapper.Map<List<FacilityLineReportViewModel>>(facilityLineReportModel);
                return View(facilityLineReportViewModel);
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
                string id = HttpContext.Session.GetString("FacilityId");
                string response = await _generwellManagement.SetFollowUnfollow(_appSettings.Facilities, isFollow, id, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                return response;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in FacilityLineReports Controller Follow action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return string.Empty;
            }

        }
    }
}
