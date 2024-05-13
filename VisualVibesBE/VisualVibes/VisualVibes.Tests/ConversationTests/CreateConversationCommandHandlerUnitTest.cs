using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.Conversations.CommandsHandler;
using VisualVibes.App.DTOs.ConversationDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.ConversationTests
{
    public class CreateConversationCommandHandlerUnitTest
    {
        private readonly Mock<IConversationRepository> _conversationRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CreateConversationCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateConversationCommandHandler _createConversationCommandHandler;

        public CreateConversationCommandHandlerUnitTest()
        {
            _conversationRepositoryMock = new Mock<IConversationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateConversationCommandHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.ConversationRepository).Returns(_conversationRepositoryMock.Object);
            
            _createConversationCommandHandler = new CreateConversationCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_CreateConverstion_Correclty()
        {
            //Arrange
            var conversationDto = new CreateConversationDto
            {
                FirstParticipantId = Guid.NewGuid(),
                SecondParticipantId = Guid.NewGuid(),
            };

            var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                FirstParticipantId = conversationDto.FirstParticipantId,
                SecondParticipantId = conversationDto.SecondParticipantId,
            };

            var responseDto = new ResponseConversationDto
            {
                Id = conversation.Id,
                FirstParticipantId = conversation.FirstParticipantId,
                SecondParticipantId = conversation.SecondParticipantId,
            };

            var createConversationCommand = new CreateConversationCommand(conversationDto);

            _conversationRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Conversation>()))
                .ReturnsAsync(conversation);

            _mapperMock
                .Setup(m => m.Map<ResponseConversationDto>(It.IsAny<Conversation>()))
                .Returns(responseDto);

            //Act
            var result = await _createConversationCommandHandler.Handle(createConversationCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(conversation.Id, result.Id);
            Assert.Equal(conversation.FirstParticipantId, result.FirstParticipantId);
            Assert.Equal(conversation.SecondParticipantId, result.SecondParticipantId);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            _conversationRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Conversation>()), Times.Once);
            _mapperMock.Verify(m => m.Map<ResponseConversationDto>(It.IsAny<Conversation>()), Times.Once);
        }
    }
}
