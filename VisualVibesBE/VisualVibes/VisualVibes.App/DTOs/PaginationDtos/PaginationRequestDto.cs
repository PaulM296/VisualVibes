using System.ComponentModel.DataAnnotations;

namespace VisualVibes.App.DTOs.PaginationDtos
{
    public class PaginationRequestDto
    {
        [Required]
        public int PageIndex { get; set; }

        public int PageSize { get; set; } = 10;
    }
}
