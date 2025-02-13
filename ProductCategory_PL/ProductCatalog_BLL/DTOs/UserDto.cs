using System.ComponentModel.DataAnnotations;

namespace ProductCatalog_BLL.DTOs
{
    public class UserDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Token { get; set; }
    }
}
