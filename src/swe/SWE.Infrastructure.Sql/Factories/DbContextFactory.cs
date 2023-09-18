using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using System.Collections.Concurrent;
using SWE.Infrastructure.Sql.Extensions;
using SWE.Extensions.Extensions;

namespace SWE.Infrastructure.Sql.Factories
{
    public abstract class DbContextFactory<TContext>
        : IDesignTimeDbContextFactory<TContext>
        , IFactory<TContext>
        where TContext : DbContext
    {
        private readonly IConfiguration _configuration;

        protected DbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static readonly ConcurrentDictionary<string, DbContextOptions<TContext>> optionsCache = new();

        protected DbContextOptions<TContext> Options => LoadOptions();

        protected virtual string ConnectionStringConfigurationKey { get; } = "DefaultConnection";

        private DbContextOptions<TContext> LoadOptions()
        {
            var builder = new DbContextOptionsBuilder<TContext>();
            var connectionString = _configuration.GetConnectionString(ConnectionStringConfigurationKey);
            connectionString ??= Environment.GetEnvironmentVariable("EFCORETOOLSDB");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new KeyNotFoundException(
                    $"{typeof(DbContextFactory<TContext>).Name}.{nameof(LoadOptions)}: No {nameof(connectionString)} found with key {ConnectionStringConfigurationKey}");
            }

            optionsCache
                .GetValueOrAdd(
                    connectionString,
                    () =>
                    {
                        return GetOptions(
                            builder,
                            connectionString);
                    });

            return GetOptions(
                builder,
                connectionString);
        }

        private DbContextOptions<TContext> GetOptions(
            DbContextOptionsBuilder<TContext> builder,
            string? connectionString)
        {
            return builder
                .ApplyConfiguration(
                    connectionString ?? string.Empty,
                    _configuration)
                .Options;
        }

        public abstract TContext CreateDbContext(string[] args);

        public TContext Create()
        {
            return CreateDbContext(Array.Empty<string>());
        }
    }
}