using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Modules.Management.FacilityManagement;
using AutoMapper;
using Generwell.Core.Model;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Generwell.Modules.GenerwellEnum;
using Generwell.Modules.ViewModels;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.Global;

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class FacilityDetailsController : BaseController
    {
        private readonly IFacilityManagement _facilityManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;
        private readonly AppSettingsModel _appSettings;
        public FacilityDetailsController(IFacilityManagement facilityManagement,
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
        /// Display well Details for particular well.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string reportId)
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
                HttpContext.Session.SetString("previousPage", PageOrder.FacilityDetails.ToString());
                LineReportsModel facilityDetailsModel = await _generwellManagement.GetWellDetailsByReportId(reportId, HttpContext.Session.GetString("FacilityId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                LineReportsViewModel facilityDetailsViewModel = _mapper.Map<LineReportsViewModel>(facilityDetailsModel);
                return View(facilityDetailsViewModel.fields);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in WellDetails Controller Index action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Error", "Accounts");
            }
        }
    }
}
