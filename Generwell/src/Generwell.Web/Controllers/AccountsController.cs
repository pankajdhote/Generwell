using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.Authorization;
using Generwell.Web.ViewModels;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Generwell.Modules;
using Generwell.Modules.GenerwellConstants;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class AccountsController : Controller
    {

        /// <summary>
        /// Added by pankaj
        /// Date:- 10-11-2016
        /// Login get method
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 10-11-2016
        /// Login POST method
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login(SignInViewModel signInViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    Authorization authorizeUser = new Authorization();
                    var responseMsg = await authorizeUser.AuthenticateUser(signInViewModel.UserName, signInViewModel.Password, signInViewModel.WebApiUrl);
                    AccessTokenViewModel accessTokenViewModel = JsonConvert.DeserializeObject<AccessTokenViewModel>(responseMsg);
                    if (accessTokenViewModel.access_token != null)
                    {
                        GenerwellConstants.Constants.AccessToken = accessTokenViewModel.access_token;
                        GenerwellConstants.Constants.TokenType = accessTokenViewModel.token_type;

                        //Fetch user name from api/v{apiVersion}/personnel/current api and disaply on every page.
                        WebClient webClient = new WebClient();
                        var personnelRecord = await webClient.GetWebApiDetails(GenerwellConstants.Constants.ContactDetails, GenerwellConstants.Constants.AccessToken);
                        ContactFieldsViewModel contactFieldRecord = JsonConvert.DeserializeObject<ContactFieldsViewModel>(personnelRecord);
                        GenerwellConstants.Constants.UserName = contactFieldRecord.firstName+" "+contactFieldRecord.lastName;

                        TempData["ServerError"] = "";
                        return RedirectToAction("Index", "Well");
                    }
                    else
                    {
                        TempData["ServerError"] = "UserName or Password is incorrect";
                    }
                }
                return View(signInViewModel);
        }
            catch (Exception ex)
            {
                throw ex;
            }
}

        /// <summary>
        /// Added by pankaj
        /// Date:- 10-11-2016
        /// Display support popup
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> Support()
        {
            WebClient webClient = new WebClient();
            var getContactDetails = await webClient.GetWebApiDetails(GenerwellConstants.Constants.Support,null);
            ContactDetailsViewModel contactDetailsViewModel = JsonConvert.DeserializeObject<ContactDetailsViewModel>(getContactDetails);
            return PartialView("_Support", contactDetailsViewModel);
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 21-11-2016
        /// Logout page functionality
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
