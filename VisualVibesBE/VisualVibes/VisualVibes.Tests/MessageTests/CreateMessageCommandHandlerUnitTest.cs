//using Moq;
//using VisualVibes.App.DTOs;
//using VisualVibes.App.Interfaces;
//using VisualVibes.App.Messages.Commands;
//using VisualVibes.App.Messages.CommandsHandler;
//using VisualVibes.Domain.Models.BaseEntity;

//namespace VisualVibes.Tests.MessageTests
//{
//    public class CreateMessageCommandHandlerUnitTest
//    {
//        private readonly Mock<IMessageRepository> _messageRepositoryMock;
//        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//        private CreateMessageCommandHandler _createMessageCommandHandler;

//        public CreateMessageCommandHandlerUnitTest()
//        {
//            _messageRepositoryMock = new Mock<IMessageRepository>();
//            _unitOfWorkMock = new Mock<IUnitOfWork>();

//            _unitOfWorkMock.Setup(uow => uow.MessageRepository).Returns(_messageRepositoryMock.Object);
//            _createMessageCommandHandler = new CreateMessageCommandHandler(_unitOfWorkMock.Object);
//        }

//        [Fact]
//        public async void Should_CreateMessage_Correctly()
//        {
//            //Arrange
//            var messageDto = new MessageDto
//            {
//                Id = Guid.NewGuid(),
//                UserId = Guid.NewGuid(),
//                ConversationId = Guid.NewGuid(),
//                Content = "This is a message test",
//                Timestamp = DateTime.UtcNow,
//            };

//            var message = new Message
//            {
//                Id = messageDto.Id,
//                UserId = messageDto.UserId,
//                ConversationId = messageDto.ConversationId,
//                Content = messageDto.Content,
//                Timestamp = messageDto.Timestamp
//            };

//            var createMessageCommand = new CreateMessageCommand(messageDto);

//            _messageRepositoryMock
//                .Setup(x => x.AddAsync(It.Is<Message>(y => y.UserId == messageDto.UserId &&
//                y.ConversationId == messageDto.ConversationId && 
//                y.Content == messageDto.Content && y.Timestamp == messageDto.Timestamp))).ReturnsAsync(message);

//            //Act
//            var result = await _createMessageCommandHandler.Handle(createMessageCommand, new CancellationToken());

//            //Assert
//            Assert.NotNull(result);
//            Assert.Equal(message.UserId, result.UserId);
//            Assert.Equal(message.ConversationId, result.ConversationId);
//            Assert.Equal(message.Content, result.Content);
//            Assert.Equal(message.Timestamp, result.Timestamp);
//            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
//        }
//    }
//}
