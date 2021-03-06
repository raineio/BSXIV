using BSXIV.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BSXIV
{
    public class DbContext
    {
        private LoggingUtils _logging;
        private static MongoClient _client;

        public DbContext(LoggingUtils logging)
        {
            _logging = logging;
            MongoConnection();
        }

        private void MongoConnection()
        {
            _client = new MongoClient(
                Environment.GetEnvironmentVariable("MONGODB")
                );
            var settings = 
                MongoClientSettings.FromConnectionString(
                    Environment.GetEnvironmentVariable("MONGODB")
                );
            settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(30);
                
            _logging.Log(LogSeverity.Info, "Started MongoDB connection successfully!");
        }

        public void Insert(string collection, BsonDocument insert)
        {
            var db = _client.GetDatabase("bsxiv");
            var dbCollection = db.GetCollection<BsonDocument>(collection);
            dbCollection.InsertOne(insert);
        }
        
        public List<BsonDocument> Find(string collection, BsonDocument search)
        {
            var db = _client.GetDatabase("bsxiv");
            var dbCollection = db.GetCollection<BsonDocument>(collection);
            return dbCollection.Find(search).ToList();
        }
        
        public BsonDocument? FindOne(string collection, BsonDocument search)
        {
            var db = _client.GetDatabase("bsxiv");
            var dbCollection = db.GetCollection<BsonDocument>(collection);
            return dbCollection.Find(search).FirstOrDefault();
        }
        
        public void Update(string collection, BsonDocument search, BsonDocument set)
        {
            var db = _client.GetDatabase("bsxiv");
            var dbCollection = db.GetCollection<BsonDocument>(collection);
            dbCollection.UpdateOne(search, set);
        }
        
        public void UpdateMany(string collection, BsonDocument search, BsonDocument set)
        {
            var db = _client.GetDatabase("bsxiv");
            var dbCollection = db.GetCollection<BsonDocument>(collection);
            dbCollection.UpdateMany(search, set);
        }
        
        public void Delete(string collection, BsonDocument search)
        {
            var db = _client.GetDatabase("bsxiv");
            var dbCollection = db.GetCollection<BsonDocument>(collection);
            dbCollection.DeleteOne(search);
        }
    }
}