using Generwell.Modules.Management.GenerwellManagement;
using Microsoft.AspNetCore.Mvc;
using Generwell.Core.Model;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Generwell.Modules.Global;
using Generwell.Modules.GenerwellEnum;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class FacilityController : BaseController
    {
        private readonly AppSettingsModel _appSettings;
        public FacilityController(IGenerwellManagement generwellManagement, IOptions<AppSettingsModel> appSettings) : base(generwellManagement)
        {
            _appSettings = appSettings.Value;

        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {

            //set previous page value for google map filteration
            HttpContext.Session.SetString("previousPage", PageOrder.Facilitylisting.ToString());
            //change active menu class
            GlobalFields.SetMenu(Menu.Well.ToString());           

            //Create License for well
            if (HttpContext.Session.GetString("ModuleId") != "14")
            {
                //Release license
                string releaseLicense = await ReleaseLicense(HttpContext.Session.GetString("LicenseHandleId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                LicenseModel licenseModel = await CreateLicense(_appSettings.Facilities, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                //set LicenseHandleId and moduleId.
                if (licenseModel != null)
                {
                    HttpContext.Session.SetString("LicenseHandleId", licenseModel.handleId);
                    HttpContext.Session.SetString("ModuleId", licenseModel.moduleId);
                }
            }
           
            return View();
        }
    }
}
