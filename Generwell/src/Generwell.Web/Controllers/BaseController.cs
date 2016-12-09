using System;
using System.Text;
using Generwell.Modules.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Generwell.Core.Model;
using System.Collections.Generic;
using Generwell.Modules.Services;
using Generwell.Modules.GenerwellConstants;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IGenerwellServices _generwellServices;
        public BaseController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices)
        {
            _appSettings = appSettings.Value;
            _generwellServices = generwellServices;
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 15-11-2016
        /// Authenticate user for successfull login
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AccessTokenViewModel> AuthenticateUser(string userName, string password, string webApiUrl)
        {
            try
            {
                string url = webApiUrl + "/" + _appSettings.Token;
                string webApiDetails = await _generwellServices.ProcessRequest(userName, password, url);
                AccessTokenViewModel accessTokenViewModel = JsonConvert.DeserializeObject<AccessTokenViewModel>(webApiDetails);
                return accessTokenViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all well from web api.
        /// </summary>
        /// <returns></returns>
     
        /// <summary>
        /// Added by pankaj
        /// Date:-01-12-2016
        /// Fetch all contact details from web api.
        /// </summary>
        /// <returns></returns>
        public async Task<ContactFieldsViewModel> GetContactDetails()
        {
            string personnelRecord = await _generwellServices.GetWebApiDetails(_appSettings.ContactDetails, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
            ContactFieldsViewModel contactFieldRecord = JsonConvert.DeserializeObject<ContactFieldsViewModel>(personnelRecord);
            return contactFieldRecord;
        }
      
       
       
    }
}
