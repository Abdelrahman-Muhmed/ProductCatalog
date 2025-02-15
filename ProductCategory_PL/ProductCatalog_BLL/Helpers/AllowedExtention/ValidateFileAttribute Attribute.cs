using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ProductCatalog_BLL.Helpers.AllowedExtention
{
	public class ValidateFileAttribute : ValidationAttribute
	{
		private readonly string[] _extensions;
		private readonly long _maxFileSizeInBytes;

		public ValidateFileAttribute(string[] extensions, long maxFileSizeInMB)
		{
			_extensions = extensions;
			_maxFileSizeInBytes = maxFileSizeInMB * 1024 * 1024; // Convert MB to Bytes
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var file = value as IFormFile;
			if (file == null)
			{
				return ValidationResult.Success; 
			}

			var extension = Path.GetExtension(file.FileName)?.ToLower();
			if (string.IsNullOrEmpty(extension) || !_extensions.Contains(extension))
			{
				return new ValidationResult($"Only {string.Join(", ", _extensions)} file types are allowed.");
			}

			if (file.Length > _maxFileSizeInBytes)
			{
				return new ValidationResult($"File size must not exceed {_maxFileSizeInBytes / (1024 * 1024)} MB.");
			}

			return ValidationResult.Success;
		}
	}
}
