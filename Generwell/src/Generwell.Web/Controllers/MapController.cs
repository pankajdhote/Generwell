using System;
using System.Collections.Generic;
using System.Linq;
using Generwell.Modules;
using Newtonsoft.Json;
using Generwell.Web.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.GenerwellEnum;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class MapController : Controller
    {
        /// <summary>
        /// Added by pankaj
        /// Date:- 22-11-2016
        /// Fetch all wells from web api and display on map.
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        public ActionResult Index()
        {
            //set menu
            GenerwellConstants.Constants.setMenu(Menu.Map.ToString());
            List<MapViewModel> mapViewModel = new List<MapViewModel>();
            return View(mapViewModel);
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 22-11-2016
        /// plot markers for well on google map
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        public async Task<JsonResult> PlotMarker()
        {
            try
            {               
                WebClient webClient = new WebClient();
                List<MapViewModel> mapViewModel = new List<MapViewModel>();
                MapViewModel emptyMapViewModel = new MapViewModel();
                
                //get previous page value for google map filteration
                int previousPageValue = (int)Enum.Parse(typeof(PageOrder), GenerwellConstants.Constants.previousPage);
                //set previous page value for google map filteration
                GenerwellConstants.Constants.previousPage = PageOrder.Map.ToString();

                #region Switch
                switch (previousPageValue)
                {
                    case 1:

                        if (GenerwellConstants.Constants.myWellCheck == GenerwellConstants.Constants.trueState && ((!string.IsNullOrEmpty(GenerwellConstants.Constants.defaultFilter))))
                        {
                            var wellRecord1 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.WellFilter + "=" + GenerwellConstants.Constants.defaultFilter, GenerwellConstants.Constants.AccessToken);
                            mapViewModel = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord1);
                            if (mapViewModel.Count() > 0)
                            {
                                mapViewModel = mapViewModel.Where(w => w.isFavorite == Convert.ToBoolean(GenerwellConstants.Constants.myWellCheck)).ToList();
                            }
                        }
                        else if (GenerwellConstants.Constants.myWellCheck == GenerwellConstants.Constants.trueState)
                        {
                            var wellRecord2 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GenerwellConstants.Constants.AccessToken);
                            mapViewModel = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord2);
                            if (mapViewModel.Count() > 0)
                            {
                                mapViewModel = mapViewModel.Where(w => w.isFavorite == Convert.ToBoolean(GenerwellConstants.Constants.myWellCheck)).ToList();
                            }
                        }
                        else if (!string.IsNullOrEmpty(GenerwellConstants.Constants.defaultFilter))
                        {
                            var wellRecord3 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.WellFilter + "=" + GenerwellConstants.Constants.defaultFilter, GenerwellConstants.Constants.AccessToken);
                            mapViewModel = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord3);
                        }
                        else
                        {
                            var wellRecord4 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GenerwellConstants.Constants.AccessToken);
                            mapViewModel = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord4);
                        }                       

                        if (mapViewModel.Count == 0)
                        {
                            mapViewModel.Add(emptyMapViewModel);
                        }

                        break;
                    case 2:
                        var wellRecord5 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GenerwellConstants.Constants.AccessToken);
                        mapViewModel = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord5);
                        mapViewModel = mapViewModel.Where(w => w.id == Convert.ToInt32(GenerwellConstants.Constants.WellId)).ToList();
                        break;
                    case 3:
                        var wellRecord6 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GenerwellConstants.Constants.AccessToken);
                        mapViewModel = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord6);
                        mapViewModel = mapViewModel.Where(w => w.id == Convert.ToInt32(GenerwellConstants.Constants.WellId)).ToList();
                        break;
                    case 4:
                    case 12:
                        var wellRecord7 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GenerwellConstants.Constants.AccessToken);
                        mapViewModel = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord7);
                        break;
                    default:
                        break;
                }
                mapViewModel = mapViewModel.Where(w => w.latitude != null).ToList();
                return Json(mapViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            #endregion
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 25-11-2016
        /// set google map objects
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        public JsonResult SetGooleMapObjects(string isMyWell, string filterId)
        {
            //find out previous page url                
            GenerwellConstants.Constants.myWellCheck = isMyWell;
            GenerwellConstants.Constants.defaultFilter = filterId;

            return Json("success");
        }

    }
}
