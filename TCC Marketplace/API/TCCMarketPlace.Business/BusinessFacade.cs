using TCCMarketPlace.Business.Interface;
using TCCMarketPlace.Model;

namespace TCCMarketPlace.Business
{
    public class BusinessFacade
    {
        public static IMarketPlace GetMarketPlaceInstance()
        {
            return new MarketPlace();
        }

        public static IAuthentication GetAuthenticationInstance()
        {
            return new TccAuthentication();
        }

        public static IThirdPartyService GetServiceProviderInstance(ServiceProvider serviceProvider)
        {
            return new ThirdPartyService(serviceProvider);
        }
       
    }
}
