using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Reporter.DbContexts;
using Reporter.Models;

namespace Reporter.Repositories
{
    public class Repository
    {
        private readonly DatabaseConnection _dbConnection;

        public Repository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> SaveImportDataToTempStorage<TImportModel, TDataModel>(
            IAsyncEnumerable<TImportModel> importData,
            string dbCollectionName)
            where TDataModel : IRewritable<TImportModel>, new()
        {
            var collectionOfAllData = _dbConnection.Set<TDataModel>(dbCollectionName);

            const byte bufferSize = 20;
            var buffer = Enumerable.Range(0, bufferSize).Select(_ => new TDataModel()).ToArray();

            await foreach (var rawRecord in importData)
            {
                for (var i = 0; i < bufferSize; i++)
                {
                    buffer[i].Rewrite(rawRecord);
                }
                await collectionOfAllData.InsertManyAsync(buffer);
            }

            return true;
        }

        public async Task<IReadOnlyCollection<TModel>> TakeDataForReport<TModel>(
            int startIndex,
            int count,
            string dbCollectionName)
        {
            var collectionOfAllData = _dbConnection.Set<TModel>(dbCollectionName);

            var projection = Builders<TModel>.Projection.Exclude("_id");
            return await collectionOfAllData
                .Find(FilterDefinition<TModel>.Empty)
                .Project<TModel>(projection)
                .Skip(startIndex)
                .Limit(count)
                .ToListAsync();
        }
    }
}