using System;

namespace Marketplace.Admin.Data.Infrastructure
{
    /// <summary>
    /// Inherit IDisposible to dispose off custom object.
    /// </summary>
    public class Disposable : IDisposable
    {
        private bool isDisposed;

        ~Disposable()
        {
            Dispose(false);
        }

        /// <summary>
        /// Sets disposed = true and calls Garbage collector.
        /// /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                DisposeCore();
            }

            isDisposed = true;
        }

        /// <summary>
        /// Override this to dispose custom objects
        /// </summary>
        protected virtual void DisposeCore()
        {
        }
    }
}
