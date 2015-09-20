using Common.Utils;
using Domain.Data.Core.MongoDb;
using Domain.Data.Identity;
using MongoDB.Driver;
using Repositories.Interfaces;

namespace Repositories.MongoDb
{
    public class RecoverPasswordRepository : MongoRepository<RecoverPassword>, IRecoverPasswordRepository
    {
        protected override bool IsGlobal => true;

        protected override void EnsureIndexes(string tenantId, IMongoCollection<RecoverPassword> col)
        {
            var cName = Builders<RecoverPassword>.IndexKeys.Ascending(t => t.Token);
            var unique = new CreateIndexOptions { Unique = true };
            col.Indexes.CreateOneAsync(cName, unique);
        }

        protected override string CollectionName => Constants.MongoDbSettings.CollectionNames.RecoverPasswords;
    }
}
