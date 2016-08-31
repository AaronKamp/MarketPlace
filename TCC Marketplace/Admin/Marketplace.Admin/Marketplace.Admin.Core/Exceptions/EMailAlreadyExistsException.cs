using System;

namespace Marketplace.Admin.Core.Exceptions
{
    /// <summary>
    /// Email already exists custom exception.
    /// </summary>
    public class EMailAlreadyExistsException : Exception
    {
        public EMailAlreadyExistsException(string message)
        : base(message)
        {
        }
    }
}
