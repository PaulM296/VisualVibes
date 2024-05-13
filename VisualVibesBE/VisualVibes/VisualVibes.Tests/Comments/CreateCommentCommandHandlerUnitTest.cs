using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.Comments.CommandsHandler;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.Comments
{
    public class CreateCommentCommandHandlerUnitTest
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CreateCommentCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private CreateCommentCommandHandler _createCommentCommandHandler;

        public CreateCommentCommandHandlerUnitTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateCommentCommandHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.CommentRepository).Returns(_commentRepositoryMock.Object);
            
            _createCommentCommandHandler = new CreateCommentCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_CreateComment_Correctly()
        {
            //Arrange
            var commentDto = new CreateCommentDto
            {
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Text = "This is a test comment",
            };

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                UserId = commentDto.UserId,
                PostId = commentDto.PostId,
                Text = commentDto.Text,
                CreatedAt = DateTime.UtcNow,
            };

            var responseCommentDto = new ResponseCommentDto
            {
                Id = comment.Id,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt
            };

            var createCommentCommand = new CreateCommentCommand(commentDto);

            _commentRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Comment>()))
                .ReturnsAsync(comment);

            _mapperMock
                .Setup(m => m.Map<ResponseCommentDto>(It.IsAny<Comment>()))
                .Returns(responseCommentDto);

            //Act
            var result = await _createCommentCommandHandler.Handle(createCommentCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(comment.Id, result.Id);
            Assert.Equal(comment.UserId, result.UserId);
            Assert.Equal(comment.PostId, result.PostId);
            Assert.Equal(comment.Text, result.Text);
            Assert.Equal(comment.CreatedAt, result.CreatedAt);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            _commentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Comment>()), Times.Once);
            _mapperMock.Verify(m => m.Map<ResponseCommentDto>(comment), Times.Once);
        }
    }
}
