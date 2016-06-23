using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Admin.Core.Exceptions
{
    public class EMailAlreadyExistsException : Exception
    {
        public EMailAlreadyExistsException(string message)
        : base(message)
        {
        }
    }
}
