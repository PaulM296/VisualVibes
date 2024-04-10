using Moq;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Commands;
using VisualVibes.App.Messages.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.MessageTests
{
    public class CreateMessageCommandHandlerUnitTest
    {
        private readonly Mock<IMessageRepository> _messageRepositoryMock;
        private CreateMessageCommandHandler _createMessageCommandHandler;

        public CreateMessageCommandHandlerUnitTest()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();
            _createMessageCommandHandler = new CreateMessageCommandHandler(_messageRepositoryMock.Object);
        }

        [Fact]
        public async void Should_CreateMessage_Correctly()
        {
            //Arrange
            var messageDto = new MessageDto
            {
                Id = Guid.NewGuid(),
                SenderId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                ConversationId = Guid.NewGuid(),
                Content = "This is a message test",
                Timestamp = DateTime.UtcNow,
            };

            var message = new Message
            {
                Id = messageDto.Id,
                SenderId = messageDto.SenderId,
                ReceiverId = messageDto.ReceiverId,
                ConversationId = messageDto.ConversationId,
                Content = messageDto.Content,
                Timestamp = messageDto.Timestamp
            };

            var createMessageCommand = new CreateMessageCommand(messageDto);

            _messageRepositoryMock
                .Setup(x => x.AddAsync(It.Is<Message>(y => y.SenderId == messageDto.SenderId &&
                y.ReceiverId == messageDto.ReceiverId && y.ConversationId == messageDto.ConversationId && 
                y.Content == messageDto.Content && y.Timestamp == messageDto.Timestamp))).ReturnsAsync(message);

            //Act
            var result = await _createMessageCommandHandler.Handle(createMessageCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(message.SenderId, result.SenderId);
            Assert.Equal(message.ReceiverId, result.ReceiverId);
            Assert.Equal(message.ConversationId, result.ConversationId);
            Assert.Equal(message.Content, result.Content);
            Assert.Equal(message.Timestamp, result.Timestamp);
        }
    }
}
