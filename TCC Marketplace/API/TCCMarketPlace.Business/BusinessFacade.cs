using TCCMarketPlace.Business.Interface;
using TCCMarketPlace.Model;

namespace TCCMarketPlace.Business
{
    /// <summary>
    /// Façade class to handle the business objects
    /// </summary>
    public class BusinessFacade
    {
        /// <summary>
        /// Returns marketplace instance
        /// </summary>
        /// <returns></returns>
        public static IMarketPlace GetMarketPlaceInstance()
        {
            return new MarketPlace();
        }

        /// <summary>
        /// Returns authentication provider instance
        /// </summary>
        /// <returns></returns>
        public static IAuthentication GetAuthenticationInstance()
        {
            return new TccAuthentication();
        }

        /// <summary>
        /// Returns third party service provider instance
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IThirdPartyService GetServiceProviderInstance(ServiceProvider serviceProvider)
        {
            return new ThirdPartyService(serviceProvider);
        }
       
    }
}
