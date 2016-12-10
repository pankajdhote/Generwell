using System.ComponentModel.DataAnnotations;

namespace Generwell.Modules.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Server { get; set; }
        [Required(ErrorMessage = "Url is required")]
        public string WebApiUrl { get; set; }

    }
}
