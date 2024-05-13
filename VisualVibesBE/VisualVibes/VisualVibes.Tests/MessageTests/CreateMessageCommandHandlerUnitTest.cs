using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs.MessageDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Commands;
using VisualVibes.App.Messages.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.MessageTests
{
    public class CreateMessageCommandHandlerUnitTest
    {
        private readonly Mock<IMessageRepository> _messageRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CreateMessageCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateMessageCommandHandler _createMessageCommandHandler;

        public CreateMessageCommandHandlerUnitTest()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateMessageCommandHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.MessageRepository).Returns(_messageRepositoryMock.Object);
            
            _createMessageCommandHandler = new CreateMessageCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_CreateMessage_Correctly()
        {
            //Arrange
            var messageDto = new CreateMessageDto
            {
                UserId = Guid.NewGuid(),
                ConversationId = Guid.NewGuid(),
                Content = "This is a message test",
                Timestamp = DateTime.UtcNow,
            };

            var message = new Message
            {
                UserId = messageDto.UserId,
                ConversationId = messageDto.ConversationId,
                Content = messageDto.Content,
                Timestamp = messageDto.Timestamp
            };

            var responseMessageDto = new ResponseMessageDto
            {
                Id = message.Id,
                UserId = message.UserId,
                ConversationId = message.ConversationId,
                Content = message.Content,
                Timestamp = message.Timestamp
            };

            var createMessageCommand = new CreateMessageCommand(messageDto);

            _messageRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Message>()))
                .ReturnsAsync(message);

            _mapperMock
                .Setup(m => m.Map<ResponseMessageDto>(It.IsAny<Message>()))
                .Returns(responseMessageDto);

            //Act
            var result = await _createMessageCommandHandler.Handle(createMessageCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(responseMessageDto.UserId, result.UserId);
            Assert.Equal(responseMessageDto.ConversationId, result.ConversationId);
            Assert.Equal(responseMessageDto.Content, result.Content);
            Assert.Equal(responseMessageDto.Timestamp, result.Timestamp);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<ResponseMessageDto>(It.IsAny<Message>()), Times.Once);
        }
    }
}
