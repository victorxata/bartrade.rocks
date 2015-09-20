using System.Runtime.Serialization;

namespace Domain.Data.Models.Languages
{
    [DataContract]
    public class LanguageTranslation
    {
        [DataMember]
        public string IsoCode { get; set; }

        [DataMember]
        public string TranslationText { get; set; }

    }
}
