using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Posts.Queries;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/activityPosts")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostDto createPostDto)
        {
            var response = await _mediator.Send(new CreatePostCommand(createPostDto));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var response = await _mediator.Send(new GetPostByIdQuery(id));

            return Ok(response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, UpdatePostDto updatePostDto)
        {
            var updatedPost = await _mediator.Send(new UpdatePostCommand(id, updatePostDto));

            return Ok(updatedPost);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePost(Guid id)
        {
            var response = await _mediator.Send(new RemovePostCommand(id));

            return Ok(response);
        }
    }
}
