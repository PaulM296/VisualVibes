using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection.Metadata;
using VisualVibes.App.DTOs.MessageDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Queries;
using VisualVibes.App.Messages.QueriesHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.MessageTests
{
    public class GetAllConversationMessagesQueryHandlerUnitTest
    {
        private readonly Mock<IMessageRepository> _messageRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetAllConversationMessagesQueryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllConversationMessagesQueryHandler _getAllConversationMessagesQueryHandler;

        public GetAllConversationMessagesQueryHandlerUnitTest()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetAllConversationMessagesQueryHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.MessageRepository).Returns(_messageRepositoryMock.Object);

            _getAllConversationMessagesQueryHandler = new GetAllConversationMessagesQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_GetAllConversationMessages_Correctly()
        {
            //Arrange
            var conversationId = Guid.NewGuid();

            var messages = new List<Message>
            {
                new Message 
                { 
                    ConversationId = conversationId, 
                    Content = "Hello World!", 
                    Timestamp = DateTime.UtcNow 
                },

                new Message 
                { 
                    ConversationId = conversationId, 
                    Content = "Second message", 
                    Timestamp = DateTime.UtcNow }
            };
            var responseDtos = new List<ResponseMessageDto>
            {
                new ResponseMessageDto 
                { 
                    Id = messages[0].Id,
                    Content = messages[0].Content, 
                    Timestamp = messages[0].Timestamp 
                },

                new ResponseMessageDto 
                { 
                    Id = messages[1].Id, 
                    Content = messages[1].Content, 
                    Timestamp = messages[1].Timestamp 
                }
            };

            var query = new GetAllConversationMessagesQuery(conversationId);

            _messageRepositoryMock
                .Setup(m => m.GetAllAsync(conversationId))
                .ReturnsAsync(messages);

            _mapperMock
                .Setup(m => m.Map<ICollection<ResponseMessageDto>>(messages))
                .Returns(responseDtos);

            //Act
            var result = await _getAllConversationMessagesQueryHandler.Handle(query, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mapperMock.Verify(m => m.Map<ICollection<ResponseMessageDto>>(messages), Times.Once);  
        }

        [Fact]
        public async Task Should_ThrowWhenNoMessages_Found()
        {
            // Arrange
            var conversationId = Guid.NewGuid();
            var query = new GetAllConversationMessagesQuery(conversationId);

            _messageRepositoryMock.Setup(m => m.GetAllAsync(conversationId)).ReturnsAsync(new List<Message>());

            // Act & Assert
            await Assert.ThrowsAsync<MessageNotFoundException>(() => _getAllConversationMessagesQueryHandler.Handle(query, new CancellationToken()));
        }
    }
}
