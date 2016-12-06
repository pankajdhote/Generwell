﻿using Generwell.Modules.Model;
using Generwell.Modules.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class PictureController : BaseController
    {
        public PictureController(IOptions<AppSettingsModel> appSettings, IGenerwellServices generwellServices) : base(appSettings, generwellServices)
        {
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult AddPicture()
        {
            return View();
        }

    }
}
