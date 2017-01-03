using System;
using AutoMapper;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Generwell.Core.Model;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Generwell.Modules.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Generwell.Modules.Management.PictureManagement;
using Generwell.Modules.Management.GenerwellManagement;
using Generwell.Modules.GenerwellConstants;
using System.Collections.Generic;
using Generwell.Modules.Management;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class PictureController : BaseController
    {
        private readonly IPictureManagement _pictureManagement;
        private readonly IGenerwellManagement _generwellManagement;
        private readonly IMapper _mapper;
        private readonly PictureModel _pictureModel;
        private readonly AlbumModel _albumModel;
        public PictureController(IPictureManagement pictureManagement, IMapper mapper,
            IGenerwellManagement generwellManagement,
            AlbumModel albumModel,
            PictureModel pictureModel) : base(generwellManagement)
        {
            _pictureManagement = pictureManagement;
            _mapper = mapper;
            _generwellManagement = generwellManagement;
            _pictureModel = pictureModel;
            _albumModel = albumModel;
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 16-11-2016
        /// Fetched all picture and display on screen..
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(string id, string fieldId = null, string flagCheck = null)
        {
            try
            {
                HttpContext.Session.SetString("FieldId", Encoding.UTF8.GetString(Convert.FromBase64String(fieldId != null ? fieldId : string.Empty)));
                if (!string.IsNullOrEmpty(id))
                {
                    AlbumModel albumModel = await _pictureManagement.GetPictureAlbum(Encoding.UTF8.GetString(Convert.FromBase64String(id != null ? id : string.Empty)), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                    AlbumViewModel albumViewModel = _mapper.Map<AlbumViewModel>(albumModel);
                    if (albumViewModel != null)
                    {
                        List<PictureViewModel> pictureViewModelList = (from p in albumViewModel.pictures select p).OrderBy(p => p.label).ToList();
                        albumViewModel.pictures = pictureViewModelList;
                        albumViewModel.flagCheck = Encoding.UTF8.GetString(Convert.FromBase64String(flagCheck != null ? flagCheck : string.Empty));
                        return View("Index", albumViewModel);
                    }
                }
                AlbumViewModel albumVM = _mapper.Map<AlbumViewModel>(_albumModel);
                albumVM.id = Encoding.UTF8.GetString(Convert.FromBase64String(id != null ? id : string.Empty));
                albumVM.flagCheck = Encoding.UTF8.GetString(Convert.FromBase64String(flagCheck != null ? flagCheck : string.Empty));
                return View("Index", albumVM);

            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Accounts Controller Login [POST] action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
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
        public async Task<ActionResult> EditPicture(string fileUrl, string label, string comment, string id, string albumId)
        {
            try
            {
                PictureModel pictureModel = await _pictureManagement.GetPicture(Encoding.UTF8.GetString(Convert.FromBase64String(fileUrl)), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                PictureViewModel pictureViewModel = _mapper.Map<PictureViewModel>(pictureModel);
                pictureViewModel.id = Encoding.UTF8.GetString(Convert.FromBase64String(id != null ? id : string.Empty));
                pictureViewModel.albumId = Encoding.UTF8.GetString(Convert.FromBase64String(albumId != null ? albumId : string.Empty));
                pictureViewModel.label = Encoding.UTF8.GetString(Convert.FromBase64String(label != null ? label : string.Empty));
                pictureViewModel.comment = Encoding.UTF8.GetString(Convert.FromBase64String(comment != null ? comment : string.Empty));
                pictureViewModel.fileUrl = Encoding.UTF8.GetString(Convert.FromBase64String(fileUrl != null ? fileUrl : string.Empty));
                return View("EditPicture", pictureViewModel);
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Accounts Controller Login [POST] action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Error", "Accounts");
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 20-12-2016
        /// Update picture..
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdatePicture(IFormFile file, PictureViewModel pictureViewModel)
        {
            try
            {
                PictureViewModel accessTokenModel;
                if (file != null)
                {
                    PictureModel pictureModel = _mapper.Map<PictureModel>(pictureViewModel);
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        byte[] imageByteArray = memoryStream.ToArray();
                        string taskDetailsResponse = await _pictureManagement.UpdatePicture(imageByteArray, pictureModel, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        accessTokenModel = JsonConvert.DeserializeObject<PictureViewModel>(taskDetailsResponse);
                    }
                }
                else
                {
                    string content = "[\"{ \"op\": \"replace\", \"path\": \"/Label\", \"value\": \"" + pictureViewModel.label + "\"},";
                    content += "{ \"op\": \"replace\", \"path\": \"/Comment\", \"value\": \"" + pictureViewModel.comment + "\"}\"]";
                    string taskDetailsResponse = await _pictureManagement.UpdatePictureDetails(content, pictureViewModel.id, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                    accessTokenModel = pictureViewModel;
                }
                return RedirectToAction("Index", "Picture", new { id = Convert.ToBase64String(Encoding.UTF8.GetBytes(accessTokenModel.albumId)), string.Empty, flagCheck = Convert.ToBase64String(Encoding.UTF8.GetBytes("Updated")) });
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in Picture Controller UpdatePicture action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Error", "Accounts");
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 20-12-2016
        /// Delete picture..
        /// </summary>
        /// <returns></returns>
        public async Task<string> DeletePicture(string pictureId, string albumId)
        {
            try
            {
                string taskDetailsResponse = await _pictureManagement.DeletePicture(pictureId, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                return taskDetailsResponse;
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskDetails Controller UpdateTaskFields action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return string.Empty;
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 20-12-2016
        /// Display add picture screen..
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddPicture(string albumId)
        {
            try
            {
                _pictureModel.albumId = Encoding.UTF8.GetString(Convert.FromBase64String(albumId != null ? albumId : string.Empty));
                PictureViewModel pictureViewModel = _mapper.Map<PictureViewModel>(_pictureModel);
                return View(pictureViewModel);
            }
            catch (Exception ex)
            {

                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskDetails Controller UpdateTaskFields action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Error", "Accounts");
            }
        }
        /// <summary>
        /// Added by pankaj
        /// Date:- 20-12-2016
        /// Upload selected picture file..
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file, PictureViewModel pictureViewModel)
        {
            try
            {
                if (file != null)
                {
                    PictureModel pictureModel = _mapper.Map<PictureModel>(pictureViewModel);
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        byte[] imageByteArray = memoryStream.ToArray();
                        string taskDetailsResponse = await _pictureManagement.AddPicture(imageByteArray, pictureModel, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        PictureViewModel responsePictureViewModel = JsonConvert.DeserializeObject<PictureViewModel>(taskDetailsResponse);
                        if (string.IsNullOrEmpty(pictureViewModel.albumId))
                        {
                            string content = "[\"{ \"op\": \"replace\", \"path\": \"/Fields/" + HttpContext.Session.GetString("FieldId")+ "\", \"value\": " + responsePictureViewModel.albumId + "}\"]";
                            string response = await _pictureManagement.UpdateTaskDetails(content, HttpContext.Session.GetString("TaskId"), HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
                        }
                        return RedirectToAction("Index", "Picture", new { id = Convert.ToBase64String(Encoding.UTF8.GetBytes(responsePictureViewModel.albumId)), string.Empty, flagCheck = Convert.ToBase64String(Encoding.UTF8.GetBytes("Uploaded")) });
                    }
                }
                return RedirectToAction("Index", "Picture", new { id = Convert.ToBase64String(Encoding.UTF8.GetBytes(pictureViewModel.albumId)), string.Empty, flagCheck = Convert.ToBase64String(Encoding.UTF8.GetBytes("Uploaded")) });
            }
            catch (Exception ex)
            {
                string logContent = "{\"message\": \"" + ex.Message + "\", \"callStack\": \"" + ex.InnerException + "\",\"comments\": \"Error Comment:- Error Occured in TaskDetails Controller UpdateTaskFields action method.\"}";
                await _generwellManagement.LogError(Constants.logShortType, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"), logContent);
                return RedirectToAction("Error", "Accounts");
            }
        }
    }
}
