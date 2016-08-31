
using Microsoft.WindowsAzure.StorageClient;

namespace Marketplace.Admin.Utils
{
    /// <summary>
    /// handles Blob Extension methods.
    /// </summary>
    public static class BlobExtensions
    {
        /// <summary>
        /// Checks if the blob exist.
        /// </summary>
        public static bool Exists(this CloudBlockBlob blob)
        {
            try
            {
                blob.FetchAttributes();
                return true;
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        
    }
}