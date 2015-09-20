using Common.Utils;
using Domain.Data.Core.MongoDb;
using Domain.Data.Models.Email;
using MongoDB.Driver;
using Repositories.Interfaces;

namespace Repositories.MongoDb
{
    public class EmailTemplatesRepository : MongoRepository<EmailTemplate>, IEmailTemplatesRepository
    {
        protected override bool IsGlobal => true;

        protected override void EnsureIndexes(string tenantId, IMongoCollection<EmailTemplate> col)
        {
            var cName = Builders<EmailTemplate>.IndexKeys.Ascending(t => t.Name);
            var unique = new CreateIndexOptions { Unique = true };
            col.Indexes.CreateOneAsync(cName, unique);
        }

        protected override string CollectionName => Constants.MongoDbSettings.CollectionNames.EmailTemplates;
    }
}
