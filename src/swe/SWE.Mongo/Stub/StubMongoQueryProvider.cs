using MongoDB.Driver;
using NieuweStroom.Infrastructure.Data.Mongo.Containers;
using NieuweStroom.Infrastructure.Data.Mongo.Interfaces;
using SWE.Mongo.Interfaces;

namespace NieuweStroom.Infrastructure.Data.Mongo.Stub
{
    //.AddSingleton<IQueryProvider<IMongoConditionContainer<T>, T>>(x => x.GetRequiredService<IBaseMongoConnector<T>>())

    public class StubMongoQueryProvider<T>
        : IMongoQueryProvider<T>
        where T : class
    {
        protected IMongoCollection<T> Source { get; }

        public StubMongoQueryProvider(IQueryable<T> source)
        {
            Source = GetCollection(source);
        }

        public StubMongoQueryProvider(IEnumerable<T> source)
           : this(source?.AsQueryable())
        { }

        public IEnumerable<T> Query(IMongoConditionContainer<T> request)
        {
            IEnumerable<FilterDefinition<T>> filters;

            FilterDefinition<T> combinedFilter = null;

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

            var query = Source
                .Find(combinedFilter)
                .Project(request.Projection);

            if (request.Skip > 0)
            {
                query = query
                    .Skip(request.Skip);
            }
            return
                //query?.ToList() ?? 
                Enumerable.Empty<T>();
        }

        public Task<IEnumerable<T>> Get(
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Query(new MongoConditionContainer<T>()));
        }

        public Task<IEnumerable<T>> Get(
            IMongoConditionContainer<T> request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Query(request));
        }

        public T QuerySingle(IMongoConditionContainer<T> request)
        {
            return Query(request)
                .ToList()
                .Single();
        }

        public Task<T> GetSingle(
            IMongoConditionContainer<T> request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(QuerySingle(request));
        }

        public T? QuerySingleOrDefault(IMongoConditionContainer<T> request)
        {
            return Query(request)
                .ToList()
                .GetSingleOrDefaultResult();
        }

        public Task<T?> GetSingleOrDefault(
            IMongoConditionContainer<T> request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(QuerySingleOrDefault(request));
        }

        public long QueryCount(IMongoConditionContainer<T> request)
        {
            return Query(request).Count();
        }

        public Task<long> GetCount(
            IMongoConditionContainer<T> request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(QueryCount(request));
        }

        public bool QueryExists(IMongoConditionContainer<T> request)
        {
            return Query(request)
                .Take(1)
                .Any();
        }

        public Task<bool> GetExists(
            IMongoConditionContainer<T> request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(QueryExists(request));
        }

        private IMongoCollection<T> GetCollection<T>(
            IQueryable<T> source)
        {
            var mockMongoCollection = new Mock<IMongoCollection<T>>();

            //Mock IAsyncCursor
            var mockCursor = new Mock<IAsyncCursor<T>>();
            mockCursor.Setup(x => x.Current).Returns(source.ToList());
            mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true);

            //Mock Aggregate
            mockMongoCollection
                .Setup(x => x.Aggregate<T>(It.IsAny<PipelineDefinition<T, T>>(), null, It.IsAny<CancellationToken>()))
                .Returns(mockCursor.Object);

            return mockMongoCollection.Object;
        }
    }
}