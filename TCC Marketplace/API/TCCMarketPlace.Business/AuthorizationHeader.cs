using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCMarketPlace.Business.Enum;

namespace TCCMarketPlace.Business
{
    public class AuthorizationHeader
    {
        public AuthorizationHeader(AuthorizationScheme authorizationScheme, string parameter, string contentType)
        {
            Scheme = authorizationScheme;
            Parameter = parameter;
            Content_Type = contentType;
        }

        public AuthorizationScheme Scheme { get; set; }

        internal string Parameter { get; set; }

        public string Content_Type { get; set; }

    }
}
