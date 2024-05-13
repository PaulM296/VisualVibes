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
    public class CreateReactionCommandHandlerUnitTest
    {
        private readonly Mock<IReactionRepository> _reactionRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CreateReactionCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateReactionCommandHandler _createReactionCommandHandler;


        public CreateReactionCommandHandlerUnitTest()
        {
            _reactionRepositoryMock = new Mock<IReactionRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateReactionCommandHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.ReactionRepository).Returns(_reactionRepositoryMock.Object);

            _createReactionCommandHandler = new CreateReactionCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_CreateReaction_Correctly()
        {
            //Arrange
            var createReactionDto = new CreateReactionDto
            {
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                ReactionType = Domain.Enum.ReactionType.Love,
                Timestamp = DateTime.UtcNow,
            };

            var reaction = new Reaction
            {
                UserId = createReactionDto.UserId,
                PostId = createReactionDto.PostId,
                ReactionType = createReactionDto.ReactionType,
                Timestamp = createReactionDto.Timestamp
            };

            var responseDto = new ResponseReactionDto
            {
                UserId = reaction.UserId,
                PostId = reaction.PostId,
                ReactionType = reaction.ReactionType,
                Timestamp = reaction.Timestamp
            };

            var createReactionCommand = new CreateReactionCommand(createReactionDto);

            _reactionRepositoryMock
                .Setup(x => x.AddAsync(It.Is<Reaction>(y => y.UserId == createReactionDto.UserId &&
                y.PostId == createReactionDto.PostId && y.ReactionType == createReactionDto.ReactionType &&
                y.Timestamp == createReactionDto.Timestamp))).ReturnsAsync(reaction);

            _mapperMock
                .Setup(m => m.Map<ResponseReactionDto>(It.IsAny<Reaction>()))
                .Returns(responseDto);

            //Act
            var result = await _createReactionCommandHandler.Handle(createReactionCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto.UserId, result.UserId);
            Assert.Equal(responseDto.PostId, result.PostId);
            Assert.Equal(responseDto.ReactionType, result.ReactionType);
            Assert.Equal(responseDto.Timestamp, result.Timestamp);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            _reactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Reaction>()), Times.Once);
            _mapperMock.Verify(m => m.Map<ResponseReactionDto>(It.IsAny<Reaction>()), Times.Once);
        }
    }
}
