using TCCMarketPlace.Business.Interface;

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
    }
}
