using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Web.ViewModels;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Mvc.Rendering;
using Generwell.Modules.Global;
using Microsoft.AspNetCore.Http;
using Generwell.Modules.Model;
using Microsoft.Extensions.Options;
using Generwell.Modules.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class WellController : BaseController
    {
        public WellController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices) : base(appSettings, generwellServices)
        {
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
                List<FilterViewModel> filterViewModel = await GetFilters();
                ViewBag.FilterList = filterViewModel.Select(c => new SelectListItem { Text = c.name.ToString(), Value = c.id.ToString()});
                List<WellViewModel> wellList = await GetWells(null);
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
                List<WellViewModel> wellViewModel= await GetWells(id);
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
                    wellObj = await GetWellById(id);
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
        [HttpPost]
        public async Task<PartialViewResult> Follow(string isFollow,string wellId, string filterId=null)
        {
            try
            {
                string response = await SetFollowUnfollow(isFollow, wellId);
                //get filtered wells
                List<WellViewModel> wellViewModel = await GetWells(filterId);
                return PartialView("_FilterWell", wellViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
