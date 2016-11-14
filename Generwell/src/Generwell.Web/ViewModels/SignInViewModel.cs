using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Generwell.Web.ViewModels
{
    public class SignInViewModel
    {
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please Use letters only.")]
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please Use letters only.")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Server { get; set; }
        [Required(ErrorMessage = "Url is required")]
        public string WebApiUrl { get; set; }
    }
}
