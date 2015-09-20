using Common.Utils;
using Domain.Data.Core.MongoDb;
using Domain.Data.Models.Languages;
using MongoDB.Driver;
using Repositories.Interfaces;

namespace Repositories.MongoDb
{
    public class CharacterSetsRepository : MongoRepository<CharacterSet>, ICharacterSetsRepository
    {
        protected override bool IsGlobal => true;

        protected override void EnsureIndexes(string tenantId, IMongoCollection<CharacterSet> col)
        {
            var csCode = Builders<CharacterSet>.IndexKeys.Ascending(t => t.Code);
            var unique = new CreateIndexOptions { Unique = true };
            col.Indexes.CreateOneAsync(csCode, unique);
        }
        protected override string CollectionName => Constants.MongoDbSettings.CollectionNames.CharacterSets;
    }
}
