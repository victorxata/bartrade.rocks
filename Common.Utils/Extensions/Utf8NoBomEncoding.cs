using System.Text;

namespace Common.Utils.Extensions
{
    /// <summary>
    /// UTF-8 with no BOM (Byte Order Mark)
    /// .NET does not have a way to indicate UTF-8 with no BOM. This is how Sajan deals with that problem.
    /// </summary>
    public class Utf8NoBomEncoding : UTF8Encoding
    {
        public const string Name = "utf-8n";

        public Utf8NoBomEncoding() : base(false)
        {
        }

        public override string BodyName
        {
            get { return Name; }
        }
    }
}