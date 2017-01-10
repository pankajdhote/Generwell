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
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                //set latitude for google map
                HttpContext.Session.SetString("IsFollow", "All");
                HttpContext.Session.SetString("Latitude", "All");
                HttpContext.Session.SetString("Longitude", "All");
                //change active menu class
                GlobalFields.SetMenu(Menu.Facility.ToString());

                //Create License for well
                if (HttpContext.Session.GetString("ModuleId") != "14")
                {
                    await ApplyLicense(_appSettings.Facilities);
                }

                //fill Filters dropdown list
                List<FilterModel> filterModel = await _generwellManagement.GetFilters(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                List<FilterViewModel> filterViewModel = _mapper.Map<List<FilterViewModel>>(filterModel);
                ViewBag.FilterList = filterViewModel.Select(c => new SelectListItem { Text = c.name.ToString(), Value = c.id.ToString() });
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
        /// <summary>
        /// Added by pankaj
        /// Date:- 18-11-2016
        /// filter wells by filter id.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> FilterFacility(string id)
        {
            try
            {
                List<FacilityModel> facilityModelList = await _facilityManagement.GetFacilities(id, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                List<FacilityViewModel> facilityViewModelList = _mapper.Map<List<FacilityViewModel>>(facilityModelList);
                return PartialView("_FilterFacility", facilityViewModelList);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in facility Controller FilterFacility action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return PartialView("_FilterFacility");
            }
        }
        [HttpGet]
        public async Task<PartialViewResult> SetFollowUnfollow(string isFollow, string facilityId, string filterId)
        {
            try
            {
                if (isFollow == Constants.trueState)
                {
                    HttpContext.Session.SetString("IsFollow", Constants.checkedState);
                }
                else
                {
                    HttpContext.Session.SetString("IsFollow", Constants.uncheckedState);
                }
                string response = await _generwellManagement.SetFollowUnfollow(_appSettings.Facilities, isFollow, facilityId, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                //get filtered wells
                List<FacilityModel> facilityModel = await _facilityManagement.GetFacilities(filterId, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                List<FacilityViewModel> facilityViewModelList = _mapper.Map<List<FacilityViewModel>>(facilityModel);
                return PartialView("_FilterFacility", facilityViewModelList);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Facility Controller FilterFacility action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return PartialView("_FilterWell");
            }
        }
    }
}
