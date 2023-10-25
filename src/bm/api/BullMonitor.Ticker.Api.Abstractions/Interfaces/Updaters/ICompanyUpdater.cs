namespace BullMonitor.Ticker.Api.Abstractions.Interfaces.Updaters
{
    public interface ICompanyUpdater
    {
        Task<Guid> SetKnownByZacks(
                string code,
                bool value,
                CancellationToken cancellationToken);

        Task<Guid> SetKnownByTipranks(
            string code,
            bool value,
            CancellationToken cancellationToken);
    }
}