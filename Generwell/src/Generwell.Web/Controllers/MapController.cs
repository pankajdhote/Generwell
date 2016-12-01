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
using Generwell.Modules.Global;

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
            GlobalFields.SetMenu(Menu.Map.ToString());
            List<MapViewModel> mapViewModelList = new List<MapViewModel>();
            return View(mapViewModelList);
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 22-11-2016
        /// Plot markers for well on google map
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        public async Task<JsonResult> PlotMarker()
        {
            try
            {               
                WebClient webClient = new WebClient();
                List<MapViewModel> mapViewModelList = new List<MapViewModel>();
                MapViewModel emptyMapViewModel = new MapViewModel();
                
                //get previous page value for google map filteration
                int previousPageValue = (int)Enum.Parse(typeof(PageOrder), GlobalFields.previousPage);
                //set previous page value for google map filteration
                GlobalFields.previousPage = PageOrder.Map.ToString();

                #region Switch
                switch (previousPageValue)
                {
                    case 1:

                        if (GlobalFields.myWellCheck == GenerwellConstants.Constants.trueState && (!string.IsNullOrEmpty(GlobalFields.defaultFilter)))
                        {
                            var wellRecord1 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.WellFilter + "=" + GlobalFields.defaultFilter, GlobalFields.AccessToken);
                            mapViewModelList = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord1);
                            if (mapViewModelList.Count() > 0)
                            {
                                mapViewModelList = mapViewModelList.Where(w => w.isFavorite == Convert.ToBoolean(GlobalFields.myWellCheck)).ToList();
                            }
                        }
                        else if (GlobalFields.myWellCheck == GenerwellConstants.Constants.trueState)
                        {
                            var wellRecord2 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GlobalFields.AccessToken);
                            mapViewModelList = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord2);
                            if (mapViewModelList.Count() > 0)
                            {
                                mapViewModelList = mapViewModelList.Where(w => w.isFavorite == Convert.ToBoolean(GlobalFields.myWellCheck)).ToList();
                            }
                        }
                        else if (!string.IsNullOrEmpty(GlobalFields.defaultFilter))
                        {
                            var wellRecord3 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.WellFilter + "=" + GlobalFields.defaultFilter, GlobalFields.AccessToken);
                            mapViewModelList = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord3);
                        }
                        else
                        {
                            var wellRecord4 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GlobalFields.AccessToken);
                            mapViewModelList = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord4);
                        }                       

                        if (mapViewModelList.Count == 0)
                        {
                            mapViewModelList.Add(emptyMapViewModel);
                        }

                        break;
                    case 2:
                        var wellRecord5 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GlobalFields.AccessToken);
                        mapViewModelList = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord5);
                        mapViewModelList = mapViewModelList.Where(w => w.id == Convert.ToInt32(GlobalFields.WellId)).ToList();
                        break;
                    case 3:
                        var wellRecord6 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GlobalFields.AccessToken);
                        mapViewModelList = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord6);
                        mapViewModelList = mapViewModelList.Where(w => w.id == Convert.ToInt32(GlobalFields.WellId)).ToList();
                        break;
                    case 4:
                    case 12:
                        var wellRecord7 = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GlobalFields.AccessToken);
                        mapViewModelList = JsonConvert.DeserializeObject<List<MapViewModel>>(wellRecord7);
                        break;
                    default:
                        break;
                }
                mapViewModelList = mapViewModelList.Where(w => w.latitude != null).ToList();

                return Json(mapViewModelList);
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
            GlobalFields.myWellCheck = isMyWell;
            GlobalFields.defaultFilter = filterId;
            return Json("success");
        }
    }
}
