using Moq;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.Comments.CommandsHandler;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.Comments
{
    public class UpdateCommentCommandHandlerUnitTest
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private UpdateCommentCommandHandler _updateCommentCommandHandler;

        public UpdateCommentCommandHandlerUnitTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _updateCommentCommandHandler = new UpdateCommentCommandHandler(_commentRepositoryMock.Object);
        }

        [Fact]
        public async void Should_UpdateComment_Correctly()
        {
            //Arrange
            var commentDto = new CommentDto
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Text = "This is a test comment",
                CreatedAt = DateTime.Now,
            };

            var updatedCommentDto = new CommentDto
            {
                Id = commentDto.Id,
                UserId = commentDto.Id,
                PostId = commentDto.Id,
                Text = "This is an updated comment test comment",
                CreatedAt = DateTime.Now,
            };

            var comment = new Comment
            {
                Id = updatedCommentDto.Id,
                UserId = updatedCommentDto.UserId,
                PostId = updatedCommentDto.PostId,
                Text = updatedCommentDto.Text,
                CreatedAt = updatedCommentDto.CreatedAt
            };

            var updateCommentCommand = new UpdateCommentCommand(updatedCommentDto);

            _commentRepositoryMock
                .Setup(x => x.UpdateAsync(It.Is<Comment>(y => y.UserId == updatedCommentDto.UserId &&
                y.PostId == updatedCommentDto.PostId && y.Text == updatedCommentDto.Text &&
                y.CreatedAt == updatedCommentDto.CreatedAt))).ReturnsAsync(comment);

            //Act
            var result = await _updateCommentCommandHandler.Handle(updateCommentCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(comment.UserId, result.UserId);
            Assert.Equal(comment.PostId, result.PostId);
            Assert.Equal(comment.Text, result.Text);
            Assert.Equal(comment.CreatedAt, result.CreatedAt);
        }
    }
}
