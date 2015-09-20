using Domain.Data.Core.MongoDb;

namespace Domain.Data.Models.Email
{
    public class EmailTemplate : Entity
    {
        public string Name { get; set; }
        public string HtmlBody { get; set; }
        public string PlainText { get; set; }
        public string Language { get; set; }
    }
}
