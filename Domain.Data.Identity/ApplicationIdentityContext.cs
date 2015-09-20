using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using MongoDB.Driver;
using Common.Utils;
using Domain.Data.Identity.Aspnet;

namespace Domain.Data.Identity
{
    public class ApplicationIdentityContext : IDisposable
	{
        public static ApplicationIdentityContext Create()
        {
            var client = new MongoClient(ConfigurationManager.AppSettings[Constants.MongoDbAppSettings.GlobalDbMongoDbServer] +
                "/" + ConfigurationManager.AppSettings[Constants.MongoDbAppSettings.GlobalDbMongoDatabaseName]);
            var database = client.GetDatabase(ConfigurationManager.AppSettings[Constants.MongoDbAppSettings.GlobalDbMongoDatabaseName]);
            var users = database.GetCollection<ApplicationUser>(Constants.MongoDbSettings.CollectionNames.Users);
            var roles = database.GetCollection<IdentityRole>(Constants.MongoDbSettings.CollectionNames.Roles);
            return new ApplicationIdentityContext(users, roles);
        }

        private ApplicationIdentityContext(IMongoCollection<ApplicationUser> users, IMongoCollection<IdentityRole> roles)
        {
            Users = users;
            Roles = roles;
        }

        public IMongoCollection<IdentityRole> Roles { get; set; }

        public IMongoCollection<ApplicationUser> Users { get; set; }

        public Task<List<IdentityRole>> AllRolesAsync()
        {
            return Roles.Find(r => true).ToListAsync();
        }

        ~ApplicationIdentityContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            Roles = null;
            Users = null;
        }
	}
}