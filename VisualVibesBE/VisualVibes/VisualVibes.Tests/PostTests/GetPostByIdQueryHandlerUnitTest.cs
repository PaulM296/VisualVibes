//using Moq;
//using VisualVibes.App.DTOs;
//using VisualVibes.App.Interfaces;
//using VisualVibes.App.Posts.Queries;
//using VisualVibes.App.Posts.QueriesHandler;
//using VisualVibes.Domain.Models.BaseEntity;

//namespace VisualVibes.Tests.PostTests
//{
//    public class GetPostByIdQueryHandlerUnitTest
//    {
//        private readonly Mock<IPostRepository> _postRepositoryMock;
//        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//        private GetPostByIdQueryHandler _getPostByIdQueryHandler;

//        public GetPostByIdQueryHandlerUnitTest()
//        {
//            _postRepositoryMock = new Mock<IPostRepository>();
//            _unitOfWorkMock = new Mock<IUnitOfWork>();

//            _unitOfWorkMock.Setup(uow => uow.PostRepository).Returns(_postRepositoryMock.Object);
//            _getPostByIdQueryHandler = new GetPostByIdQueryHandler(_unitOfWorkMock.Object);
//        }

//        [Fact]
//        public async void Should_GetPost_Correctly()
//        {
//            //Arrange
//            var postDto = new PostDto
//            {
//                Id = Guid.NewGuid(),
//                UserId = Guid.NewGuid(),
//                Pictures = "Picture1.png",
//                Caption = "This is a test caption",
//                CreatedAt = DateTime.UtcNow
//            };

//            var post = new Post
//            {
//                Id = postDto.Id,
//                UserId = postDto.UserId,
//                Pictures = postDto.Pictures,
//                Caption = postDto.Caption,
//                CreatedAt = postDto.CreatedAt
//            };

//            var getPostByIdQuery = new GetPostByIdQuery(postDto.Id);
            
//            _postRepositoryMock
//                .Setup(x => x.GetByIdAsync(postDto.Id)).ReturnsAsync(post);

//            //Act
//            var result = await _getPostByIdQueryHandler.Handle(getPostByIdQuery, new CancellationToken());

//            //Assert
//            Assert.NotNull(result);
//            Assert.Equal(post.Id, result.Id);
//            Assert.Equal(post.UserId, result.UserId);
//            Assert.Equal(post.Pictures, result.Pictures);
//            Assert.Equal(post.Caption, result.Caption);
//            Assert.Equal(post.CreatedAt, result.CreatedAt);
//            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Never);
//        }
//    }
//}
