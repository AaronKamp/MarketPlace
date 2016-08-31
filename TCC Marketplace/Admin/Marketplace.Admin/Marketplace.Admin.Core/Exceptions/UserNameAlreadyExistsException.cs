using System;

namespace Marketplace.Admin.Core.Exceptions
{
    /// <summary>
    /// User name already exist exception.
    /// </summary>
    public class UserNameAlreadyExistsException : Exception
    {
        public UserNameAlreadyExistsException(string message)
        : base(message)
        {
        }
    }
}
