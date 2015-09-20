using Common.Utils;
using Domain.Data.Core.MongoDb;
using Domain.Data.Models.Languages;
using MongoDB.Driver;
using Repositories.Interfaces;

namespace Repositories.MongoDb
{
    public class CountriesRepository : MongoRepository<Country>, ICountriesRepository
    {
        protected override bool IsGlobal => true;

        protected override void EnsureIndexes(string tenantId, IMongoCollection<Country> col)
        {
            var cName = Builders<Country>.IndexKeys.Ascending(t => t.Name);
            var unique = new CreateIndexOptions { Unique = true };
            col.Indexes.CreateOneAsync(cName, unique);

            var englishName = Builders<Country>.IndexKeys.Ascending(t => t.EnglishName);
            col.Indexes.CreateOneAsync(englishName);
        }

        protected override string CollectionName => Constants.MongoDbSettings.CollectionNames.Countries;
    }
}
