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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class WellController : BaseController
    {
        private readonly IWellManagement _wellManagement;
        public WellController(IWellManagement wellManagement)
        {
            _wellManagement = wellManagement;
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
                List<FilterViewModel> filterViewModel = await _wellManagement.GetFilters(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                ViewBag.FilterList = filterViewModel.Select(c => new SelectListItem { Text = c.name.ToString(), Value = c.id.ToString() });
                List<WellViewModel> wellList = await _wellManagement.GetWells(null, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                return View(wellList);
            }
            catch (Exception ex)
            {
                throw ex;
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
                List<WellViewModel> wellViewModel = await _wellManagement.GetWells(id, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                return PartialView("_FilterWell", wellViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
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
                WellViewModel wellObj = new WellViewModel();
                if (!string.IsNullOrEmpty(id))
                {
                    wellObj = await _wellManagement.GetWellById(id, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                }
                return View(wellObj);
            }
            catch (Exception ex)
            {
                throw ex;
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
                List<WellViewModel> wellViewModel = await _wellManagement.GetWells(filterId, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                return PartialView("_FilterWell", wellViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
