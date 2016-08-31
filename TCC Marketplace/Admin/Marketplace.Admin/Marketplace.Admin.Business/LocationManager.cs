using System.Collections.Generic;
using System.Linq;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    /// <summary>
    /// Handles location functionalities.
    /// </summary>
    public class LocationManager : ILocationManager
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ISCFRepository _scfRepository;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="countryRepository"></param>
        /// <param name="stateRepository"></param>
        /// <param name="scfRepository"></param>
        public LocationManager(ICountryRepository countryRepository, IStateRepository stateRepository, ISCFRepository scfRepository)
        {
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _scfRepository = scfRepository;
        }

        /// <summary>
        /// Gets the list of countries.
        /// </summary>
        /// <returns> List of Countries. </returns>
        public IList<Country> GetCountries()
        {
            return _countryRepository.GetAll().ToList();
        }

        /// <summary>
        /// Gets the list of states by countryId.
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns> List of states.</returns>
        public IList<State> GetStates(int countryId)
        {
            return _stateRepository.GetMany(x => x.CountryId == countryId).ToList();
        }

        /// <summary>
        /// Gets the list of SCFs by stateId.
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns> List of SCFs.</returns>
        public IList<SCF> GetScFsOfState(int stateId)
        {
            return _scfRepository.GetMany(x => x.StateId == stateId).ToList();
        }

        /// <summary>
        /// Gets all SCFs by countryId.
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns>List of SCFs.</returns>
        public IList<SCF> GetAllScFs(int countryId)
        {
            return _scfRepository.GetMany(s => ((countryId == 0) || (s.State.CountryId == countryId))).ToList();
        }

        /// <summary>
        /// Gets SCF by scfId
        /// </summary>
        /// <param name="scfId"></param>
        /// <returns> SCF </returns>
        public SCF GetScf(int scfId)
        {
            return _scfRepository.GetById(scfId);
        }
    }
}
