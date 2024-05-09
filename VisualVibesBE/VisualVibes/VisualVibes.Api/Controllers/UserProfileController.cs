using MediatR;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.App.UserProfiles.Queries;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserProfile(CreateUserProfileDto createUserProfileDto)
        {
            var userProfile = await _mediator.Send(new CreateUserProfileCommand(createUserProfileDto));

            return Ok(userProfile);
        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateUserProfile(Guid id, UpdateUserProfileDto updateUserProfileDto)
        {
            var updatedUser = await _mediator.Send(new UpdateUserProfileCommand(id, updateUserProfileDto));

            return Ok(updatedUser);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfileByUserId(Guid id)
        {
            var userProfile = await _mediator.Send(new GetUserProfileByUserIdQuery(id));

            return Ok(userProfile);
        }
    }
}
