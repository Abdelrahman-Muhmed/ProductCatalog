using System.ComponentModel.DataAnnotations;

namespace ProductCatalog_BLL.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string PassWord { get; set; } = null!;
    }
}
