using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Extensions;

namespace SWE.Infrastructure.Sql.Contexts
{
    public abstract class BaseDbContext
        : DbContext
    {
        private bool isDisposed;

        protected ILogger<BaseDbContext>? Logger { get; }

        protected BaseDbContext()
        { }

        protected BaseDbContext(
            ILogger<BaseDbContext>? logger)
        {
            Logger = logger;
        }

        protected BaseDbContext(
            DbContextOptions options)
            : base(options)
        { }

        protected BaseDbContext(
            DbContextOptions options,
            ILogger<BaseDbContext>? logger)
            : base(options)
        {
            Logger = logger;
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            return base.DisposeAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (this.isDisposed) return;

                if (disposing) // && !this.IsDisposed())
                {
                    // free managed resources
                    //Database?.GetDbConnection().CloseConnection(Logger);
                }

                this.isDisposed = true;
            }
            catch (ObjectDisposedException exception)
            {
                Logger?.LogWarning(exception.Message);
            }
            catch (Exception exception)
            {
                foreach (var innerException in exception.GetInnerExceptions())
                {
                    Logger?.LogError(innerException, innerException.Message);
                }

                throw exception.GetInnerMostException();
            }
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~BaseDbContext()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
    }
}