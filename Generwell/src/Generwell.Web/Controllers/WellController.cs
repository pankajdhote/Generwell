using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.ViewModels;
using Generwell.Modules.Global;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Generwell.Modules.Services;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Generwell.Modules.Management;
using Generwell.Core.Model;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.Management.GenerwellManagement;
using AutoMapper;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class WellController : BaseController
    {
        private readonly IWellManagement _wellManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;
        public WellController(IWellManagement wellManagement, 
            IGenerwellManagement generwellManagement, IMapper mapper)
        {
            _wellManagement = wellManagement;
            _generwellManagement = generwellManagement;
            _mapper = mapper;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 13-11-2016
        /// Fetch all wells from web api and display on well list page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.Welllisting.ToString());
                //change active menu class
                GlobalFields.SetMenu(Menu.Well.ToString());
                //fill Filters dropdown list
                List<FilterModel> filterModel = await _wellManagement.GetFilters(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                List<FilterViewModel> filterViewModel = _mapper.Map<List<FilterViewModel>>(filterModel);

                ViewBag.FilterList = filterViewModel.Select(c => new SelectListItem { Text = c.name.ToString(), Value = c.id.ToString() });
                List<WellModel> wellList = await _wellManagement.GetWells(null, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                List<WellViewModel> wellViewModelList = _mapper.Map<List<WellViewModel>>(wellList);

                return View(wellViewModelList);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Well Controller Index action method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
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
        public async Task<PartialViewResult> FilterWell(string id)
        {
            try
            {
                List<WellModel> wellModelList = await _wellManagement.GetWells(id, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                List<WellViewModel> wellViewModelList = _mapper.Map<List<WellViewModel>>(wellModelList);
                return PartialView("_FilterWell", wellViewModelList);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Well Controller FilterWell action method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return PartialView("_FilterWell");
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 21-11-2016
        /// display well details from task details page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            try
            {
                WellModel wellObj = new WellModel();
                if (!string.IsNullOrEmpty(id))
                {
                    wellObj = await _wellManagement.GetWellById(id, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                    List<WellViewModel> wellViewModelList = _mapper.Map<List<WellViewModel>>(wellObj);
                    return View(wellViewModelList);
                }
                return View();
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Well Controller Details action method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Error","Accounts");
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 22-11-2016
        /// follow or unfollow well by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> SetFollowUnfollow(string isFollow, string wellId, string filterId)
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
                string response = await _wellManagement.SetFollowUnfollow(isFollow, wellId, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                //get filtered wells
                List<WellModel> wellModel = await _wellManagement.GetWells(filterId, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                List<WellViewModel> wellViewModelList = _mapper.Map<List<WellViewModel>>(wellModel);
                return PartialView("_FilterWell", wellViewModelList);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Well Controller FilterWell action method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return PartialView("_FilterWell");
            }
        }
    }
}
