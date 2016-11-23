﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class WellController : Controller
    {
        
        /// <summary>
        /// Added by pankaj
        /// Date:- 13-11-2016
        /// Fetch all wells from web api and display on well list page.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                //change active menu class
                GenerwellConstants.Constants.WellActive = GenerwellConstants.Constants.Active;
                GenerwellConstants.Constants.TaskActive = string.Empty;
                GenerwellConstants.Constants.MapActive = string.Empty;

                //fill Filters dropdown list
                WebClient webClient = new WebClient();
                var filterList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Filters, GenerwellConstants.Constants.AccessToken);
                List<FilterViewModel> filterViewModel = JsonConvert.DeserializeObject<List<FilterViewModel>>(filterList);
                ViewBag.FilterList = filterViewModel.Select(c => new SelectListItem { Text = c.name.ToString(), Value = c.id.ToString()}); 

                var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GenerwellConstants.Constants.AccessToken);
                List<WellViewModel> wellViewModel = JsonConvert.DeserializeObject<List<WellViewModel>>(getWellList);              
                return View(wellViewModel);
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> FilterWell(string id)
        {
            try
            {
                WebClient webClient = new WebClient();
                List<WellViewModel> wellViewModel;
                if (!string.IsNullOrEmpty(id))
                {
                    var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.WellFilter + "=" + id, GenerwellConstants.Constants.AccessToken);
                    wellViewModel = JsonConvert.DeserializeObject<List<WellViewModel>>(getWellList);
                }
                else
                {
                    var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GenerwellConstants.Constants.AccessToken);
                    wellViewModel = JsonConvert.DeserializeObject<List<WellViewModel>>(getWellList);
                }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            try
            {
                WebClient webClient = new WebClient();
                WellViewModel wellViewModel=new WellViewModel();
                if (!string.IsNullOrEmpty(id))
                {
                    var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well + "/" + id, GenerwellConstants.Constants.AccessToken);
                    wellViewModel = JsonConvert.DeserializeObject<WellViewModel>(getWellList);
                }
                return View(wellViewModel);
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
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> Follow(string isFollow,string wellId )
        {
            string id = wellId;
            WebClient webClient = new WebClient();
            if (isFollow == GenerwellConstants.Constants.trueState)
            {
                GenerwellConstants.Constants.IsFollow = GenerwellConstants.Constants.checkedState;
                var getResponse = await webClient.PostWebApiData(GenerwellConstants.Constants.Well + "/" + id + "/follow", GenerwellConstants.Constants.AccessToken);
                return Json(getResponse);
            }
            else
            {
                GenerwellConstants.Constants.IsFollow = GenerwellConstants.Constants.uncheckedState;
                var getResponse = await webClient.DeleteWebApiData(GenerwellConstants.Constants.Well + "/" + id + "/unfollow", GenerwellConstants.Constants.AccessToken);
                return Json(getResponse);
            }
        }
    }
}
