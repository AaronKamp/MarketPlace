using TCCMarketPlace.Business.Enum;

namespace TCCMarketPlace.Business
{
    /// <summary>
    /// AuthorizationHeader Model.
    /// </summary>
    public class AuthorizationHeader
    {
        /// <summary>
        /// Parameterized constructor to initialize Authorization header.
        /// </summary>
        /// <param name="authorizationScheme"></param>
        /// <param name="parameter"></param>
        /// <param name="contentType"></param>
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
