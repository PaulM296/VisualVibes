using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs.ReactionDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.App.Reactions.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.ReactionTests
{
    public class UpdateReactionCommandHandlerUnitTest
    {
        private readonly UpdateReactionCommandHandler _updateReactionCommandHandler;
        private readonly Mock<IReactionRepository> _reactionRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<UpdateReactionCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public UpdateReactionCommandHandlerUnitTest()
        {
            _reactionRepositoryMock = new Mock<IReactionRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UpdateReactionCommandHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.ReactionRepository).Returns(_reactionRepositoryMock.Object);
            _updateReactionCommandHandler = new UpdateReactionCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Should_UpdateReaction_Correctly()
        {
            // Arrange
            var reactionId = Guid.NewGuid();

            var updateReactionDto = new UpdateReactionDto 
            { 
                ReactionType = Domain.Enum.ReactionType.Like 
            };

            var existingReaction = new Reaction 
            { 
                Id = reactionId, 
                ReactionType = Domain.Enum.ReactionType.Love 
            };

            var updatedReaction = new Reaction 
            { 
                Id = reactionId, 
                ReactionType = updateReactionDto.ReactionType 
            };

            var responseDto = new ResponseReactionDto { Id = reactionId, ReactionType = updateReactionDto.ReactionType };

            _reactionRepositoryMock
                .Setup(x => x.GetByIdAsync(reactionId)).ReturnsAsync(existingReaction);

            _reactionRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Reaction>())).ReturnsAsync(updatedReaction);

            _mapperMock
                .Setup(m => m.Map<ResponseReactionDto>(It.IsAny<Reaction>())).Returns(responseDto);

            var command = new UpdateReactionCommand(reactionId, updateReactionDto);

            // Act
            var result = await _updateReactionCommandHandler.Handle(command, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto.ReactionType, result.ReactionType);
            _reactionRepositoryMock.Verify(x => x.GetByIdAsync(reactionId), Times.Once);
            _reactionRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Reaction>(r => r.ReactionType == updateReactionDto.ReactionType)), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
