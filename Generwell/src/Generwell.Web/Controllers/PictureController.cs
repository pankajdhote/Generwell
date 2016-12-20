using System;
using AutoMapper;
using System.Text;
using Generwell.Core.Model;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Generwell.Modules.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Generwell.Modules.Management.PictureManagement;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Modules.GenerwellConstants;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class PictureController : BaseController
    {
        private readonly IPictureManagement _pictureManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;
        public PictureController(IPictureManagement pictureManagement, IMapper mapper, IGenerwellManagement generwellManagement)
        {
            _pictureManagement = pictureManagement;
            _mapper = mapper;
            _generwellManagement = generwellManagement;
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 16-11-2016
        /// Fetched all picture and display on screen..
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        public async Task<IActionResult> Index(string id)
        {
            try
            {
                AlbumModel albumModel = await _pictureManagement.GetPictureAlbum(Encoding.UTF8.GetString(Convert.FromBase64String(id)), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                AlbumViewModel albumViewModel = _mapper.Map<AlbumViewModel>(albumModel);
                return View("Index", albumViewModel);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Accounts Controller Login [POST] action method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Error", "Accounts");
            }
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 19-11-2016
        /// Fetched picture and display on screen for edit functionality..
        /// </summary>
        /// Need to Modify later
        /// <returns></returns>
        // GET: /<controller>/
        [AllowAnonymous]
        public async Task<ActionResult> EditPicture(string fileUrl, string label, string comment)
        {
            PictureModel pictureModel = await _pictureManagement.GetPicture(Encoding.UTF8.GetString(Convert.FromBase64String(fileUrl)), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
            PictureViewModel pictureViewModel = _mapper.Map<PictureViewModel>(pictureModel);
            pictureViewModel.label = Encoding.UTF8.GetString(Convert.FromBase64String(label));
            pictureViewModel.comment = Encoding.UTF8.GetString(Convert.FromBase64String(comment));
            return View("EditPicture", pictureViewModel);
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 20-12-2016
        /// Update picture..
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        [AllowAnonymous]
        public async Task<string> UpdatePicture(string content)
        {
            try
            {
                string taskDetailsResponse = await _pictureManagement.UpdatePicture(content, HttpContext.Session.GetString("TaskId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                return taskDetailsResponse;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskDetails Controller UpdateTaskFields action method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return response;
            }
        }

        /// <summary>
        /// Added by pankaj
        /// Date:- 20-12-2016
        /// Delete picture..
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        [AllowAnonymous]
        public async Task<string> DeletePicture(string pictureId)
        {
            try
            {
                string taskDetailsResponse = await _pictureManagement.DeletePicture(pictureId, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                return taskDetailsResponse;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskDetails Controller UpdateTaskFields action method.\"}";
                string response = await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return response;
            }
        }


        // GET: /<controller>/
        public IActionResult AddPicture()
        {
            return View();
        }

    }
}
