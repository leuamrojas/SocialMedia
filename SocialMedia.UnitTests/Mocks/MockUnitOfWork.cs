using Moq;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.UnitTests.Mocks
{
    public class MockUnitOfWork : Mock<IUnitOfWork>
    {
        public MockUnitOfWork MockGetPostRepository(MockPostRepository postRepository)
        {
            SetupGet(s => s.PostRepository)
                .Returns(postRepository.Object);

            return this;
        }
    }
}
