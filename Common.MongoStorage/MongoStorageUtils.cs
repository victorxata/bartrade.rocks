using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Common.MongoStorage
{
    public static class MongoStorageUtils
    {
        private static string DatabaseName { get; set; }
        private static string TenantId { get; set; }

        private static IMongoDatabase MongoDataBase { get; set; }

        private static void Connect()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString);
            MongoDataBase = client.GetDatabase(ConfigurationManager.AppSettings["GlobalDatabase"]);

        }

        public static MongoGridFS GridFs()
        {
            var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString);
            MongoDataBase = client.GetDatabase(ConfigurationManager.AppSettings["GlobalDatabase"]);
            client.GetDatabase(DatabaseName);
            GetCredentialsFromConnectionString(
                ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString);
            var credentials = GetCredentialsFromConnectionString(
                    ConfigurationManager.ConnectionStrings["MongoServerSettings"].ConnectionString);

            var settings = new MongoServerSettings
            {
                Server = new MongoServerAddress(client.Settings.Server.Host, client.Settings.Server.Port),
                Credentials = new List<MongoCredential> { credentials }
            };

            var server = new MongoServer(settings);
            var gridfs = new MongoGridFS(server, DatabaseName, new MongoGridFSSettings());
            return gridfs;
        }

        private static MongoCredential GetCredentialsFromConnectionString(string cs)
        {
            var part1 = cs.Split('/')[1].Substring(1);
            var user = part1.Split(':')[0];
            var part2 = part1.Split(':')[1];
            var password = part2.Split('@')[0];

            return MongoCredential.CreateCredential(DatabaseName, user, password);
        }

        public static string UploadFile(string tenantId, FileInfo fileInfo)
        {
            if (!EnsureConnectionExists(tenantId))
                return null;

            using (var fs = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                var gridFsInfo = GridFs().Upload(fs, fileInfo.Name);

                var fileId = gridFsInfo.Id.ToString();
                return $"api/tenant/{tenantId}/project/{"ss"}/downloaddirectfile/{fileId}";
            }
        }

        public static string UploadFileId(string tenantId, FileInfo fileInfo)
        {
            if (!EnsureConnectionExists(tenantId))
                return null;

            using (var fs = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                var gridFsInfo = GridFs().Upload(fs, fileInfo.Name);

                return gridFsInfo.Id.ToString();
            }
        }

        public static string DownloadFile(string tenantId, string projectId, string fileId)
        {
            if (!EnsureConnectionExists(tenantId))
                return null;
            var mediaFolder = ConfigurationManager.AppSettings["MediaFolder"];
            var rootFolder = ConfigurationManager.AppSettings["RootFiles"];
            var path = Path.Combine(rootFolder, mediaFolder);
            var exists = Directory.Exists(path);

            if (!exists)
                Directory.CreateDirectory(path);

            var tempDir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            tempDir.Create();

            var id = new ObjectId(fileId);
            var fileExists = GridFs().ExistsById(id);

            if (!fileExists)
            {
                Console.WriteLine("MongoDbFileStorage:DonwloadFile:: => Error[File not found]");
                throw new Exception("File not found");
            }
            var file = GridFs().FindOneById(id);

            var filename = $"{Guid.NewGuid().ToString().Replace("-", "")}.{Path.GetExtension(file.Name)}";
            using (var stream = file.OpenRead())
            {
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                using (var newFs = new FileStream(Path.Combine(path, filename), FileMode.Create))
                {
                    newFs.Write(bytes, 0, bytes.Length);
                    return $"{ConfigurationManager.AppSettings["httpRootFiles"]}/{mediaFolder}/{filename}";
                }
            }
        }

        public static FileStream DownloadDirectFile(string tenantId, string projectId, string fileId) 
        {
            if (!EnsureConnectionExists(tenantId))
                return null;
            var mediaFolder = ConfigurationManager.AppSettings["MediaFolder"];
            var rootFolder = ConfigurationManager.AppSettings["RootFiles"];
            var path = Path.Combine(rootFolder, mediaFolder);
            var exists = Directory.Exists(path);

            if (!exists)
                Directory.CreateDirectory(path);

            var id = new ObjectId(fileId);
            var fileExists = GridFs().ExistsById(id);

            if (!fileExists)
            {
                Console.WriteLine("MongoDbFileStorage:DonwloadFile:: => Error[File not found]");
                throw new Exception("File not found");
            }
            var file = GridFs().FindOneById(id);

            var filename = $"{Guid.NewGuid().ToString().Replace("-", "")}{Path.GetExtension(file.Name)}";
            using (var stream = file.OpenRead())
            {
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                using (var newFs = new FileStream(Path.Combine(path,filename), FileMode.Create))
                {
                    newFs.Write(bytes, 0, bytes.Length);
                    return newFs;
                }
            }
        }

        private static bool EnsureConnectionExists(string tenantId)
        {
            if (TenantId != tenantId)
            {
                if (String.IsNullOrEmpty(tenantId))
                    return false;
                TenantId = tenantId;
                try
                {
                    Connect();
                    return true;
                }
                catch (Exception )
                {
                    return false;
                }
            }
            if (MongoDataBase != null) 
                return true;
            try
            {
                Connect();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
    }
}
