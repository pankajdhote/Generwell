using System;
using System.Collections.Generic;
using System.Linq;
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
using AutoMapper;
using Generwell.Core.Model;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class MapController : BaseController
    {
        private readonly IWellManagement _wellManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;
        public MapController(IWellManagement wellManagement, IGenerwellManagement generwellManagement, IMapper mapper) : base(generwellManagement)
        {
            _wellManagement = wellManagement;
            _generwellManagement = generwellManagement;
            _mapper = mapper;
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
            try
            {
                MapViewModel emptyMapViewModel = new MapViewModel();
                //get previous page value for google map filteration
                int previousPageValue = (int)Enum.Parse(typeof(PageOrder), HttpContext.Session.GetString("previousPage"));
                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.Map.ToString());
                List<MapModel> wellForMapModel = new List<MapModel>();


                #region Get well data to display on map. 
                switch (previousPageValue)
                {
                    case 1:

                        if (HttpContext.Session.GetString("myWellCheck") == Constants.trueState && (!string.IsNullOrEmpty(HttpContext.Session.GetString("defaultFilter")) && HttpContext.Session.GetString("defaultFilter") != "null"))
                        {
                            wellForMapModel = await _wellManagement.GetWellsByFilterId(HttpContext.Session.GetString("defaultFilter"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                            if (wellForMapModel.Count() > 0)
                            {
                                wellForMapModel = wellForMapModel.Where(w => w.isFavorite == Convert.ToBoolean(HttpContext.Session.GetString("myWellCheck"))).ToList();
                            }
                        }
                        else if (HttpContext.Session.GetString("myWellCheck") == Constants.trueState)
                        {
                            wellForMapModel = await _wellManagement.GetWellsWithoutFilterId(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                            if (wellForMapModel.Count() > 0)
                            {
                                wellForMapModel = wellForMapModel.Where(w => w.isFavorite == Convert.ToBoolean(HttpContext.Session.GetString("myWellCheck"))).ToList();
                            }
                        }
                        else if (!string.IsNullOrEmpty(HttpContext.Session.GetString("defaultFilter")) && HttpContext.Session.GetString("defaultFilter") != "null")
                        {
                            wellForMapModel = await _wellManagement.GetWellsByFilterId(HttpContext.Session.GetString("defaultFilter"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        }
                        else
                        {
                            wellForMapModel = await _wellManagement.GetWellsWithoutFilterId(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        }

                        if (mapViewModelList.Count == 0)
                        {
                            mapViewModelList.Add(emptyMapViewModel);
                        }

                        break;
                    case 2:
                        wellForMapModel = await _wellManagement.GetWellsWithoutFilterId(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        wellForMapModel = wellForMapModel.Where(w => w.id == Convert.ToInt32(HttpContext.Session.GetString("WellId"))).ToList();
                        break;
                    case 3:
                        wellForMapModel = await _wellManagement.GetWellsWithoutFilterId(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        wellForMapModel = wellForMapModel.Where(w => w.id == Convert.ToInt32(HttpContext.Session.GetString("WellId"))).ToList();
                        break;
                    case 4:
                    case 12:
                        wellForMapModel = await _wellManagement.GetWellsWithoutFilterId(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        break;
                    default:
                        break;
                }
                mapViewModelList = _mapper.Map<List<MapViewModel>>(wellForMapModel);
                mapViewModelList = mapViewModelList.Where(w => w.latitude != null).ToList();
                return Json(mapViewModelList);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Map Controller Plot Marker action method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
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
        public JsonResult SetGooleMapObjects(string isMyWell, string filterId)
        {
            //find out previous page url                
            HttpContext.Session.SetString("myWellCheck", isMyWell);
            HttpContext.Session.SetString("defaultFilter", filterId != null ? filterId : string.Empty);
            return Json("success");
        }
    }
}
