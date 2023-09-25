using MongoDB.Bson;
using MongoDB.Driver;

namespace SWE.Mongo.Extensions
{
    internal static class MongoDatabaseExtensions
    {
        internal static async Task<bool> CollectionExistsAsync(
            this IMongoDatabase database,
            string collectionName,
            CancellationToken cancellationToken)
        {
            var filter = new BsonDocument("name", collectionName);
            //filter by collection name
            var collections = await database
                .ListCollectionsAsync(new ListCollectionsOptions { Filter = filter }, cancellationToken)
                .ConfigureAwait(false);

            //check for existence
            return await collections
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        internal static bool CollectionExists(
            this IMongoDatabase database,
            string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            //filter by collection name
            var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter });
            //check for existence
            return collections.Any();
        }
    }
}