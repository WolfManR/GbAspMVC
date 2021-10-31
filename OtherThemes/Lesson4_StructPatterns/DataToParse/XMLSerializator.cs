using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DataToParse
{
    public class XMLSerializator : ISerializer
    {
        public string Serialize<T>(IEnumerable<T> data)
        {
            StringBuilder sb = new();
            TextWriter writer = new StringWriter(sb);
            XmlSerializer ser = new(typeof(T));
            ser.Serialize(writer, data);
            
            return sb.ToString();
        }
    }
}