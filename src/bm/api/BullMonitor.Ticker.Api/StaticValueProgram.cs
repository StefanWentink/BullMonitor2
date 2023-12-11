using BullMonitor.Ticker.Api.Abstractions.Interfaces.Providers;
using BullMonitor.Ticker.Api.Abstractions.Interfaces.Updaters;
using BullMonitor.Ticker.Api.Abstractions.Responses;
using SWE.Extensions.Extensions;
using System.Reflection;

namespace BullMonitor.Ticker.Api
{
    public static class StaticValueProgram
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

            var assembly = typeof(StaticValueProgram)
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

        public static Task<IEnumerable<CompanyResponse>> Get(
            IValueProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .Get(cancellationToken);
        }

        public static Task<IEnumerable<CompanyResponse>> GetKnownByAll(
            IValueProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .GetKnownByAll(cancellationToken);
        }

        public static Task<IEnumerable<CompanyResponse>> GetKnownByZacks(
            IValueProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .GetKnownByZacks(cancellationToken);
        }

        public static Task<IEnumerable<CompanyResponse>> GetKnownByTipRanks(
            IValueProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .GetKnownByZacks(cancellationToken);
        }

        public static Task<CompanyResponse?> GetById(Guid id,
            IValueProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .GetSingleOrDefault(id, cancellationToken);
        }

        public static Task<CompanyResponse?> GetByCode(
            string code,
            IValueProvider provider,
            CancellationToken cancellationToken)
        {
            return provider
                .GetSingleOrDefault(code, cancellationToken);
        }
    }
}