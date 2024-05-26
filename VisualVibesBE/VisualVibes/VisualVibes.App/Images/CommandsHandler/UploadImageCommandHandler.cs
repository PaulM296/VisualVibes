using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.ImageDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Images.Commands;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Images.CommandsHandler
{
    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, ImageResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UploadImageCommandHandler> _logger;

        public UploadImageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UploadImageCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ImageResponseDto> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            if(request.uploadImageDto.File == null || request.uploadImageDto.File.Length == 0)
            {
                throw new ImageNotFoundException("The image has not been found!");
            }

            if(!IsValidImageFormat(request.uploadImageDto.File.ContentType))
            {
                throw new InvalidImageFormatException("Invalid file format! Only .png and .jpeg formats are allowed!");
            }

            using var memoryStream = new MemoryStream();
            await request.uploadImageDto.File.CopyToAsync(memoryStream, cancellationToken);

            var image = new Image
            {
                Name = request.uploadImageDto.File.FileName,
                Type = request.uploadImageDto.File.ContentType,
                Data = memoryStream.ToArray()
            };

            await _unitOfWork.ImageRepository.UploadImage(image);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Image successfully uploaded!");

            return _mapper.Map<ImageResponseDto>(image);
        }

        private bool IsValidImageFormat(string contentType)
        {
            return contentType == "image/jpeg" || contentType == "image/png" || contentType == "image/jpg";
        }
    }
}
