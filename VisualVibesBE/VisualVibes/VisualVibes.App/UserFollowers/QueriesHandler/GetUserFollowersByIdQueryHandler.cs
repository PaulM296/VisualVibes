﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.UserFollowerDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserFollowers.Queries;

namespace VisualVibes.App.UserFollowers.QueriesHandler
{
    public class GetUserFollowersByIdQueryHandler : IRequestHandler<GetUserFollowersByIdQuery, IEnumerable<UserFollowerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserFollowersByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetUserFollowersByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserFollowersByIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserFollowerDto>> Handle(GetUserFollowersByIdQuery request, CancellationToken cancellationToken)
        {
            var followers = await _unitOfWork.UserFollowerRepository.GetFollowersByUserIdAsync(request.UserId);

            var followerDtos = _mapper.Map<IEnumerable<UserFollowerDto>>(followers);

            _logger.LogInformation("Followers successfully retrieved!");

            return followerDtos;
        }
    }
}