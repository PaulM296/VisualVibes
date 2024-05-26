using MediatR;
using VisualVibes.App.DTOs.ImageDtos;

namespace VisualVibes.App.Images.Commands
{
    public record UploadImageCommand(UploadImageDto uploadImageDto) : IRequest<ImageResponseDto>;
}
