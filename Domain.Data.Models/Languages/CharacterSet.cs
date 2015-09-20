using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Domain.Data.Core.MongoDb;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Data.Models.Languages
{
    [DataContract]
    public class CharacterSet : Entity
    {
        [DataMember]
        public string Code { get; set; }

        [BsonIgnore]
        public EncodingInfo CharSet
        {
            get
            {
                return Encoding.GetEncodings().FirstOrDefault(x => x.Name == Code);
            }
        }
    }
}
