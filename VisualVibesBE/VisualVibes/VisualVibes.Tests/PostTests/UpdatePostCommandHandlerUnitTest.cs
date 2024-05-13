using AutoMapper;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Posts.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.PostTests
{
    public class UpdatePostCommandHandlerUnitTest
    {
        private readonly UpdatePostCommandHandler _updatePostCommandHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<UpdatePostCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public UpdatePostCommandHandlerUnitTest()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UpdatePostCommandHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.PostRepository).Returns(_postRepositoryMock.Object);
            _updatePostCommandHandler = new UpdatePostCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_UpdatePost_Correctly()
        {
            //Arrange
            var postId = Guid.NewGuid();
            var updatePostDto = new UpdatePostDto
            {
                UserId = Guid.NewGuid(),
                Pictures = "Picture2.png",
                Caption = "Updated test caption"
            };

            var post = new Post
            {
                Id = postId,
                UserId = updatePostDto.UserId,
                Pictures = "Picture1.png",
                Caption = "This is a test caption",
                CreatedAt = DateTime.UtcNow
            };

            var updatedPost = new Post
            {
                Id = postId,
                UserId = updatePostDto.UserId,
                Pictures = updatePostDto.Pictures,
                Caption = updatePostDto.Caption,
                CreatedAt = post.CreatedAt
            };

            var responsePostDto = new ResponsePostDto
            {
                Id = updatedPost.Id,
                UserId = updatedPost.UserId,
                Pictures = updatedPost.Pictures,
                Caption = updatedPost.Caption,
                CreatedAt = updatedPost.CreatedAt
            };

            var updatePostCommand = new UpdatePostCommand(postId, updatePostDto);

            _postRepositoryMock
                .Setup(x => x.GetByIdAsync(postId))
                .ReturnsAsync(post);

            _postRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Post>()))
                .ReturnsAsync(updatedPost);

            _mapperMock
                .Setup(m => m.Map<ResponsePostDto>(It.IsAny<Post>()))
                .Returns(responsePostDto);

            //Act
            var result = await _updatePostCommandHandler.Handle(updatePostCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(updatedPost.UserId, result.UserId);
            Assert.Equal(updatedPost.Pictures, result.Pictures);
            Assert.Equal(updatedPost.Caption, result.Caption);
            _postRepositoryMock.Verify(x => x.GetByIdAsync(postId), Times.Once);
            _postRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Post>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<ResponsePostDto>(updatedPost), Times.Once);
        }
    }
}
