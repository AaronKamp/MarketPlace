using System.Collections.Generic;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    public interface ILocationManager
    {
        IList<Country> GetCountries();
        IList<State> GetStates(int countryId);
        IList<SCF> GetScFsOfState(int stateId);
        IList<SCF> GetAllScFs(int countryId);
        SCF GetScf(int scfId);
    }
}
