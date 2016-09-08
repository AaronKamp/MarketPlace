using System;
using System.Runtime.Serialization;

namespace TCCMarketPlace.Cache
{
    /// <summary>
    /// Exception specific to Redis issues.
    /// </summary>
    [Serializable]
    public class RedisCacheException : Exception
    {
        /// <summary>
        /// The actual exception object
        /// </summary>
        public Exception RedisException { get; private set; }
        
        /// <summary>
        /// Custom exception message
        /// </summary>
        public string ExceptionMessage { get; private set; }

        /// <summary>
        /// Constructs RedisCacheException object from Exception object
        /// </summary>
        /// <param name="innerException"></param>
        public RedisCacheException(Exception innerException)
        {
            this.RedisException = innerException;
        }
        /// <summary>
        /// Constructs RedisCacheException object from Exception object and custom error message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public RedisCacheException(string message, Exception innerException) : base(message, innerException)
        {
            this.RedisException = innerException;
            this.ExceptionMessage = message;
        }

       
    }
}