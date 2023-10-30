using BullMonitor.Data.Sql.Contexts;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Updaters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace BullMonitor.Ticker.Core.Updaters
{
    public record CompanyUpdater(
            IFactory<TickerContext> ContextFactory,
            ILogger<CompanyUpdater> Logger)
        : ICompanyUpdater
    {
        public async Task<Guid> SetKnownByZacks(
            string code,
            bool value,
            CancellationToken cancellationToken)
        {
            using var context = ContextFactory.Create();

            var ticker = await context
                .Tickers
                .SingleOrDefaultAsync(b => b.Code.Equals(code), cancellationToken)
                .ConfigureAwait(false);

            try
            {
                if (ticker?.SetKnownByZacks(value) == true)
                {
                    await context
                        .SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);

                    return ticker.Id;
                }

                return Guid.Empty;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
                throw;
            }
        }

        public async Task<Guid> SetKnownByTipRanks(
            string code,
            bool value,
            CancellationToken cancellationToken)
        {
            using var context = ContextFactory.Create();

            var ticker = await context
                .Tickers
                .SingleOrDefaultAsync(b => b.Code.Equals(code), cancellationToken)
                .ConfigureAwait(false);

            try
            {
                if (ticker?.SetKnownByTipRanks(value) == true)
                {
                    await context
                        .SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);

                    return ticker.Id;
                }

                return Guid.Empty;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
                throw;
            }
        }
    }
}