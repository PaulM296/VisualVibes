using Moq;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.Comments.CommandsHandler;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.App.Reactions.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.Comments
{
    public class CreateCommentCommandHandlerUnitTest
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private CreateCommentCommandHandler _createCommentCommandHandler;

        public CreateCommentCommandHandlerUnitTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _createCommentCommandHandler = new CreateCommentCommandHandler(_commentRepositoryMock.Object);
        }

        [Fact]
        public async void Should_CreateComment_Correctly()
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

            var comment = new Comment
            {
                Id = commentDto.Id,
                UserId = commentDto.UserId,
                PostId = commentDto.PostId,
                Text = commentDto.Text,
                CreatedAt = commentDto.CreatedAt
            };

            var createCommentCommand = new CreateCommentCommand(commentDto);

            _commentRepositoryMock
                .Setup(x => x.AddAsync(It.Is<Comment>(y => y.UserId == commentDto.UserId &&
                y.PostId == commentDto.PostId && y.Text == commentDto.Text &&
                y.CreatedAt == commentDto.CreatedAt))).ReturnsAsync(comment);

            //Act
            var result = await _createCommentCommandHandler.Handle(createCommentCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(comment.UserId, result.UserId);
            Assert.Equal(comment.PostId, result.PostId);
            Assert.Equal(comment.Text, result.Text);
            Assert.Equal(comment.CreatedAt, result.CreatedAt);
        }
    }
}
