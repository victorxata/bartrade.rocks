namespace Common.Utils
{
    public static class Constants
    {
        public static class MongoDbAppSettings
        {
            public const string GlobalDbMongoConnectionStringName = "MongoServerSettings";
            public const string GlobalDbMongoDatabaseName = "GlobalDatabase";
            public const string GlobalDbMongoDbServer = "MongoServer";
        }

        public static class MongoDbSettings
        {
            public static class CollectionNames
            {
                public const string Users = "users";
                public const string Roles = "roles";
                public const string Countries = "Country";
                public const string CharacterSets = "CharacterSet";
                public const string EmailTemplates = "EmailTemplate";
                public const string Languages = "Language";
                public const string Preregistrations = "PreRegistration";
                public const string RecoverPasswords = "RecoverPassword";
            }

        }
        public static class Errors
        {
            public const string DuplicateKey = "Cannot insert current entity because it has an existing key in the collection";
            public const string EntryNotFound = "Entry not found";
            public const string CannotParseId = "Cannot parse given Id";
            
        }
    }
}