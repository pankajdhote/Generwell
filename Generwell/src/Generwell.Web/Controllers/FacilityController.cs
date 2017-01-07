using Generwell.Modules.Management.GenerwellManagement;
using Microsoft.AspNetCore.Mvc;
using Generwell.Core.Model;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Generwell.Modules.Global;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Generwell.Modules.Management.FacilityManagement;
using AutoMapper;
using Generwell.Modules.ViewModels;
using System;
using Generwell.Modules.GenerwellConstants;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class FacilityController : BaseController
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IMapper _mapper;
        private readonly IFacilityManagement _facilityManagement;
        private readonly IGenerwellManagement _generwellManagement;
        public FacilityController(IMapper mapper,
            IFacilityManagement facilityManagement,
            IGenerwellManagement generwellManagement,
            IOptions<AppSettingsModel> appSettings) : base(generwellManagement)
        {
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _facilityManagement = facilityManagement;
            _generwellManagement = generwellManagement;

        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {

            try
            {
                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.Facilitylisting.ToString());
                //change active menu class
                GlobalFields.SetMenu(Menu.Facility.ToString());

                //Create License for well
                if (HttpContext.Session.GetString("ModuleId") != "14")
                {
                    await ApplyLicense(_appSettings.Facilities);
                }

                List<FacilityModel> facilityList = await _facilityManagement.GetFacilities(null, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                List<FacilityViewModel> facilityViewModelList = _mapper.Map<List<FacilityViewModel>>(facilityList);
                return View(facilityViewModelList);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Well Controller Index action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Error", "Accounts");
            }
        }
    }
}
