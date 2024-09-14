using BaseShare.Common.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BaseShare.Common.Repositories.Mongo
{
    public class MongoContext<T> where T : class
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;
        private readonly ILogger<MongoContext<T>> _logger;

        public MongoContext(IOptions<MongoDbSettings> settings,
                            ILogger<MongoContext<T>> logger)
        {
            _client = new MongoClient(settings.Value.ConnectionString);
            _database = _client.GetDatabase(settings.Value.DatabaseName);
            _logger = logger;
        }

        public IMongoCollection<T> GetCollection(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> GetAllAsync(string collectionName)
        {
            try
            {
                var collection = GetCollection(collectionName);
                return await collection.Find(Builders<T>.Filter.Empty).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve data: {ex.Message}");

                throw new ApplicationException($"Failed to retrieve data: {ex.Message}");
            }
        }

        public async Task<T> GetByIdAsync(string collectionName, string id)
        {
            try
            {
                var collection = GetCollection(collectionName);
                return await collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve data: {ex.Message}");

                throw new ApplicationException($"Failed to retrieve data: {ex.Message}");
            }
        }

        public async Task InsertAsync(string collectionName, T entity)
        {
            try
            {
                var collection = GetCollection(collectionName);
                await collection.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to insert data: {ex.Message}");

                throw new ApplicationException($"Failed to insert data: {ex.Message}");
            }
        }

        public async Task UpdateAsync(string collectionName, string id, T entity)
        {
            try
            {
                var collection = GetCollection(collectionName);
                await collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update data: {ex.Message}");

                throw new ApplicationException($"Failed to update data: {ex.Message}");
            }
        }

        public async Task DeleteAsync(string collectionName, string id)
        {
            try
            {
                var collection = GetCollection(collectionName);
                await collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete data: {ex.Message}");

                throw new ApplicationException($"Failed to delete data: {ex.Message}");
            }
        }
    }
}
