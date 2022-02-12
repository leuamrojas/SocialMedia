using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Services;
using SocialMedia.UnitTests.Mocks;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SocialMedia.UnitTests
{
    public class PostServiceTests
    {
        [Fact]
        public async Task PostService_GetPost_ValidPost()
        {
            var postId = 5;
            var mockPost = GetMockPost(postId);
            var mockPostRepo = new MockPostRepository().MockGetPost(mockPost);
            var mockUnitOfWork = new MockUnitOfWork().MockGetPostRepository(mockPostRepo);

            IOptions<PaginationOptions> options = Options.Create<PaginationOptions>(new PaginationOptions());
            var postService = new PostService(mockUnitOfWork.Object, options);

            var result = await postService.GetPost(5);
            Assert.Equal(5, result.Id);
        }

        [Fact]
        public async Task PostService_GetPost_InvalidNoPost()
        {
            var postId = 0;
            var mockPostRepo = new MockPostRepository().MockGetPost(null);
            var mockUnitOfWork = new MockUnitOfWork().MockGetPostRepository(mockPostRepo);

            IOptions<PaginationOptions> options = Options.Create<PaginationOptions>(new PaginationOptions());
            var postService = new PostService(mockUnitOfWork.Object, options);

            Func<Task> action = () => postService.GetPost(postId);
            var exception = await Assert.ThrowsAsync<PostNotFoundException>(action);
            Assert.Equal($"The post with Id {postId} was not found.", exception.Message);
        }

        private Post GetMockPost(int postId)
        {
            return new Post()
            {
                Id = postId,
                UserId = 1,
                Date = DateTime.Now,
                Description = "Test description",
                Image = "Imaqge url",
                User = GetMockUser(1),
            };
        }

        private User GetMockUser(int userId)
        {
            return new User()
            {
                Id = userId,
                DateOfBirth = DateTime.Now,
                Email = "test@domain.com",
                FirstName = "Manuel",
                LastName = "Rojas",
                IsActive = true,
                Telephone = "555-5555555"
            };
        }
    }
}
