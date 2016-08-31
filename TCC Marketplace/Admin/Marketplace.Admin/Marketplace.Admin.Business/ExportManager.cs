using System.Collections.Generic;
using System.Linq;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    /// <summary>
    /// Export functionality handlers.
    /// </summary>
    public class ExportManager : IExportManager
    {
        private readonly IFrequencyRepository _frequencyRepository;
        private readonly IExtractFrequencyRepository _extractFrequencyRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="frequencyRepository"></param>
        /// <param name="extractFrequencyRepository"></param>
        /// <param name="unitOfWork"></param>
        public ExportManager(IFrequencyRepository frequencyRepository, IExtractFrequencyRepository extractFrequencyRepository, IUnitOfWork unitOfWork)
        {
            _frequencyRepository = frequencyRepository;
            _extractFrequencyRepository = extractFrequencyRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the first frequency setting from repository.
        /// </summary>
        /// <returns> First frequency setting.</returns>
        public ExtractFrequency GetExtractFrequency()
        {
            return _extractFrequencyRepository.GetAll().FirstOrDefault();
        }

        /// <summary>
        /// Gets all frequency settings from repository.
        /// </summary>
        /// <returns>List of frequency setting. </returns>
        public IList<Frequency> GetFrequencies()
        {
            return _frequencyRepository.GetAll().ToList();
        }

        /// <summary>
        /// Commit data base changes.
        /// </summary>
        public void SaveExportFrequency()
        {
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Save extract job frequency setting.
        /// </summary>
        /// <param name="extractFrequency">ExtractFrequency</param>
        public void CreateExportFrequency(ExtractFrequency extractFrequency)
        {
            _extractFrequencyRepository.Add(extractFrequency);
        }

        /// <summary>
        /// Change extract job frequency setting.
        /// </summary>
        /// <param name="extractFrequency">ExtractFrequency </param>
        public void UpdateExportFrequency(ExtractFrequency extractFrequency)
        {
            _extractFrequencyRepository.Update(extractFrequency);
        }

        /// <summary>
        /// Get selected frequency details by Id.
        /// </summary>
        /// <param name="frequencyId"></param>
        /// <returns> Frequency details.</returns>
        public Frequency GetFrequency(int frequencyId)
        {
            return _frequencyRepository.GetById(frequencyId);
        }
    }
}
