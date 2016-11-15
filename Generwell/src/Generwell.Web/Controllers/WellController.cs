using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using Generwell.Modules.GenerwellConstants;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class WellController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                //fill Filters dropdown list
                WebClient webClient = new WebClient();
                var filterList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Filters, GenerwellConstants.Constants.AccessToken);
               // List<WellViewModel> wellViewModel = JsonConvert.DeserializeObject<List<WellViewModel>>(filterList);


                var getWellList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Well, GenerwellConstants.Constants.AccessToken);
                List<WellViewModel> wellViewModel = JsonConvert.DeserializeObject<List<WellViewModel>>(getWellList);
                //if (isFavorite != null)
                //{
                //    wellViewModel = wellViewModel.Where(w => w.isFavorite == Convert.ToBoolean(isFavorite)).ToList();
                //}
                return View(wellViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
