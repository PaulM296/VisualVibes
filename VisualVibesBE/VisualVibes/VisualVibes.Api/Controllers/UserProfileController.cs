﻿using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.DTOs.ImageDtos;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.App.UserProfiles.Queries;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/userProfiles")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserProfile([FromForm] CreateUserProfileDto createUserProfileDto)
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                var imageDto = new CreateImageDto
                {
                    Name = file.FileName,
                    Type = file.ContentType,
                    Data = file
                };

                createUserProfileDto.ProfilePicture = imageDto;
            }

            var userProfile = await _mediator.Send(new CreateUserProfileCommand(createUserProfileDto));

            return Created(string.Empty, userProfile);
        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateUserProfile(Guid id, UpdateUserProfileDto updateUserProfileDto)
        {
            var updatedUser = await _mediator.Send(new UpdateUserProfileCommand(id, updateUserProfileDto));

            return Ok(updatedUser);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfileByUserId(string id)
        {
            var userProfile = await _mediator.Send(new GetUserProfileByUserIdQuery(id));

            return Ok(userProfile);
        }
    }
}
