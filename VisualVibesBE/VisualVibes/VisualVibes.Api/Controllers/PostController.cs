using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.Api.Extensions;
using VisualVibes.App.DTOs.PaginationDtos;
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
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDto createPostDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new CreatePostCommand(userId, createPostDto));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var response = await _mediator.Send(new GetPostByIdQuery(id));

            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(string userId, [FromQuery] PaginationRequestDto paginationRequest)
        {
            var response = await _mediator.Send(new GetPaginatedPostsWithInformationByUserIdQuery(userId, paginationRequest));
            return Ok(response);
        }

        [HttpGet("admin-posts")]
        public async Task<IActionResult> GetAdminPosts([FromQuery] PaginationRequestDto paginationRequest)
        {
            var query = new GetAdminPostsQuery(paginationRequest);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, UpdatePostDto updatePostDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var updatedPost = await _mediator.Send(new UpdatePostCommand(userId, id, updatePostDto));

            return Ok(updatedPost);
        }

        [HttpPut("{postId}/moderate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ModeratePost(Guid postId)
        {
            var response = await _mediator.Send(new ModeratePostCommand(postId));
            return Ok(response);
        }

        [HttpPut("{postId}/unmoderate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnmoderatePost(Guid postId)
        {
            var response = await _mediator.Send(new UnmoderatePostCommand(postId));
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePost(Guid id)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new RemovePostCommand(userId, id));

            return Ok(response);
        }
    }
}
