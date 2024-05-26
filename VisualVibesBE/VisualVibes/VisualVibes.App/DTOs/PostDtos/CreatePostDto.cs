using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using VisualVibes.App.DTOs.ImageDtos;
using VisualVibes.App.Extensions;

namespace VisualVibes.App.DTOs.PostDtos
{
    public class CreatePostDto
    {
        [Required]
        [StringLength(1000, ErrorMessage = "Caption must have 1000 characters or fewer!")]
        public string Caption { get; set; }

        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg" })]
        public IFormFile? Image { get; set; }
    }
}
