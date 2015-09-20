using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Utils.Extensions
{
    public static class XmlExtensions
    {
        /// <summary>
        /// Escape raw text for use in an XML document. Note: This is different than HttpUtility.HtmlEncode!
        /// </summary>
        /// <param name="unEncodedText"></param>
        /// <returns></returns>
        public static string XmlEncode(this string unEncodedText)
        {
            var node = new XmlDocument().CreateElement("x");
            node.InnerText = unEncodedText;
            return node.InnerXml;
        }

        /// <summary>
        /// Serialize the object as XML
        /// </summary>
        /// <typeparam name="T">The type that will be serialized</typeparam>
        /// <param name="thing">The object that will be serialized</param>
        /// <param name="prettyPrint">Format the generated XML nicely.</param>
        /// <returns></returns>
        public static string SerializeAsXml<T>(this T thing, bool prettyPrint) where T : class, new()
        {
            var serializer = new XmlSerializer(typeof (T));
            using (var stringWriter = new StringWriter())
            {
                using (var xmlTextWriter = new XmlTextWriter(stringWriter))
                {
                    if (prettyPrint)
                        xmlTextWriter.Formatting = Formatting.Indented;

                    serializer.Serialize(xmlTextWriter, thing);
                    return stringWriter.ToString();
                }
            }
        }

        /// <summary>
        /// Serialize the object as XML. Will not pretty print the output.
        /// </summary>
        public static string SerializeAsXml<T>(this T thing) where T : class, new()
        {
            return thing.SerializeAsXml(false);
        }

        /// <summary>
        /// Deserialize XML into an object
        /// </summary>
        /// <typeparam name="T">The type of the object represented in the XML</typeparam>
        /// <param name="xml">The XML</param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(this string xml) where T : class
        {
            using (var stringReader = new StringReader(xml))
            {
                var xmlSerializer = new XmlSerializer(typeof (T));
                var thing = xmlSerializer.Deserialize(stringReader) as T;
                return thing;
            }
        }
    }
}