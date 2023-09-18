using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Sql.Extensions;
using System.Reflection;

namespace SWE.Infrastructure.Sql.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyConfigurations<TContext>(
            this ModelBuilder modelBuilder,
            TContext context,
            IEnumerable<Type>? excludedTypes = null)
            where TContext: DbContext
        {
            IEnumerable<Type> typelist = excludedTypes ?? Enumerable.Empty<Type>();

            var applyConfigurationMethod = modelBuilder
                .GetApplyConfigurationMethod();

            foreach (var (entityType, configuration) in context
                .GetType()
                .Assembly
                .GetInterfaceTypes())
            {
                if (!typelist.Contains(entityType))
                {
                    applyConfigurationMethod
                        .MakeGenericMethod(entityType)
                        .Invoke(modelBuilder, new[] { configuration });
                }
            }
        }

        internal static IEnumerable<(Type entityType, object configuration)>
            GetInterfaceTypes(
            this Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Select(@type => (
                    @type,
                    @interface: Array.Find(@type.GetInterfaces(), @interface => @interface.Name.Equals(typeof(IEntityTypeConfiguration<>).Name, StringComparison.Ordinal))))
                .Where(interfaceType => interfaceType.@interface != null)
                .Select(
                    interfaceType => (
                    entityType: interfaceType.@interface.GetGenericArguments()[0],
                    configuration: Activator.CreateInstance(interfaceType.@type)));
        }

        internal static MethodInfo GetApplyConfigurationMethod(this ModelBuilder modelBuilder)
        {
            return modelBuilder
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Single(x =>
                    x.Name.Equals(nameof(ModelBuilder.ApplyConfiguration), StringComparison.OrdinalIgnoreCase)
                    && x.GetGenericArguments()[0].Name == "TEntity");
        }
    }
}