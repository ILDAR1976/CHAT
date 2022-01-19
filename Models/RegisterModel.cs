using System.ComponentModel.DataAnnotations;

namespace Chat.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указано максимальное количиство сообщений")]
        public string MQM { get; set; }
    }
}
