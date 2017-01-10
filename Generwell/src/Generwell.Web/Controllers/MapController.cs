using System;
using AutoMapper;
using System.Linq;
using Generwell.Core.Model;
using System.Collections.Generic;
using Generwell.Modules.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.GenerwellEnum;
using Generwell.Modules.Global;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Generwell.Modules.Management;
using Generwell.Modules.Management.GenerwellManagement;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class MapController : BaseController
    {
        private readonly IWellManagement _wellManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;
        private readonly AppSettingsModel _appSettings;
        public MapController(IWellManagement wellManagement,
            IGenerwellManagement generwellManagement,
            IOptions<AppSettingsModel> appSettings,
            IMapper mapper) : base(generwellManagement)
        {
            _wellManagement = wellManagement;
            _generwellManagement = generwellManagement;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 22-11-2016
        /// Fetch all wells from web api and display on map.
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        public ActionResult Index()
        {
            //get previous page value for google map filteration
            //int previousPageValue = (int)Enum.Parse(typeof(PageOrder), HttpContext.Session.GetString("previousPage"));
            if (HttpContext.Session.GetString("previousPage") == PageOrder.Map.ToString())
            {
                //set latitude for google map
                HttpContext.Session.SetString("Latitude", "All");
                HttpContext.Session.SetString("Longitude", "All");
            }
            //Set Map menu Active after menu click.
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
            List<MapViewModel> mapViewModelList = new List<MapViewModel>();
            List<MapModel> mapModelList = new List<MapModel>();
            try
            {
                MapViewModel emptyMapViewModel = new MapViewModel();
                //get previous page value for google map filteration
                int previousPageValue = (int)Enum.Parse(typeof(PageOrder), HttpContext.Session.GetString("previousPage"));
                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.Map.ToString());
                List<MapModel> assetsForMapModel = new List<MapModel>();

                string assetsUrlFilterId = string.Empty;
                string assetsUrl = string.Empty;
                int id = 0;
                //Set assets url
                switch (previousPageValue)
                {
                    case 1:
                    case 2:
                    case 3:
                        assetsUrlFilterId = _appSettings.Well + "?filterId";
                        assetsUrl = _appSettings.Well;
                        id = Convert.ToInt32(HttpContext.Session.GetString("WellId"));
                        break;
                    case 14:
                    case 15:
                    case 16:
                        assetsUrlFilterId = _appSettings.Facilities + "?filterId";
                        assetsUrl = _appSettings.Facilities;
                        id = Convert.ToInt32(HttpContext.Session.GetString("FacilityId"));
                        break;
                    case 12:
                        if (HttpContext.Session.GetString("ModuleId") == ((int)Enum.Parse(typeof(PageOrder), PageOrder.Welllisting.ToString())).ToString())
                        {
                            assetsUrl = _appSettings.Well;
                        }
                        else if (HttpContext.Session.GetString("ModuleId") == ((int)Enum.Parse(typeof(PageOrder), PageOrder.Facilitylisting.ToString())).ToString())
                        {
                            assetsUrl = _appSettings.Facilities;
                        }
                        id = Convert.ToInt32(HttpContext.Session.GetString("WellId"));
                        break;
                    default:
                        break;
                }


                #region Get well data to display on map. 
                switch (previousPageValue)
                {
                    case 1:
                    case 14:
                        if (HttpContext.Session.GetString("myAssets") == Constants.trueState && (!string.IsNullOrEmpty(HttpContext.Session.GetString("defaultFilter")) && HttpContext.Session.GetString("defaultFilter") != Constants.NullValue))
                        {
                            assetsForMapModel = await _generwellManagement.GetAssetsByFilterId(assetsUrlFilterId, HttpContext.Session.GetString("defaultFilter"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                            if (assetsForMapModel.Count() > 0)
                            {
                                assetsForMapModel = assetsForMapModel.Where(w => w.isFavorite == Convert.ToBoolean(HttpContext.Session.GetString("myAssets"))).ToList();
                            }
                        }
                        else if (HttpContext.Session.GetString("myAssets") == Constants.trueState)
                        {
                            assetsForMapModel = await _generwellManagement.GetAssetsWithoutFilterId(assetsUrl, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                            if (assetsForMapModel.Count() > 0)
                            {
                                assetsForMapModel = assetsForMapModel.Where(w => w.isFavorite == Convert.ToBoolean(HttpContext.Session.GetString("myAssets"))).ToList();
                            }
                        }
                        else if (!string.IsNullOrEmpty(HttpContext.Session.GetString("defaultFilter")) && HttpContext.Session.GetString("defaultFilter") != Constants.NullValue)
                        {
                            assetsForMapModel = await _generwellManagement.GetAssetsByFilterId(assetsUrlFilterId, HttpContext.Session.GetString("defaultFilter"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        }
                        else
                        {
                            assetsForMapModel = await _generwellManagement.GetAssetsWithoutFilterId(assetsUrl, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        }

                        if (mapViewModelList.Count == 0)
                        {
                            mapViewModelList.Add(emptyMapViewModel);
                        }

                        break;
                    case 2:
                    case 15:
                        assetsForMapModel = await _generwellManagement.GetAssetsWithoutFilterId(assetsUrl, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        mapModelList = assetsForMapModel.Where(w => w.isFavorite == true).ToList();
                        bool isExist = mapModelList.Any(x => x.id == id);
                        if (!isExist)
                        {
                            assetsForMapModel = assetsForMapModel.Where(w => w.id == id).ToList();
                        }
                        else
                        {
                            assetsForMapModel.Clear();
                        }
                        assetsForMapModel.AddRange(mapModelList);
                        break;
                    case 3:
                    case 16:
                        assetsForMapModel = await _generwellManagement.GetAssetsWithoutFilterId(assetsUrl, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        mapModelList = assetsForMapModel.Where(w => w.isFavorite == true).ToList();
                        assetsForMapModel = assetsForMapModel.Where(w => w.id == id).ToList();
                        bool isExistCheck = mapModelList.Any(x => x.id == id);
                        if (!isExistCheck)
                        {
                            assetsForMapModel = assetsForMapModel.Where(w => w.id == id).ToList();
                        }
                        else
                        {
                            assetsForMapModel.Clear();
                        }
                        assetsForMapModel.AddRange(mapModelList);
                        break;
                    case 4:
                    case 12:
                        assetsForMapModel = await _generwellManagement.GetAssetsWithoutFilterId(assetsUrl, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        break;
                    default:
                        break;
                }
                mapViewModelList = _mapper.Map<List<MapViewModel>>(assetsForMapModel);
                mapViewModelList = mapViewModelList.Where(w => w.latitude != null).ToList();
                return Json(mapViewModelList);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Map Controller Plot Marker action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return Json(mapViewModelList);
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
        public JsonResult SetGooleMapObjects(string isMyAssets, string filterId)
        {
            //find out previous page url                
            HttpContext.Session.SetString("myAssets", isMyAssets);
            HttpContext.Session.SetString("defaultFilter", filterId != null ? filterId : string.Empty);
            return Json("success");
        }
    }
}
