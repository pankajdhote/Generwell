using Generwell.Modules.GenerwellConstants;
using Generwell.Modules.Management.GenerwellManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Generwell.Core.Model;
using Microsoft.AspNetCore.Http;
using Generwell.Modules.ViewModels;
using AutoMapper;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IGenerwellManagement _generwellManagement;
        public BaseController(IGenerwellManagement generwellManagement)
        {
            _generwellManagement = generwellManagement;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 23-11-2016
        /// Keep License alive after 50 seconds.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task KeepLicenseAlive()
        {
            try
            {
                if (HttpContext.Session.GetString("LicenseHandleId") != null)
                {
                    await _generwellManagement.KeepLicenseAlive(HttpContext.Session.GetString("LicenseHandleId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                }
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Well Controller FilterWell action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 23-11-2016
        /// Create Licenses
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<LicenseModel> CreateLicense(string url, string accessToken, string tokenType)
        {
            try
            {
                LicenseModel licenseModel = await _generwellManagement.CreateLicense(url, accessToken, tokenType);
                return licenseModel;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Base Controller Create License action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return new LicenseModel();
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 23-11-2016
        /// Release License
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ReleaseLicense(string id, string accessToken, string tokenType)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    string response = await _generwellManagement.ReleaseLicense(id, accessToken, tokenType);
                    return response;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Base Controller Release License action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return string.Empty;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 27-12-2016
        /// Display support popup
        /// </summary>
        /// <returns></returns>
        public async Task<PartialViewResult> DisplaySupportPopup()
        {
            SupportModel supportModel = await _generwellManagement.GetSupportDetails();            
            return PartialView("_Support", supportModel);
        }

    }
}
