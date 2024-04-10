using Moq;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.Comments.CommandsHandler;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.Comments
{
    public class RemoveCommentCommandHandlerUnitTest
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private RemoveCommentCommandHandler _removeCommentCommandHandler;

        public RemoveCommentCommandHandlerUnitTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _removeCommentCommandHandler = new RemoveCommentCommandHandler(_commentRepositoryMock.Object);
        }

        [Fact]
        public async void Should_RemoveComment_Correctly()
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

            var removeCommentCommand = new RemoveCommentCommand(commentDto.Id);

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(commentDto.Id)).ReturnsAsync(comment);

            //Act
            var result = await _removeCommentCommandHandler.Handle(removeCommentCommand, new CancellationToken());

            //Assert
            _commentRepositoryMock.Verify(x => x.GetByIdAsync(commentDto.Id), Times.Once);
            _commentRepositoryMock.Verify(x => x.RemoveAsync(comment), Times.Once);
        }
    }
}
