using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Updaters;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using SWE.Extensions.Extensions;
using System.Reflection;

namespace BullMonitor.Ticker.Api
{
    public static class StaticProgram
    {
        public static string[] Hello(
            HttpContext context,
            IHostEnvironment environment,
            IConfiguration configuration)
        {
            var name = context
                .User?
                .Claims
                .FirstOrDefault(x => x.Type.Equals("Username"))
                ?.Value
                ?? "anonymous";

            var apiName = "BullMonitor.Ticker.Api";

            var assembly = typeof(StaticProgram)
                .Assembly;


            assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute));

            return new[]
            {
                    $"Hello '{name}' from '{ apiName }'.",
                    context?.User?.Identity?.IsAuthenticated == true
                        ? "You are authenticated."
                        : "You are not authenticated.",
                    $"Your {nameof(IHostEnvironment.ApplicationName)} is {environment.ApplicationName}.",
                    $"Your {nameof(IHostEnvironment.EnvironmentName)} is {environment.EnvironmentName}.",
                    $"SHA: { assembly.GetGitHashFromInformationalVersion() }.",
            };
        }

        public static Task<IEnumerable<CompanyListResponse>> Get(
            ICompanyProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .Get(cancellationToken);
        }

        public static Task<IEnumerable<CompanyListResponse>> GetKnownByAll(
            ICompanyProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .GetKnownByAll(cancellationToken);
        }

        public static Task<IEnumerable<CompanyListResponse>> GetKnownByZacks(
            ICompanyProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .GetKnownByZacks(cancellationToken);
        }

        public static Task<IEnumerable<CompanyListResponse>> GetKnownByTipRanks(
            ICompanyProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .GetKnownByZacks(cancellationToken);
        }

        public static Task<CompanyListResponse?> GetById(Guid id,
            ICompanyProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .GetSingleOrDefault(id, cancellationToken);
        }

        public static Task<CompanyListResponse?> GetByCode(
            string code,
            ICompanyProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .GetSingleOrDefault(code, cancellationToken);
        }

        public static Task<Guid> SetKnownByZacks(
            CompanySetKnownRequest value,
            ICompanyUpdater updater,
            CancellationToken cancellationToken)
        {
            return updater
                .SetKnownByZacks(value.Code, value.Known, cancellationToken);
        }

        public static Task<Guid> SetKnownByTipRanks(
            CompanySetKnownRequest value,
            ICompanyUpdater updater,
            CancellationToken cancellationToken)
        {
            return updater
                .SetKnownByTipRanks(value.Code, value.Known, cancellationToken);
        }
    }
}