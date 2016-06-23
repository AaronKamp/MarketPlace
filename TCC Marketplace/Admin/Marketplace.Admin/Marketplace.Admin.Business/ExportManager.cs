using System.Collections.Generic;
using System.Linq;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    public class ExportManager : IExportManager
    {
        private readonly IFrequencyRepository _frequencyRepository;
        private readonly IExtractFrequencyRepository _extractFrequencyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ExportManager(IFrequencyRepository frequencyRepository, IExtractFrequencyRepository extractFrequencyRepository, IUnitOfWork unitOfWork)
        {
            _frequencyRepository = frequencyRepository;
            _extractFrequencyRepository = extractFrequencyRepository;
            _unitOfWork = unitOfWork;
        }

        public ExtractFrequency GetExtractFrequency()
        {
            return _extractFrequencyRepository.GetAll().FirstOrDefault();
        }

        public IList<Frequency> GetFrequencies()
        {
            return _frequencyRepository.GetAll().ToList();
        }

        public void SaveExportFrequency()
        {
            _unitOfWork.Commit();
        }

        public void CreateExportFrequency(ExtractFrequency extractFrequency)
        {
            _extractFrequencyRepository.Add(extractFrequency);
        }

        public void UpdateExportFrequency(ExtractFrequency extractFrequency)
        {
            _extractFrequencyRepository.Update(extractFrequency);
        }

        public Frequency GetFrequency(int frequencyId)
        {
            return _frequencyRepository.GetById(frequencyId);
        }
    }
}
