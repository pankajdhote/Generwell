using System;
using System.Collections.Generic;
using System.Linq;
using Generwell.Web.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.GenerwellEnum;
using Generwell.Modules.Global;
using Microsoft.AspNetCore.Http;
using Generwell.Modules.Model;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class MapController : BaseController
    {
        public MapController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices) : base(appSettings, generwellServices)
        {
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
            try
            {               
                List<MapViewModel> mapViewModelList = new List<MapViewModel>();
                MapViewModel emptyMapViewModel = new MapViewModel();
                //get previous page value for google map filteration
                int previousPageValue = (int)Enum.Parse(typeof(PageOrder), HttpContext.Session.GetString("previousPage"));
                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.Map.ToString());

                List<MapViewModel> wellsByFilterId=new List<MapViewModel>();
                List<MapViewModel> wellsWithoutFilterId = new List<MapViewModel>();

                #region Switch
                switch (previousPageValue)
                {
                    case 1:

                        if (HttpContext.Session.GetString("myWellCheck") == GenerwellConstants.Constants.trueState && (!string.IsNullOrEmpty(HttpContext.Session.GetString("defaultFilter")) && HttpContext.Session.GetString("defaultFilter")!="null"))
                        {
                            wellsByFilterId = await GetWellsByFilterId();
                            if (wellsByFilterId.Count() > 0)
                            {
                                mapViewModelList = wellsByFilterId.Where(w => w.isFavorite == Convert.ToBoolean(HttpContext.Session.GetString("myWellCheck"))).ToList();
                            }
                        }
                        else if (HttpContext.Session.GetString("myWellCheck") == GenerwellConstants.Constants.trueState)
                        {
                            wellsWithoutFilterId = await GetWellsWithoutFilterId();
                            if (wellsWithoutFilterId.Count() > 0)
                            {
                                mapViewModelList = wellsWithoutFilterId.Where(w => w.isFavorite == Convert.ToBoolean(HttpContext.Session.GetString("myWellCheck"))).ToList();
                            }
                        }
                        else if (!string.IsNullOrEmpty(HttpContext.Session.GetString("defaultFilter")) && HttpContext.Session.GetString("defaultFilter") != "null")
                        {
                            wellsByFilterId = await GetWellsByFilterId();
                            mapViewModelList = wellsByFilterId;
                        }
                        else
                        {
                            wellsWithoutFilterId = await GetWellsWithoutFilterId();
                            mapViewModelList = wellsWithoutFilterId;
                        }                       

                        if (mapViewModelList.Count == 0)
                        {
                            mapViewModelList.Add(emptyMapViewModel);
                        }

                        break;
                    case 2:
                        wellsWithoutFilterId = await GetWellsWithoutFilterId();
                        mapViewModelList = wellsWithoutFilterId.Where(w => w.id == Convert.ToInt32(HttpContext.Session.GetString("WellId"))).ToList();
                        break;
                    case 3:
                        wellsWithoutFilterId = await GetWellsWithoutFilterId();
                        mapViewModelList = wellsWithoutFilterId.Where(w => w.id == Convert.ToInt32(HttpContext.Session.GetString("WellId"))).ToList();
                        break;
                    case 4:
                    case 12:
                        wellsWithoutFilterId = await GetWellsWithoutFilterId();
                        mapViewModelList = wellsWithoutFilterId;
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
            HttpContext.Session.SetString("myWellCheck",isMyWell);
            HttpContext.Session.SetString("defaultFilter", filterId!=null? filterId:string.Empty);
            return Json("success");
        }
    }
}
