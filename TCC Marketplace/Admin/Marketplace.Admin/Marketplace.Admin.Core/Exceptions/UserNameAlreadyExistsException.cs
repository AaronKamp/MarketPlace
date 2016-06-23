using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Admin.Core.Exceptions
{
    public class UserNameAlreadyExistsException : Exception
    {
        public UserNameAlreadyExistsException(string message)
        : base(message)
        {
        }
    }
}
