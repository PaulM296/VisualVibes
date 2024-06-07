using MediatR;
using VisualVibes.App.DTOs.ImageDtos;

namespace VisualVibes.App.Images.Queries
{
    public record GetImageByIdQuery(Guid id) : IRequest<ImageResponseDto>;
}
