using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ResponsePostDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreatePostCommandHandler> _logger;
        private readonly IMapper _mapper;
        public CreatePostCommandHandler(IUnitOfWork unitOfWork, ILogger<CreatePostCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ResponsePostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            //if (!IsValidImageFormat(request.createPostDto.Image.Type))
            //{
            //    throw new InvalidImageFormatException("Unsupported image format. Only PNG, JPEG, and JPG are supported.");
            //}

            //byte[] imageData;
            //using (var memoryStream = new MemoryStream())
            //{
            //    await request.createPostDto.Image.Data.CopyToAsync(memoryStream);
            //    imageData = memoryStream.ToArray();
            //}

            //var image = new Image
            //{
            //    Name = request.createPostDto.Image.Name,
            //    Type = request.createPostDto.Image.Type,
            //    Data = imageData
            //};

            //await _unitOfWork.ImageRepository.UploadImage(image);

            Image image = null;
            if (request.createPostDto.Image != null)
            {
                // Validate image format
                var allowedFormats = new[] { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(request.createPostDto.Image.FileName).ToLower();
                if (!allowedFormats.Contains(fileExtension))
                {
                    throw new InvalidImageFormatException("Invalid image format. Only .png, .jpg, and .jpeg are allowed.");
                }

                // Convert image to binary format
                byte[] imageData;
                using (var memoryStream = new MemoryStream())
                {
                    await request.createPostDto.Image.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }

                image = new Image
                {
                    Name = request.createPostDto.Image.FileName,
                    Type = request.createPostDto.Image.Name,
                    Data = imageData
                };

                await _unitOfWork.ImageRepository.UploadImage(image);
                await _unitOfWork.SaveAsync(); // Ensure the image is saved before referencing it
            }

            var post = new Post()
            {
                UserId = request.userId,
                Caption = request.createPostDto.Caption,
                CreatedAt = DateTime.UtcNow,
                ImageId = image.Id,
                Image = image
            };

            var createdPost = await _unitOfWork.PostRepository.AddAsync(post);

            await _unitOfWork.FeedPostRepository.AddPostToFeedAsync(createdPost.Id);

            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Post successfully created!");

            return _mapper.Map<ResponsePostDto>(createdPost);
        }

        private bool IsValidImageFormat(string imageType)
        {
            var supportedFormats = new[] { "image/png", "image/jpeg", "image/jpg" };
            return supportedFormats.Contains(imageType.ToLower());
        }
    }
}
