using SWE.Infrastructure.Sql.Interfaces;
using SWE.Infrastructure.Sql.Models;
using Microsoft.Extensions.Logging;
using SWE.Infrastructure.Abstractions.Interfaces.Requests;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using Ardalis.GuardClauses;

namespace SWE.Infrastructure.Sql.Mappers
{
    public abstract class BaseSqlRequestConditionMapper<TCondition, T>
        : IMapper<TCondition, ISqlConditionContainer<T>>
        where TCondition : IBaseCollectionRequest
        where T : class
    {
        protected ILogger<BaseSqlRequestConditionMapper<TCondition, T>> Logger { get; }

        protected BaseSqlRequestConditionMapper(
            ILogger<BaseSqlRequestConditionMapper<TCondition, T>> logger)
        {
            Logger = logger;
        }

        public virtual Task<ISqlConditionContainer<T>> Map(
            TCondition value,
            CancellationToken cancellationToken)
        {
            var response = new SqlConditionContainer<T>();

            if (value.Skip.HasValue && value.Skip != 0)
            {
                Guard
                    .Against
                    .InvalidInput(
                        value.Skip,
                        $"{nameof(value)}.{nameof(value.Skip)}",
                        x => x > 0,
                        $"{ GetType().Name }.{ nameof(Map) }: { nameof(IBaseCollectionRequest.Skip) } should not be less than zero.");

                response.SetSkip(value.Skip.Value);
            }

            if (value.Take.HasValue && value.Take != 0)
            {
                Guard
                    .Against
                    .InvalidInput(
                           value.Take,
                        $"{nameof(value)}.{nameof(value.Take)}",
                           x => x > 0,
                           $"{ GetType().Name }.{ nameof(Map) }: { nameof(IBaseCollectionRequest.Take) } should not be less than zero.");

                response.SetTake(value.Take.Value);
            }

            return Task.FromResult<ISqlConditionContainer<T>>(response);
        }
    }
}