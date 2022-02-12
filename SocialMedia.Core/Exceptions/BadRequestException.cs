using System;

namespace SocialMedia.Core.Exceptions
{
    public abstract class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}
