﻿using Microsoft.EntityFrameworkCore;
using SWE.Infrastructure.Abstractions.Interfaces.Contracts;
using SWE.Infrastructure.Sql.Interfaces.Handlers;
using SWE.Infrastructure.Sql.Interfaces.Validators;

namespace SWE.Infrastructure.Sql.Contracts
{
    public class SqlValidateHandleUpdater<TContext, T>
        : SqlValidateCollectionHandleCollectionUpdater<TContext, T>
        where TContext : DbContext
        where T : class
    {
        public SqlValidateHandleUpdater(
            IFactory<TContext> contextFactory,
            IUpdateValidator<T> validator,
            IUpdatedHandler<T> handler)
            :base(
                contextFactory,
                new CollectionToSingleValidator<T>(validator),
                new CollectionToSingleHandler<T>(handler))
        { }
    }
}