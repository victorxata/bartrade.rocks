using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Domain.Data.Core.MongoDb;

namespace Domain.Data.Identity
{
    [DataContract]
    public class PreRegisteredUser : Entity
    {
        /// <summary>
        /// The user name pre-registered
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// The email used in the pre-registration step
        /// </summary>
        [DataMember]
        //[Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(255, ErrorMessage = "The Email Address is too long")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The language of the user
        /// </summary>
        [DataMember]
        public string Language { get; set; }

        /// <summary>
        /// This is a token for access this object again from the register page, and invite tenant page. AND SHOULD NOT BE THE TOKEN USED TO AUTHENTICATE A USER
        /// </summary>
        public string Token { get; set; }
        
        public DateTime ValidUntil { get; set; }
        
        public bool Activated { get; set; }

        [DataMember]
        public string TenantId { get; set; }

    }

}
