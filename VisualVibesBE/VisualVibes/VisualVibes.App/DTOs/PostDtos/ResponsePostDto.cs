using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.PostDtos
{
    public class ResponsePostDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Caption { get; set; }
        public string Pictures { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ImageId { get; set; }
    }
}
