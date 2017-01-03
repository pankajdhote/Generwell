using Generwell.Modules;
using Generwell.Modules.Management;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Modules.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Generwell.Modules.GenerwellConstants;
using Generwell.Core.Model;
using AutoMapper;
using System.Text;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{

    public class AccountsController : BaseController
    {
        private readonly IWellManagement _wellManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;
        public AccountsController(IWellManagement wellManagement, IGenerwellManagement generwellManagement, IMapper mapper) : base(generwellManagement)
        {
            _wellManagement = wellManagement;
            _generwellManagement = generwellManagement;
            _mapper = mapper;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 10-11-2016
        /// Login get method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login(string error)
        {
            HttpContext.Session.Clear();
            string errorString = Encoding.UTF8.GetString(Convert.FromBase64String(error != null ? error : string.Empty));
            if (!string.IsNullOrEmpty(errorString))
            {
                HttpContext.Session.SetString("ServerError", errorString);
            }
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
                    AccessTokenModel accessTokenModel = await _generwellManagement.AuthenticateUser(signInViewModel.UserName, signInViewModel.Password, signInViewModel.WebApiUrl);
                    AccessTokenViewModel accessTokenViewModel = _mapper.Map<AccessTokenViewModel>(accessTokenModel);

                    if (accessTokenViewModel != null && accessTokenViewModel.access_token != null)
                    {
                        //store access token in session
                        HttpContext.Session.SetString("AccessToken", accessTokenViewModel.access_token);
                        HttpContext.Session.SetString("TokenType", accessTokenViewModel.token_type);
                        //Fetch user name from api/v{apiVersion}/personnel/current api and disaply on every page.
                        ContactFieldsModel contactFieldModelRecord = await _generwellManagement.GetContactDetails(HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        ContactFieldsViewModel contactFieldRecord = _mapper.Map<ContactFieldsViewModel>(contactFieldModelRecord);

                        string userName = string.Format("{0} {1}", contactFieldRecord.firstName, contactFieldRecord.lastName);
                        HttpContext.Session.SetString("UserName", userName);

                        //Authorize login user
                        if (contactFieldRecord != null)
                        {
                            await AuthorizeUser(contactFieldRecord);
                        }
                        return RedirectToAction("Index", "Well");
                    }
                    else
                    {
                        HttpContext.Session.SetString("ServerError", accessTokenViewModel.error_description);
                        return RedirectToAction("Login", "Accounts", new { error = Convert.ToBase64String(Encoding.UTF8.GetBytes(HttpContext.Session.GetString("ServerError") != null ? HttpContext.Session.GetString("ServerError") : string.Empty)) });
                    }
                }
                return View(signInViewModel);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("ServerError", ex.Message);
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Accounts Controller Login [POST] action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Login", "Accounts", new { error = Convert.ToBase64String(Encoding.UTF8.GetBytes(HttpContext.Session.GetString("ServerError"))) });
            }
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
            try
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
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Accounts Controller Login [POST] action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
            }
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
