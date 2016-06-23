using System.Collections.Generic;
using System.Linq;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    public class LocationManager : ILocationManager
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ISCFRepository _scfRepository;

        public LocationManager(ICountryRepository countryRepository, IStateRepository stateRepository, ISCFRepository scfRepository)
        {
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _scfRepository = scfRepository;
        }

        public IList<Country> GetCountries()
        {
            return _countryRepository.GetAll().ToList();
        }

        public IList<State> GetStates(int countryId)
        {
            return _stateRepository.GetMany(x => x.CountryId == countryId).ToList();
        }

        public IList<SCF> GetScFsOfState(int stateId)
        {
            return _scfRepository.GetMany(x => x.StateId == stateId).ToList();
        }

        public IList<SCF> GetAllScFs(int countryId)
        {
            return _scfRepository.GetMany(s => ((countryId == 0) || (s.State.CountryId == countryId))).ToList();
        }

        public SCF GetScf(int scfId)
        {
            return _scfRepository.GetById(scfId);
        }
    }
}
