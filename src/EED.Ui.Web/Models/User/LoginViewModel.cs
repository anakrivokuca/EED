using System.ComponentModel.DataAnnotations;

namespace EED.Ui.Web.Models.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter username.")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter password.")]
        public string Password { get; set; }
    }
}