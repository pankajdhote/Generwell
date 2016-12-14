using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Generwell.Modules.Management.PictureManagement;
using Generwell.Core.Model;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Generwell.Modules.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "MyCookieMiddlewareInstance")]
    public class PictureController : BaseController
    {
        private readonly IPictureManagement _pictureManagement;
        private readonly IMapper _mapper;
        public PictureController(IPictureManagement pictureManagement, IMapper mapper) 
        {
            _pictureManagement = pictureManagement;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index(string id)
        {
            AlbumModel albumModel = await _pictureManagement.GetPictureAlbum(id, HttpContext.Session.GetString("AccessToken"), HttpContext.Session.GetString("TokenType"));
            AlbumViewModel albumViewModel = _mapper.Map<AlbumViewModel>(albumModel);
            return View(albumViewModel);
        }
        // GET: /<controller>/
        public IActionResult AddPicture()
        {
            return View();
        }

    }
}
