using Moq;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.App.Reactions.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.Reactions
{
    public class CreateReactionCommandHandlerUnitTest
    {
        private readonly Mock<IReactionRepository> _reactionRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private CreateReactionCommandHandler _createReactionCommandHandler;


        public CreateReactionCommandHandlerUnitTest()
        {
            _reactionRepositoryMock = new Mock<IReactionRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.ReactionRepository).Returns(_reactionRepositoryMock.Object);
            _createReactionCommandHandler = new CreateReactionCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async void Should_CreateReaction_Correctly()
        {
            //Arrange
            var reactionDto = new ReactionDto
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                ReactionType = Domain.Enum.ReactionType.Love,
                Timestamp = DateTime.Now,
            };

            var reaction = new Reaction
            {
                Id = reactionDto.Id,
                UserId = reactionDto.UserId,
                PostId = reactionDto.PostId,
                ReactionType = reactionDto.ReactionType,
                Timestamp = reactionDto.Timestamp
            };

            var createReactionCommand = new CreateReactionCommand(reactionDto);

            _reactionRepositoryMock
                .Setup(x => x.AddAsync(It.Is<Reaction>(y => y.UserId == reactionDto.UserId && 
                y.PostId == reactionDto.PostId && y.ReactionType == reactionDto.ReactionType && 
                y.Timestamp == reactionDto.Timestamp))).ReturnsAsync(reaction);

            //Act
            var result = await _createReactionCommandHandler.Handle(createReactionCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(reaction.UserId, result.UserId);
            Assert.Equal(reaction.PostId, result.PostId);
            Assert.Equal(reaction.ReactionType, result.ReactionType);
            Assert.Equal(reaction.Timestamp, result.Timestamp);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
