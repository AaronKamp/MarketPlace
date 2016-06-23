using System;
using System.Collections.Generic;

namespace TCCMarketPlace.Model.ExceptionHandlers
{
    public class BusinessException : Exception
    {
        public ICollection<string> Errors { get; set; }
        public BusinessException(string message)
            : base(message)
        {
            Errors = new List<string>();
            Errors.Add(message);
        }

        public BusinessException(ICollection<string> errors)
            : base()
        {
            Errors = errors;
        }
    }
}
