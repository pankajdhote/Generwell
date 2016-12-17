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
       
        // GET: /<controller>/
        public IActionResult AddPicture()
        {
            return View();
        }

    }
}
