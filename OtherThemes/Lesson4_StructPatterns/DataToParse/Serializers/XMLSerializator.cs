using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DataToParse.Serializers
{
    public class XMLSerializator : ISerializer
    {
        public string Serialize<T>(List<T> data)
        {
            StringBuilder sb = new();
            TextWriter writer = new StringWriter(sb);
            XmlSerializer ser = new(data.GetType());
            ser.Serialize(writer, data);
            
            return sb.ToString();
        }
    }
}