using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SWE.Mongo.Extensions;
using SWE.Mongo.Interfaces;
using SWE.Extensions.Extensions;

namespace SWE.Mongo
{
    public abstract class BaseMongoConnector<T>
        : BaseMongoConnector<IMongoConditionContainer<T>, T>
        , IBaseMongoConnector<T>
    {
        protected BaseMongoConnector(
            IConfiguration configuration,
            ILogger<BaseMongoConnector<T>> logger)
            : base(configuration, logger)
        { }
    }

    public abstract class BaseMongoConnector<TContainer, T>
        : BaseMongoConnector<TContainer, T, T>
        , IBaseMongoConnector<TContainer, T, T>
        where TContainer : IMongoConditionContainer<T, T>
    {
        protected BaseMongoConnector(
            IConfiguration configuration,
            ILogger<BaseMongoConnector<TContainer, T>> logger)
            : base(configuration, logger)
        { }
    }

    public abstract class BaseMongoConnector<TContainer, T, TProjection>
        : IBaseMongoConnector<TContainer, T, TProjection>
        where TContainer : IMongoConditionContainer<T, TProjection>
    {
        private string? _connectionString = null;
        private string? _tableName = null;
        private MongoClient? _client = null;
        private IMongoDatabase? _dataBase = null;
        private IMongoCollection<T>? _collection = null;

        protected IConfiguration Configuration { get; set; }
        protected ILogger<BaseMongoConnector<TContainer, T, TProjection>> Logger { get; set; }
        protected string ConnectionString => _connectionString ??= Configuration.GetMongoConnectionString();
        protected abstract string DatabaseName { get; }
        protected string TableName => _tableName ??= GetTableName();
        protected MongoClient Client => _client ??= new MongoClient(ConnectionString);
        protected IMongoDatabase DataBase => _dataBase ??= Client.GetDatabase(DatabaseName);
        protected IMongoCollection<T> Collection => _collection ??= GetOrCreateCollection();
        protected virtual string GetTableName()
        {
            return typeof(T).Name;
        }

        protected BaseMongoConnector(
            IConfiguration configuration,
            ILogger<BaseMongoConnector<TContainer, T, TProjection>> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        protected abstract CreateCollectionOptions<T> CreateCollectionOptions { get; }
        protected virtual IEnumerable<CreateIndexModel<T>> GetIndexes()
        {
            return new List<CreateIndexModel<T>>();
        }

        protected IMongoCollection<T> GetOrCreateCollection()
        {
            var mongoCollectionSettings = new MongoCollectionSettings()
            { };

            if (!DataBase.CollectionExists(TableName))
            {
                var indexes = GetIndexes().ToList();

                DataBase.CreateCollection(TableName, CreateCollectionOptions);
                var collection = DataBase.GetCollection<T>(TableName, mongoCollectionSettings);

                foreach (var index in indexes)
                {
                    collection
                        .Indexes
                        .CreateOne(index);
                }

                return collection;
            }

            return DataBase.GetCollection<T>(TableName, mongoCollectionSettings);
        }

        public async Task<IEnumerable<TProjection>> Get(
            TContainer request,
            CancellationToken cancellationToken)
        {
            return await Query(request)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<T> Create(
            T value,
            CancellationToken cancellationToken)
        {
            var responses = await Create(
                    value.Yield(),
                    cancellationToken)
                .ConfigureAwait(false);

            return responses
                .Single();
        }

        public async Task<IEnumerable<T>> Create(
            IEnumerable<T> values,
            CancellationToken cancellationToken)
        {
            if (!values.Any())
            {
                return values;
            }

            await Collection
                .InsertManyAsync(values, new InsertManyOptions { }, cancellationToken)
                .ConfigureAwait(false);

            return values;
        }

        public async Task<T> Update(
            T value,
            CancellationToken cancellationToken)
        {
            var responses = await Upsert(
                    value.Yield(),
                    cancellationToken)
                .ConfigureAwait(false);

            return responses
                .Single();
        }

        public async Task<IEnumerable<T>> Update(
            IEnumerable<T> value,
            CancellationToken cancellationToken)
        {
            return await Upsert(
                    value,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<T> Upsert(
            T value,
            CancellationToken cancellationToken)
        {
            var responses = await Upsert(
                    value.Yield(),
                    cancellationToken)
                .ConfigureAwait(false);

            return responses
                .Single();
        }

        public abstract Task<IEnumerable<T>> Upsert(
            IEnumerable<T> values,
            CancellationToken cancellationToken);

        public async Task<TProjection> GetSingle(
            TContainer request, CancellationToken cancellationToken)
        {
            return await Query(request)
                .SingleAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<TProjection?> GetSingleOrDefault(
            TContainer request,
            CancellationToken cancellationToken)
        {
            return await Query(request)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<long> GetCount(
            TContainer request,
            CancellationToken cancellationToken)
        {
            return await Query(request)
                .CountDocumentsAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> GetExists(
            TContainer request,
            CancellationToken cancellationToken)
        {
            return await Query(request)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public IFindFluent<T, TProjection> Query(
            TContainer request)
        {
            FilterDefinition<T>? combinedFilter = null;

            foreach (var filter in request.Filters)
            {
                combinedFilter = combinedFilter == null
                    ? filter
                    : combinedFilter & filter;
            }

            if (combinedFilter == null)
            {
                combinedFilter = FilterDefinition<T>.Empty;
            }

            var query = Collection
                .Find(combinedFilter)
                .Project(request.Projection);

            if (request.Skip > 0)
            {
                query = query
                    .Skip(request.Skip);
            }

            //if (request.Take > 0)
            //{
            //    query = query
            //        .Take(request.Take);
            //}

            //if (request.Projection != null)
            //{
            //    return query
            //        .Select(request.Projection)
            //        .Project(request.Projection);
            //}

            return query;
        }


        protected abstract FilterDefinition<T> GenerateUpdateFilter(T value);

        protected abstract UpdateDefinition<T> GenerateUpdateDefinition(T value);
    }
}