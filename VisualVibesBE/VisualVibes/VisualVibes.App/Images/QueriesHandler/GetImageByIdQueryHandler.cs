using AutoMapper;
using MediatR;
using VisualVibes.App.DTOs.ImageDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Images.Queries;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Images.QueriesHandler
{
    public class GetImageByIdQueryHandler : IRequestHandler<GetImageByIdQuery, ImageResponseDto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetImageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ImageResponseDto> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
        {
            var image = await _unitOfWork.ImageRepository.GetImageById(request.id);

            if (image == null)
            {
                throw new ImageNotFoundException("Image not found");
            }

            return _mapper.Map<ImageResponseDto>(image);
        }
    }
}
