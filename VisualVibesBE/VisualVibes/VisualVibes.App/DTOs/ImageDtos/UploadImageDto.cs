using Microsoft.AspNetCore.Http;

namespace VisualVibes.App.DTOs.ImageDtos
{
    public class UploadImageDto
    {
        public IFormFile File { get; set; }
        public string UserId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? UserProfileId { get; set; }
    }
}
