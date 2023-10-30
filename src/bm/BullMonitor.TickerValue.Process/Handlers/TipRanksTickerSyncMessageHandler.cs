using BullMonitor.Abstractions.Commands;
using BullMonitor.Abstractions.Requests;
using BullMonitor.Abstractions.Responses;
using BullMonitor.Data.Storage.Models;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Updaters;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Issue.Abstractions.Messages;
using SWE.Process.Handlers;
using SWE.Rabbit.Abstractions.Enumerations;
using SWE.Rabbit.Abstractions.Interfaces;

namespace BullMonitor.TickerValue.Process.Handlers
{
    public class TipRanksTickerSyncMessageHandler
        : BaseMessageHandler<TipRanksTickerSyncMessage>
    {
        protected ISingleProvider<TipRanksRequest, TipRanksResponse> TipRanksProvider { get; }
        protected IMapper<
            (TipRanksResponse response, DateTimeOffset referenceDate, bool targetDateOnly),
            IEnumerable<TipRanksEntity>> TipRanksMapper
        { get; }
        protected ICreator<TipRanksEntity> TipRanksCreator { get; }
        protected ICompanyUpdater CompanyUpdater { get; }

        public TipRanksTickerSyncMessageHandler(
            IMessageSender<IssueMessage> issueMessageSender,
            ISingleProvider<TipRanksRequest, TipRanksResponse> tipRanksProvider,
            ICompanyUpdater companyUpdater,
            IMapper<
                (TipRanksResponse response, DateTimeOffset referenceDate, bool targetDateOnly),
                IEnumerable<TipRanksEntity>> tipRanksMapper,
            ICreator<TipRanksEntity> tipRanksCreator,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<TipRanksTickerSyncMessageHandler> logger)
            : base(
                  issueMessageSender,
                  dateTimeOffsetNow,
                  logger)
        {
            TipRanksCreator = tipRanksCreator;
            TipRanksProvider = tipRanksProvider;
            TipRanksMapper = tipRanksMapper;
            CompanyUpdater = companyUpdater;
        }

        public async override Task<MessageHandlingResponse> Handle(
            TipRanksTickerSyncMessage value,
            CancellationToken cancellationToken)
        {
            var request = new TipRanksRequest(
                value.ReferenceDate,
                value.ReferenceDate,
                value.Code);

            try
            {
                await _lock
                    .WaitAsync(LockMilliSecondsTimeOut, cancellationToken)
                    .ConfigureAwait(false);

                var response = await TipRanksProvider
                    .GetSingleOrDefault(request, cancellationToken)
                    .ConfigureAwait(false);

                var known = response?.Found;
                var targetDateOnly = !value.UpdateKnown;

                if (response is not null && known == true)
                {
                    var records = await TipRanksMapper
                        .Map(
                            (response, value.ReferenceDate, targetDateOnly),
                            cancellationToken)
                        .ConfigureAwait(false);

                    var createTasks = records
                        .Select(record => TipRanksCreator.Create(record, cancellationToken));

                    var created = await Task
                        .WhenAll(createTasks)
                        .ConfigureAwait(false);

                    var entityMessage = $"{created.Length} {nameof(TipRanksValue)} for {nameof(TipRanksEntity.Ticker)} '{value.Code}' and {nameof(TipRanksEntity.ReferenceDate)} '{value.ReferenceDate}'";

                    var createMessage = string.IsNullOrWhiteSpace(created.FirstOrDefault()?.Ticker)
                        ? $"{entityMessage}' already existed."
                        : $"Created {entityMessage}'.";

                    Logger
                        .LogInformation($"{nameof(TipRanksTickerSyncMessageHandler)}: {createMessage}");
                }
                else
                {
                    Logger
                        .LogInformation($"{nameof(TipRanksTickerSyncMessageHandler)}: {value.Code}' was not known.");
                }

                if (known is null)
                {
                    Logger
                        .LogInformation($"{nameof(TipRanksTickerSyncMessageHandler)}: {value.Code}' returned no unambiguous answer of it being found");
                }
                else if (value.UpdateKnown)
                {
                    Logger
                        .LogInformation($"{nameof(TipRanksTickerSyncMessageHandler)}: updating {value.Code}' to known {known.Value}.");

                    await CompanyUpdater
                        .SetKnownByTipRanks(value.Code, known.Value, cancellationToken)
                        .ConfigureAwait(false);
                }

                return MessageHandlingResponse.Successful;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, exception.Message);
                return MessageHandlingResponse.Unsuccessful;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}