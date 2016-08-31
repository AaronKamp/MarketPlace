using System.Collections.Generic;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    public interface ILocationManager
    {

        /// <summary>
        /// Gets the list of countries.
        /// </summary>
        IList<Country> GetCountries();

        /// <summary>
        /// Gets the list of states by countryId.
        /// </summary>
        IList<State> GetStates(int countryId);

        /// <summary>
        /// Gets the list of SCFs by stateId.
        /// </summary>
        IList<SCF> GetScFsOfState(int stateId);

        /// <summary>
        /// Gets all SCFs by countryId.
        /// </summary>
        IList<SCF> GetAllScFs(int countryId);

        /// <summary>
        /// Gets SCF by scfId
        /// </summary>
        SCF GetScf(int scfId);
    }
}
