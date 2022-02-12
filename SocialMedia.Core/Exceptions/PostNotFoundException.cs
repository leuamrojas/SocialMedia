using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.Exceptions
{
    public sealed class PostNotFoundException : NotFoundException
    {
        public PostNotFoundException(int id)
            : base($"The post with Id {id} was not found.")
        {
        }
    }
}
