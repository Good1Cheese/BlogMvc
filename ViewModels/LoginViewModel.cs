using System.ComponentModel.DataAnnotations;

namespace BlogMvc.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
    }
}
