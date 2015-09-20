using Common.Utils;
using Domain.Data.Core.MongoDb;
using Domain.Data.Models.Languages;
using MongoDB.Driver;
using Repositories.Interfaces;

namespace Repositories.MongoDb
{
    public class LanguagesRepository : MongoRepository<Language>, ILanguagesRepository
    {
        protected override bool IsGlobal => true;

        protected override void EnsureIndexes(string tenantId, IMongoCollection<Language> col)
        {
            var roleName = Builders<Language>.IndexKeys.Ascending(t => t.IsoCode);
            var unique = new CreateIndexOptions { Unique = true };
            col.Indexes.CreateOneAsync(roleName, unique);

            var englishName = Builders<Language>.IndexKeys.Ascending(t => t.EnglishName);
            col.Indexes.CreateOneAsync(englishName);
        }

        protected override string CollectionName => Constants.MongoDbSettings.CollectionNames.Languages;
    }
}
