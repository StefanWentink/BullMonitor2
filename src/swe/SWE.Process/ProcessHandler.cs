using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SWE.Extensions.Extensions;
using SWE.Extensions.Interfaces;
using SWE.Infrastructure.Abstractions.Models;
using SWE.Process.Interfaces;
using SWE.Rabbit.Abstractions.Interfaces;
using SWE.Rabbit.Abstractions.Messages;
using SWE.Time.Extensions;
using SWE.Time.Utilities;

namespace SWE.Process
{
    public abstract class ProcessHandler
        : IHostedService
    {
        protected ICronProcessConfiguration CronProcessConfiguration { get; }
        protected CancellationToken MainProcessCancellationToken { get; private set; }
        protected IMessageSender<IssueModel> IssueModelSender { get; }
        protected IDateTimeOffsetNow DateTimeOffsetNow { get; }
        protected ILogger<ProcessHandler> Logger { get; }

        protected ProcessHandler(
            ICronProcessConfiguration cronProcessConfiguration,
            IMessageSender<IssueModel> issueModelSender,
            IDateTimeOffsetNow dateTimeOffsetNow,
            ILogger<ProcessHandler> logger)
        {
            CronProcessConfiguration = cronProcessConfiguration;
            IssueModelSender = issueModelSender;
            DateTimeOffsetNow = dateTimeOffsetNow;
            Logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"'{this.GetType().Name}' called {nameof(StartAsync)}.");

            MainProcessCancellationToken = cancellationToken;

            try
            {
                await ScheduleCronProcesses(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"'{this.GetType().Name}'': Exception encountered: {exception.Message}");

                await SendExceptionIssue(exception, cancellationToken)
                    .ConfigureAwait(false);

                throw;
            }

            await Task
                .CompletedTask
                .ConfigureAwait(false);
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"'{this.GetType().Name}' called {nameof(StopAsync)}.");

            await Task
                .CompletedTask
                .ConfigureAwait(false);
        }

        protected abstract Task ScheduleCronProcesses(CancellationToken cancellationToken);

        protected async Task ScheduleCronProcess<T>(
            IMessageSender<T> sender,
            CancellationToken cancellationToken)
            where T : class, new()
        {
            await ScheduleCronProcess<T>(
                    sender,
                    true,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task ScheduleCronProcess<T>(
            IMessageSender<T> sender,
            bool isFirst,
            CancellationToken cancellationToken)
            where T : class, new()
        {
            var commandName = typeof(T).Name;

            Logger.LogInformation($"'{this.GetType().Name}.{nameof(ScheduleCronProcess)}' {commandName}.");

            var cronConfigurations = CronProcessConfiguration
                .Cron
                .Where(x => x.Command.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (cronConfigurations.Count != 1)
            {
                var message = $"{nameof(ScheduleCronProcesses)}: Could not find one [{cronConfigurations.Count}] {nameof(cronConfigurations)} for {nameof(commandName)} '{commandName}'.";
                Logger.LogError(message);
                throw new ArgumentOutOfRangeException(message);
            }

            var cronConfiguration = cronConfigurations.Single();

            if (cronConfiguration.IsEnabled)
            {
                var cronExpression = CronExpression
                .Parse(cronConfiguration.CronExpression);

                var now = DateTimeOffset.Now.ToTimeZone(TimeZoneInfoUtilities.DutchTimeZoneInfo);

                var nextRunTime = cronExpression
                        .GetNextOccurrence(
                            now,
                            TimeZoneInfoUtilities.DutchTimeZoneInfo);

                if (nextRunTime.HasValue)
                {
                    var waitingTimeTillNextRun = nextRunTime
                        .Value
                        .ToTimeZone(TimeZoneInfoUtilities.DutchTimeZoneInfo) - now;

                    Logger.LogInformation($"{cronConfiguration.Command} {nameof(nextRunTime)} is {nextRunTime}.");

                    if (waitingTimeTillNextRun.TotalMilliseconds > 0)
                    {
                        var totalMilliseconds = waitingTimeTillNextRun.TotalMilliseconds;

#if DEBUG
                        if (isFirst)
                        {
                            totalMilliseconds = 1000;
                        }
#endif

                        if (totalMilliseconds > int.MaxValue)
                        {
                            await SetIntermediateTimer(sender, totalMilliseconds, cancellationToken)
                                .ConfigureAwait(false);
                        }
                        else if (totalMilliseconds <= int.MaxValue)
                        {
                            await SetHandlerTimer(sender, totalMilliseconds, cancellationToken)
                                .ConfigureAwait(false);
                        }
                    }
                }
                else
                {
                    var exception = new InvalidOperationException(
                        $"{nameof(ProcessHandler)}.{nameof(ScheduleCronProcess)}: {nameof(nextRunTime)} could not be determined.");

                    Logger
                        .LogError(exception, exception.Message);

                    await SendExceptionIssue(exception, cancellationToken)
                        .ConfigureAwait(false);

                    throw exception;
                }
            }
            else
            {
                Logger
                    .LogInformation($"Process {typeof(T).Name} was disabled in the configuration.");
            }
        }

        private Task SetHandlerTimer<T>(
            IMessageSender<T> sender,
            double totalMilliseconds,
            CancellationToken cancellationToken)
            where T : class, new()
        {
            var timer = new System.Timers.Timer(totalMilliseconds);

            timer.Elapsed += async (caller, args) =>
            {
                timer.Stop();
                timer.Dispose();

                if (!cancellationToken.IsCancellationRequested)
                {
                    await Send(
                            sender,
                            cancellationToken)
                        .ConfigureAwait(false);
                }
                else
                {
                    await ScheduleCronProcess(
                            sender,
                            false,
                            cancellationToken)
                        .ConfigureAwait(false);
                }
            };

            timer.Start();

            return Task.CompletedTask;
        }

        private async Task Send<T>(
            IMessageSender<T> sender,
            CancellationToken cancellationToken)
            where T : class, new()
        {
            try
            {
                await sender
                    .Send(
                        new T(),
                        cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Logger?
                    .LogError(
                        exception,
                        $"{nameof(sender)} for '{typeof(T).Name}' failed to send: {exception.Message}");
            }
            finally
            {
                try
                {
                    await ScheduleCronProcess(
                            sender,
                            false,
                            cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    Logger?
                        .LogError(
                            exception,
                            $"{nameof(sender)} for '{typeof(T).Name}' failed to reschedule: {exception.Message}");
                }
            }
        }

        private Task SetIntermediateTimer<T>(
            IMessageSender<T> sender,
            double totalMilliseconds,
            CancellationToken cancellationToken)
            where T : class, new()
        {
            var timer = totalMilliseconds > int.MaxValue
                ? new System.Timers.Timer(int.MaxValue)
                : new System.Timers.Timer(totalMilliseconds);

            timer.Elapsed += async (caller, args) =>
            {
                timer.Stop();
                timer.Dispose();
                if (totalMilliseconds > int.MaxValue)
                {
                    totalMilliseconds = -int.MaxValue;
                    await SetIntermediateTimer(sender, totalMilliseconds, cancellationToken)
                        .ConfigureAwait(false);
                }
                else if (totalMilliseconds <= int.MaxValue)
                {
                    await SetHandlerTimer(sender, totalMilliseconds, cancellationToken)
                        .ConfigureAwait(false);
                }
            };

            timer.Start();

            return Task.CompletedTask;
        }

        protected async Task SendIssue(
            IssueModel message,
            CancellationToken cancellationToken)
        {
            await IssueModelSender
                .Send(new RoutingMessage<IssueModel>(message.LogLevel.ToString(), message), cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task SendIssue(
            LogLevel logLevel,
            string description,
            CancellationToken cancellationToken)
        {
            var message = new IssueModel(
                GetType().Name,
                logLevel,
                description,
                DateTimeOffsetNow.Now);

            await SendIssue(message, cancellationToken)
                .ConfigureAwait(false);
        }

        protected async Task SendExceptionIssue(
            Exception exception,
            CancellationToken cancellationToken)
        {
            Logger.LogError(exception, exception.Message);

            var message = new IssueModel(
                GetType().Name,
                LogLevel.Error,
                exception.Message,
                DateTimeOffsetNow.Now);

            await SendIssue(message, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}