using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System.Text;

namespace SWE.Mongo.Mock
{
    //setup to test mongo
    public static class MongoMock
    {
        private static Mock<MongoServer> CreateMongoServer()
        {
            var serverSettings = new MongoServerSettings
            {
                Servers = new List<MongoServerAddress>
                {
                    new("MockServer")
                }
            };

            var server = new Mock<MongoServer>(MockBehavior.Strict, serverSettings);

            string message;
            server.Setup(s => s.Settings).Returns(serverSettings);
            server.Setup(s => s.IsDatabaseNameValid(It.IsAny<string>(), out message)).Returns(true);

            return server;
        }

        private static Mock<IMongoDatabase> CreateMongoDatabase(
            this MongoServer server)
        {
            var databaseSettings = new MongoDatabaseSettings
            {
                //GuidRepresentation = GuidRepresentation.Standard,
                ReadEncoding = new UTF8Encoding(),
                ReadPreference = new ReadPreference(ReadPreferenceMode.Primary),
                WriteConcern = new WriteConcern(),
                WriteEncoding = new UTF8Encoding(),
            };
            var database = new Mock<MongoDatabase>(MockBehavior.Loose, server, "MockDatabase", databaseSettings);

            string message;
            database.Setup(db => db.Server).Returns(server);
            database.Setup(db => db.Settings).Returns(databaseSettings);
            database.Setup(db => db.IsCollectionNameValid(It.IsAny<string>(), out message)).Returns(true);

            var databaseX = database.As<IMongoDatabase>();

            return databaseX;
        }

        private static Mock<IMongoCollection<T>> CreateMongoCollection<T>(
            this IMongoDatabase database,
            string collectionName)
        {
            var collectionSetting = new MongoCollectionSettings
            {
                //GuidRepresentation = GuidRepresentation.Standard,
                ReadEncoding = new UTF8Encoding(),
                ReadPreference = new ReadPreference(ReadPreferenceMode.Primary),
                WriteConcern = new WriteConcern(),
                WriteEncoding = new UTF8Encoding(),
            };

            var collectionNamespace = new CollectionNamespace("MockDatabase", collectionName);
            var collectionMock = new Mock<MongoCollection<T>>(MockBehavior.Strict, database, collectionName, collectionSetting);

            var collectionX = collectionMock.As<IMongoCollection<T>>();
            collectionX.Setup(x => x.Database).Returns(database);
            collectionX.Setup(x => x.Settings).Returns(collectionSetting);
            collectionX.Setup(x => x.CollectionNamespace).Returns(collectionNamespace);

            return collectionX;
        }

        private static Mock<IMongoCollection<T>> CreateMongoCollection<T>(
            string collectionName)
        {
            var server = CreateMongoServer().Object;
            var database = CreateMongoDatabase(server);

            return CreateMongoCollection<T>(database.Object, collectionName);
        }

        private static Mock<MongoCursor<T>> CreateMongoCursor<T>(
            this IMongoCollection<T> collection,
            IEnumerable<T>? items = null)
        {
            var cursorMock = new Mock<MongoCursor<T>>(MockBehavior.Strict, collection, null, null, null, null);

            if (items != null)
            {
                cursorMock.Setup(c => c.GetEnumerator()).Returns(items.GetEnumerator());
            }

            return cursorMock;
        }
    }
}