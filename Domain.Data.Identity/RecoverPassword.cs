using System;
using Domain.Data.Core.MongoDb;

namespace Domain.Data.Identity
{
    public class RecoverPassword : Entity
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool Activated { get; set; }
    }
}
