using Moq;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.UnitTests.Mocks
{
    public class MockPostRepository : Mock<IPostRepository>
    {
        public MockPostRepository MockGetPost(Post post)
        {
            Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(post);

            return this;
        }
    }
}
