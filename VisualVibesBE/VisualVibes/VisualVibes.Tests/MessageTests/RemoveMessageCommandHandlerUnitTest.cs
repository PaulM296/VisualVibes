using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Commands;
using VisualVibes.App.Messages.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.MessageTests
{
    public class RemoveMessageCommandHandlerUnitTest
    {
        private readonly Mock<IMessageRepository> _messageRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<RemoveMessageCommandHandler>> _loggerMock;

        private RemoveMessageCommandHandler _removeMessageCommandHandler;

        public RemoveMessageCommandHandlerUnitTest()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<RemoveMessageCommandHandler>>();

            _unitOfWorkMock.Setup(uow => uow.MessageRepository).Returns(_messageRepositoryMock.Object);
            
            _removeMessageCommandHandler = new RemoveMessageCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async void Should_RemoveMessage_Correctly()
        {
            // Arrange
            var messageId = Guid.NewGuid();

            var message = new Message
            {
                Id = messageId,
                UserId = Guid.NewGuid(),
                ConversationId = Guid.NewGuid(),
                Content = "This is a message test",
                Timestamp = DateTime.UtcNow,
            };

            var removeMessageCommand = new RemoveMessageCommand(messageId);

            _messageRepositoryMock
                .Setup(x => x.GetByIdAsync(messageId))
                .ReturnsAsync(message);

            //Act
            var result = await _removeMessageCommandHandler.Handle(removeMessageCommand, new CancellationToken());

            //Assert
            _messageRepositoryMock.Verify(x => x.GetByIdAsync(messageId), Times.Once);
            _messageRepositoryMock.Verify(x => x.RemoveAsync(message), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
