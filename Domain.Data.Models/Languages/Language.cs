using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using Domain.Data.Core.MongoDb;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Data.Models.Languages
{
    [DataContract]
    public class Language : Entity
    {
        [DataMember]
        public string IsoCode { get; set; }

        [DataMember]
        public string CustomCode { get; set; }

        [DataMember]
        public CharacterSet CharacterSet { get; set; }

        [DataMember]
        public ICollection<LanguageTranslation> Translations { get; set; }

        [DataMember]
        public string Parent { get; set; }

        [DataMember]
        public string Iso6391 { get; set; }

        [DataMember]
        public string Iso6392 { get; set; }

        [DataMember]
        public string Iso6393 { get; set; }

        [DataMember]
        public string Iso31661 { get; set; }

        [DataMember]
        public string NativeName { get; set; }

        [DataMember]
        public string EnglishName { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string LegacyLookupKey { get; set; }

        [BsonIgnore]
        public CultureInfo Culture
        {
            get { return CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(x => x.Name == IsoCode); }
        }
    }
}
