using System.Collections.Generic;
using Marketplace.Admin.Model;

namespace Marketplace.Admin.Business
{
    /// <summary>
    /// Export manager interface.
    /// </summary>
    public interface IExportManager
    {
        /// <summary>
        /// Gets all frequency settings from repository.
        /// </summary>
        IList<Frequency> GetFrequencies();

        /// <summary>
        /// Gets the first frequency setting from repository.
        /// </summary>
        ExtractFrequency GetExtractFrequency();

        /// <summary>
        /// Save extract job frequency setting.
        /// </summary>
        void CreateExportFrequency(ExtractFrequency extractFrequency);

        /// <summary>
        /// Change extract job frequency setting.
        /// </summary>
        void UpdateExportFrequency(ExtractFrequency extractFrequency);

        /// <summary>
        /// Commit data base changes.
        /// </summary>
        void SaveExportFrequency();

        /// <summary>
        /// Get selected frequency details by Id.
        /// </summary>
        Frequency GetFrequency(int frequencyId);
    }
}
