using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.ViewModels;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Generwell.Modules.Management;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Core.Model;
using AutoMapper;
using Microsoft.Extensions.Options;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class WellDetailsController : BaseController
    {
        private readonly IWellManagement _wellManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;
        private readonly AppSettingsModel _appSettings;

        public WellDetailsController(IWellManagement wellManagement, IGenerwellManagement generwellManagement, IMapper mapper, IOptions<AppSettingsModel> appSettings) : base(generwellManagement)
        {
            _wellManagement = wellManagement;
            _generwellManagement = generwellManagement;
            _mapper = mapper;
            _appSettings = appSettings.Value;

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
                //Create License for well
                if (HttpContext.Session.GetString("ModuleId") != "1")
                {
                    //Release license
                    string releaseLicense = await ReleaseLicense(HttpContext.Session.GetString("LicenseHandleId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                    LicenseModel licenseModel = await CreateLicense(_appSettings.Well, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                    //set LicenseHandleId and moduleId.
                    if (licenseModel != null)
                    {
                        HttpContext.Session.SetString("LicenseHandleId", licenseModel.handleId);
                        HttpContext.Session.SetString("ModuleId", licenseModel.moduleId);
                    }
                }

                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.WellDetails.ToString());
                LineReportsModel wellDetailsModel = await _wellManagement.GetWellDetailsByReportId(reportId, HttpContext.Session.GetString("WellId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                LineReportsViewModel wellDetailsViewModel = _mapper.Map<LineReportsViewModel>(wellDetailsModel);
                return View(wellDetailsViewModel.fields);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellDetails Controller Index action method.\"}";
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
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellDetails Controller Follow action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return string.Empty;
            }
        }
    }
}
