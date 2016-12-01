using Generwell.Modules;
using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.Global;
using Generwell.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class BaseController : Controller
    {

        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well line reports from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetWellLineReports()
        {
            WebClient webClient = new WebClient();
            var wellLineReportList = await webClient.GetWebApiDetails(GenerwellConstants.Constants.WellLineReports, GlobalFields.AccessToken);
            return View();
        }

        public async Task<ContactFieldsViewModel> GetContactDetails()
        {
            WebClient webClient = new WebClient();
            var personnelRecord = await webClient.GetWebApiDetails(GenerwellConstants.Constants.ContactDetails, GlobalFields.AccessToken);
            ContactFieldsViewModel contactFieldRecord = JsonConvert.DeserializeObject<ContactFieldsViewModel>(personnelRecord);
            return contactFieldRecord;
        }        
    }
}
