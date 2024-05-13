using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.Comments.CommandsHandler;
using VisualVibes.App.DTOs;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.Comments
{
    public class UpdateCommentCommandHandlerUnitTest
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<UpdateCommentCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateCommentCommandHandler _updateCommentCommandHandler;

        public UpdateCommentCommandHandlerUnitTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UpdateCommentCommandHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.CommentRepository).Returns(_commentRepositoryMock.Object);
            
            _updateCommentCommandHandler = new UpdateCommentCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_UpdateComment_Correctly()
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

            var updateCommentDto = new UpdateCommentDto
            {
                Text = "This is an updated comment test comment",
            };

            var updatedCommentDto = new ResponseCommentDto
            {
                Id = commentId,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Text = updateCommentDto.Text,
                CreatedAt = comment.CreatedAt
            };

            var updateCommentCommand = new UpdateCommentCommand(commentId, updateCommentDto);

            _commentRepositoryMock
                .Setup(x => x.GetByIdAsync(commentId))
                .ReturnsAsync(comment);

            _commentRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment);

            _mapperMock
                .Setup(m => m.Map<ResponseCommentDto>(It.IsAny<Comment>()))
                .Returns(updatedCommentDto);

            //Act
            var result = await _updateCommentCommandHandler.Handle(updateCommentCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(comment.UserId, result.UserId);
            Assert.Equal(comment.PostId, result.PostId);
            Assert.Equal(comment.Text, result.Text);
            Assert.Equal(comment.CreatedAt, result.CreatedAt);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<ResponseCommentDto>(comment), Times.Once);
        }
    }
}
