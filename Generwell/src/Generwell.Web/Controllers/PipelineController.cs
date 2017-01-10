using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generwell.Modules.Management.GenerwellManagement;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Generwell.Web.Controllers
{
    public class PipelineController : BaseController
    {
        public PipelineController(IGenerwellManagement generwellManagement) : base(generwellManagement)
        {
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
