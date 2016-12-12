using System;
using Generwell.Modules;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Generwell.Modules.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Generwell.Modules.Services;
using System.Security.Claims;
using System.Collections.Generic;
using Generwell.Core.Model;
using Generwell.Modules.Management;
using Generwell.Modules.Management.GenerwellManagement;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{

    public class AccountsController : BaseController
    {
        private readonly IWellManagement _wellManagement;
        private readonly IGenerwellManagement _generwellManagement;
        public AccountsController(IWellManagement wellManagement, IGenerwellManagement generwellManagement)
        {
            _wellManagement = wellManagement;
            _generwellManagement = generwellManagement;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 10-11-2016
        /// Login get method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            HttpContext.Session.Clear();
            SignInViewModel signInModel = new SignInViewModel();
            return View(signInModel);
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 10-11-2016
        /// Login POST method
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login(SignInViewModel signInViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccessTokenViewModel accessTokenViewModel = await _generwellManagement.AuthenticateUser(signInViewModel.UserName, signInViewModel.Password, signInViewModel.WebApiUrl);
                    if (accessTokenViewModel.access_token != null)
                    {
                        //store access token in session
                        HttpContext.Session.SetString("AccessToken", accessTokenViewModel.access_token);
                        HttpContext.Session.SetString("TokenType", accessTokenViewModel.token_type);
                        //Fetch user name from api/v{apiVersion}/personnel/current api and disaply on every page.
                        ContactFieldsViewModel contactFieldRecord = await _generwellManagement.GetContactDetails(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        string userName = string.Format("{0} {1}", contactFieldRecord.firstName, contactFieldRecord.lastName);
                        HttpContext.Session.SetString("UserName", userName);
                        TempData["ServerError"] = string.Empty;
                        
                        //Authorize login user
                        if (contactFieldRecord != null)
                        {
                            await AuthorizeUser(contactFieldRecord);
                        }
                        return RedirectToAction("Index", "Well");
                    }
                    else
                    {                        
                        //string error = CreateLog("Authentication Failed");
                        TempData["ServerError"] = Resource.ErrorMessage_Credentials;
                    }
                }
                return View(signInViewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error","Accounts");
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 10-11-2016
        /// Display support popup
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PartialViewResult> Support()
        {
            SupportViewModel supportViewModel = await _generwellManagement.GetSupportDetails();
            return PartialView("_Support", supportViewModel);
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 21-11-2016
        /// Logout page functionality
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.Authentication.SignOutAsync("MyCookieMiddlewareInstance");
            return RedirectToAction("Login");
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 21-11-2016
        /// Logout page functionality
        /// </summary>
        /// <returns></returns>
        public async Task<string> AuthorizeUser(ContactFieldsViewModel contactFieldRecord)
        {
            List<Claim> userClaims = new List<Claim>
                            {
                                new Claim("userId", Convert.ToString(contactFieldRecord.id)),
                                new Claim(ClaimTypes.Name, contactFieldRecord.userName),
                                new Claim(ClaimTypes.Role, Convert.ToString(contactFieldRecord.id))
                            };
            ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "local"));
            await HttpContext.Authentication.SignInAsync("MyCookieMiddlewareInstance", principal);
            return string.Empty;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 08-12-2016
        /// Logout page functionality
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
    }
}
