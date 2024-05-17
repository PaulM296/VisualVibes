using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly IIdentityService _identityService;

        public RegisterUserCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork,
            ILogger<RegisterUserCommandHandler> logger, IIdentityService identityService)
        {
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _identityService = identityService;
        }
        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new AppUser
            {
                UserName = request.registerUser.UserName,
                Email = request.registerUser.Email,
                Role = request.registerUser.Role
            };

            var newUserProfile = new UserProfile
            {
                FirstName = request.registerUser.FirstName,
                LastName = request.registerUser.LastName,
                DateOfBirth = request.registerUser.DateOfBirth,
                Bio = request.registerUser.Bio,
                ProfilePicture = request.registerUser.ProfilePicture
            };

            var createdUser = await _authenticationService.Register(newUser, newUserProfile, request.registerUser.Password);

            // Ensure a feed is created for the new user
            var feed = new Feed 
            { 
                UserID = createdUser.Id 
            };
            await _unitOfWork.FeedRepository.AddAsync(feed);
            await _unitOfWork.FeedPostRepository.EnsureFeedForUserAsync(newUser.Id);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("New user successfully added!");

            var claims = _identityService.CreateClaimsIdentity(createdUser);

            return _identityService.CreateSecurityToken(claims);
        }
    }
}
