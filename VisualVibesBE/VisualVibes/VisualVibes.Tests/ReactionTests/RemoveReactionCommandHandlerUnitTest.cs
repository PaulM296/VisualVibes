using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.App.Reactions.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.ReactionTests
{
    public class RemoveReactionCommandHandlerUnitTest
    {
        private readonly Mock<IReactionRepository> _reactionRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<RemoveReactionCommandHandler>> _loggerMock;
        private readonly RemoveReactionCommandHandler _removeReactionCommandHandler;

        public RemoveReactionCommandHandlerUnitTest()
        {
            _reactionRepositoryMock = new Mock<IReactionRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<RemoveReactionCommandHandler>>();

            _unitOfWorkMock.Setup(uow => uow.ReactionRepository).Returns(_reactionRepositoryMock.Object);
            _removeReactionCommandHandler = new RemoveReactionCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async void Should_RemoveReaction_Correctly()
        {
            //Arrange
            var reactionId = Guid.NewGuid();

            var reaction = new Reaction
            {
                Id = reactionId,
                UserId = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                ReactionType = Domain.Enum.ReactionType.Love,
                Timestamp = DateTime.Now
            };

            var removeReactionCommand = new RemoveReactionCommand(reactionId);

            _reactionRepositoryMock
                .Setup(x => x.GetByIdAsync(reactionId)).ReturnsAsync(reaction);


            //Act
            var result = await _removeReactionCommandHandler.Handle(removeReactionCommand, new CancellationToken());

            //Assert
            _reactionRepositoryMock.Verify(x => x.GetByIdAsync(reactionId), Times.Once);
            _reactionRepositoryMock.Verify(x => x.RemoveAsync(reaction), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
