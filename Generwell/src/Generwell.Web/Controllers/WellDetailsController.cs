using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Web.ViewModels;
using Generwell.Modules.GenerwellEnum;
using Microsoft.AspNetCore.Http;
using Generwell.Modules.Model;
using Generwell.Modules.Services;
using Microsoft.Extensions.Options;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class WellDetailsController : BaseController
    {
        public WellDetailsController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices) : base(appSettings, generwellServices)
        {
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 15-11-2016
        /// Display well Details for particular well.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string reportId)
        {
            try
            {
                //set previous page value for google map filteration
                HttpContext.Session.SetString("previousPage", PageOrder.WellDetails.ToString());
                LineReportsViewModel wellDetailsViewModel = await GetWellDetailsByReportId(reportId);
                return View(wellDetailsViewModel.fields);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 14-11-2016
        /// follow or unfollow well by id
        /// </summary>
        /// <returns></returns>
        public async Task<string> Follow(string isFollow)
        {
            try
            {
                string id = HttpContext.Session.GetString("WellId");
                string response = await SetFollowUnfollow(isFollow, id);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
