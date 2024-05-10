//using Moq;
//using VisualVibes.App.Conversations.Commands;
//using VisualVibes.App.Conversations.CommandsHandler;
//using VisualVibes.App.DTOs;
//using VisualVibes.App.Interfaces;
//using VisualVibes.App.Reactions.Commands;
//using VisualVibes.App.Reactions.CommandsHandler;
//using VisualVibes.Domain.Models.BaseEntity;

//namespace VisualVibes.Tests.Reactions
//{
//    public class RemoveReactionCommandHandlerUnitTest
//    {
//        private readonly Mock<IReactionRepository> _reactionRepositoryMock;
//        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//        private RemoveReactionCommandHandler _removeReactionCommandHandler;

//        public RemoveReactionCommandHandlerUnitTest()
//        {
//            _reactionRepositoryMock = new Mock<IReactionRepository>();
//            _unitOfWorkMock = new Mock<IUnitOfWork>();

//            _unitOfWorkMock.Setup(uow => uow.ReactionRepository).Returns(_reactionRepositoryMock.Object);
//            _removeReactionCommandHandler = new RemoveReactionCommandHandler(_unitOfWorkMock.Object);
//        }

//        [Fact]
//        public async void Should_RemoveReaction_Correctly()
//        {
//            //Arrange
//            var reactionDto = new ReactionDto
//            {
//                Id = Guid.NewGuid(),
//                UserId = Guid.NewGuid(),
//                PostId = Guid.NewGuid(),
//                ReactionType = Domain.Enum.ReactionType.Love,
//                Timestamp = DateTime.Now,
//            };

//            var reaction = new Reaction
//            {
//                Id = reactionDto.Id,
//                UserId = reactionDto.UserId,
//                PostId = reactionDto.PostId,
//                ReactionType = reactionDto.ReactionType,
//                Timestamp = reactionDto.Timestamp
//            };

//            var removeReactionCommand = new RemoveReactionCommand(reactionDto.Id);

//            _reactionRepositoryMock
//                .Setup(x => x.GetByIdAsync(reactionDto.Id)).ReturnsAsync(reaction);

//            //Act
//            var result = await _removeReactionCommandHandler.Handle(removeReactionCommand, new CancellationToken());

//            //Assert
//            _reactionRepositoryMock.Verify(x => x.GetByIdAsync(reactionDto.Id), Times.Once);
//            _reactionRepositoryMock.Verify(x => x.RemoveAsync(reaction), Times.Once);
//            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
//        }
//    }
//}
