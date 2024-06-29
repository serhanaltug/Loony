using System.ComponentModel.DataAnnotations;

namespace Loony.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }
        [DataType(DataType.Password), StringLength(20, MinimumLength = 4)]
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
