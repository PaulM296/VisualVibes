using Moq;
using VisualVibes.App.DTOs;
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
        private RemoveMessageCommandHandler _removeMessageCommandHandler;

        public RemoveMessageCommandHandlerUnitTest()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.MessageRepository).Returns(_messageRepositoryMock.Object);
            _removeMessageCommandHandler = new RemoveMessageCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async void Should_RemoveMessage_Correctly()
        {
            // Arrange
            var messageDto = new MessageDto
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ConversationId = Guid.NewGuid(),
                Content = "This is a message test",
                Timestamp = DateTime.UtcNow,
            };

            var message = new Message
            {
                Id = messageDto.Id,
                UserId = messageDto.UserId,
                ConversationId = messageDto.ConversationId,
                Content = messageDto.Content,
                Timestamp = messageDto.Timestamp
            };

            var removeMessageCommand = new RemoveMessageCommand(messageDto.Id);

            _messageRepositoryMock
                .Setup(x => x.GetByIdAsync(messageDto.Id)).ReturnsAsync(message);

            //Act
            var result = await _removeMessageCommandHandler.Handle(removeMessageCommand, new CancellationToken());

            //Assert
            _messageRepositoryMock.Verify(x => x.GetByIdAsync(messageDto.Id), Times.Once);
            _messageRepositoryMock.Verify(x => x.RemoveAsync(message), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
