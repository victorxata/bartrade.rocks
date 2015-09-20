using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Domain.Data.Core.MongoDb;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Data.Models.Languages
{
    [DataContract]
    public class Country : Entity
    {
        [DataMember]
        public int LCID { get; set; }

        [DataMember]
        public ICollection<LanguageTranslation> Translations { get; set; }

        [BsonIgnore]
        public RegionInfo Region
        {
            get { return new RegionInfo(LCID); }
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Iso31662 { get; set; }

        [DataMember]
        public string Iso31663 { get; set; }

        [DataMember]
        public string NativeName { get; set; }

        [DataMember]
        public string EnglishName { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string CurrencyEnglishName { get; set; }

        [DataMember]
        public string CurrencyNativeName { get; set; }

        [DataMember]
        public string CurrencySymbol { get; set; }

        [DataMember]
        public string Iso4217 { get; set; }

    }
}