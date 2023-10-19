using SWE.Infrastructure.Abstractions.Interfaces.Contracts;

namespace SWE.Tests.Base
{
    public abstract class BaseContextFactoryTests<TContext>
        : BaseDiTests
    {
        private IFactory<TContext>? factory;

        protected IFactory<TContext> Factory => this.factory ??= CreateContextFactory();

        protected abstract IFactory<TContext> CreateContextFactory();
    }
}