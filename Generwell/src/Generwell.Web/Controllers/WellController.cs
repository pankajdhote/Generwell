using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Mvc.Rendering;
using Generwell.Modules.Global;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class WellController : Controller
    {
        
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
                GlobalFields.previousPage = PageOrder.Welllisting.ToString();
                //change active menu class
                GlobalFields.SetMenu(Menu.Well.ToString());                                
                //fill Filters dropdown list
                WebClient webClient = new WebClient();
                var filterList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Filters, GlobalFields.AccessToken);
                List<FilterViewModel> filterViewModel = JsonConvert.DeserializeObject<List<FilterViewModel>>(filterList);
                ViewBag.FilterList = filterViewModel.Select(c => new SelectListItem { Text = c.name.ToString(), Value = c.id.ToString()}); 

                var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GlobalFields.AccessToken);
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
                    var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.WellFilter + "=" + id, GlobalFields.AccessToken);
                    wellViewModel = JsonConvert.DeserializeObject<List<WellViewModel>>(getWellList);
                }
                else
                {
                    var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GlobalFields.AccessToken);
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
                    var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well + "/" + id, GlobalFields.AccessToken);
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
        public async Task<PartialViewResult> Follow(string isFollow,string wellId, string filterId=null)
        {
            try
            {
                WebClient webClient = new WebClient();
                if (isFollow == GenerwellConstants.Constants.trueState)
                {
                    GlobalFields.IsFollow = GenerwellConstants.Constants.checkedState;
                    var getResponse = await webClient.PostWebApiData(GenerwellConstants.Constants.Well + "/" + wellId + "/follow", GlobalFields.AccessToken);

                    //get filtered wells
                    List<WellViewModel> wellViewModel=await GetFilteredWell(filterId);
                    return PartialView("_FilterWell", wellViewModel);
                    
                }
                else
                {
                    GlobalFields.IsFollow = GenerwellConstants.Constants.uncheckedState;
                    var getResponse = await webClient.DeleteWebApiData(GenerwellConstants.Constants.Well + "/" + wellId + "/unfollow", GlobalFields.AccessToken);

                    //get filtered wells
                    List<WellViewModel> wellViewModel = await GetFilteredWell(filterId);
                    return PartialView("_FilterWell", wellViewModel);
                }

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 29-11-2016
        /// Get Filtered well
        /// </summary>
        /// <returns></returns>
        public async Task<List<WellViewModel>> GetFilteredWell(string id = null)
        {
            try
            {
                WebClient webClient = new WebClient();
                List<WellViewModel> wellViewModel;
                if (!string.IsNullOrEmpty(id))
                {
                    var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.WellFilter + "=" + id, GlobalFields.AccessToken);
                    wellViewModel = JsonConvert.DeserializeObject<List<WellViewModel>>(getWellList);
                }
                else
                {
                    var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GlobalFields.AccessToken);
                    wellViewModel = JsonConvert.DeserializeObject<List<WellViewModel>>(getWellList);
                }
                return wellViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
