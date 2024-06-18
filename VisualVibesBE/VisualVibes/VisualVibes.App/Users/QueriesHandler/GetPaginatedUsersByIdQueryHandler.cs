using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Models;

namespace VisualVibes.App.Users.QueriesHandler
{
    public class GetPaginatedUsersByIdQueryHandler : IRequestHandler<GetPaginatedUsersByIdQuery, PaginationResponseDto<ResponseUserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetPaginatedUsersByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetPaginatedUsersByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPaginatedUsersByIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<ResponseUserDto>> Handle(GetPaginatedUsersByIdQuery request, CancellationToken cancellationToken)
        {
            var paginatedUsers = await _unitOfWork.UserRepository.GetPaginatedUsersByIdAsync(request.paginationRequest.PageIndex, request.paginationRequest.PageSize);

            if (paginatedUsers.Items.Count == 0)
            {
                throw new UserNotFoundException($"There are no users on the platform!");
            }

            var mappedUsers = _mapper.Map<List<ResponseUserDto>>(paginatedUsers.Items);

            var response = new PaginationResponseDto<ResponseUserDto>(
                items: mappedUsers,
                pageIndex: paginatedUsers.PageIndex,
                totalPages: paginatedUsers.TotalPages
            );

            _logger.LogInformation($"Users successfully retrieved!");

            return response;
        }
    }
}
