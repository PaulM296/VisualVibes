using System.ComponentModel.DataAnnotations;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.FeedDtos
{
    public class CreateFeedDto
    {
        [Required]
        public string UserID { get; set; }
    }
}
