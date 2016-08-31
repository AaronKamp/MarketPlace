using System;
using System.Collections.Generic;

namespace TCCMarketPlace.Model.ExceptionHandlers
{

    /// <summary>
    /// Application specific business exception model.
    /// </summary>
    public class BusinessException : Exception
    {
        //Errors collection 
        public ICollection<string> Errors { get; private set; }

        /// <summary>
        ///  constructor to add an error to business exception
        /// </summary>
        /// <param name="message"></param>
        public BusinessException(string message)
            : base(message)
        {
            Errors = new List<string>();
            Errors.Add(message);
        }

        /// <summary>
        /// constructor to add error list to business exception
        /// </summary>
        /// <param name="errors"></param>
        public BusinessException(ICollection<string> errors)
            : base()
        {
            Errors = errors;
        }
    }
}
