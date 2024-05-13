using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.Comments.CommandsHandler;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.Comments
{
    public class RemoveCommentCommandHandlerUnitTest
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<RemoveCommentCommandHandler>> _loggerMock;
        private readonly RemoveCommentCommandHandler _removeCommentCommandHandler;

        public RemoveCommentCommandHandlerUnitTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<RemoveCommentCommandHandler>>();

            _unitOfWorkMock.Setup(uow => uow.CommentRepository).Returns(_commentRepositoryMock.Object);
            
            _removeCommentCommandHandler = new RemoveCommentCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async void Should_RemoveComment_Correctly()
        {
            //Arrange
            var commentId = Guid.NewGuid();

            var comment = new Comment
            {
                Id = commentId,
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Text = "This is a test comment",
                CreatedAt = DateTime.UtcNow
            };

            var removeCommentCommand = new RemoveCommentCommand(commentId);

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(commentId))
                .ReturnsAsync(comment);

            //Act
            var result = await _removeCommentCommandHandler.Handle(removeCommentCommand, new CancellationToken());

            //Assert
            _commentRepositoryMock.Verify(x => x.GetByIdAsync(commentId), Times.Once);
            _commentRepositoryMock.Verify(x => x.RemoveAsync(comment), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
