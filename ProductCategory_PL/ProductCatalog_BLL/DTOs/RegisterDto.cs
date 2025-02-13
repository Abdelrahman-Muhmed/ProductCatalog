using System.ComponentModel.DataAnnotations;

namespace ProductCatalog_BLL.DTOs
{
    public class RegisterDto
    {
		[Required(ErrorMessage = "First name is required")]
		[StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
		public string firstName { get; set; } = null!;

		[Required(ErrorMessage = "Last name is required")]
		[StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
		public string lastName { get; set; } = null!;

		[Required(ErrorMessage = "Role is required")]
		public string Role { get; set; } = null!;

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; } = null!;

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[RegularExpression(
			@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
			ErrorMessage = "Password must be at least 8 characters, include an uppercase letter, a lowercase letter, a number, and a special character"
		)]
		public string Password { get; set; } = null!;

		[Required(ErrorMessage = "Confirm Password is required")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		public string ConfirmPassword { get; set; } = null!;

		[Required(ErrorMessage = "Phone number is required")]
		[Phone(ErrorMessage = "Invalid phone number format")]
		public string phoneNumber { get; set; } = null!;

		[Required(ErrorMessage = "Country is required")]
		[StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
		public string country { get; set; } = null!;

		[Required(ErrorMessage = "City is required")]
		[StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
		public string city { get; set; } = null!;
	}
}
