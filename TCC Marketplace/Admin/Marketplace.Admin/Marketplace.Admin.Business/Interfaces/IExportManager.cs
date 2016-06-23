using System.Collections.Generic;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    public interface IExportManager
    {
        IList<Frequency> GetFrequencies();
        ExtractFrequency GetExtractFrequency();
        void CreateExportFrequency(ExtractFrequency extractFrequency);
        void UpdateExportFrequency(ExtractFrequency extractFrequency);
        void SaveExportFrequency();
        Frequency GetFrequency(int frequencyId);
    }
}
