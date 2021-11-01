using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace ParseLoader
{
    class XMLParseStrategy : ParseStrategy
    {
        public override IEnumerable<Data> Parse(string data)
        {
            using var reader = new StringReader(data);
            XmlDocument serializer = new XmlDocument();
            serializer.Load(reader);

            if (!serializer.HasChildNodes || serializer.ChildNodes.Count != 2) return Array.Empty<Data>();
            var nodes = serializer.ChildNodes[1].ChildNodes;

            var result = new List<Data>();

            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var properties = node.ChildNodes;
                Data readData = new Data();
                foreach (XmlNode property in properties)
                {
                    if (property.Name == nameof(Data.Name))
                    {
                        readData.Name = property.InnerText;
                    }

                    if (property.Name == nameof(Data.Width) && double.TryParse(property.InnerText, NumberStyles.AllowDecimalPoint, new NumberFormatInfo(), out var width))
                    {
                        readData.Width = width;
                    }

                    if (property.Name == nameof(Data.Height) && double.TryParse(property.InnerText, NumberStyles.AllowDecimalPoint, new NumberFormatInfo(), out var height))
                    {
                        readData.Height = height;
                    }

                    if (property.Name == nameof(Data.Price) && decimal.TryParse(property.InnerText, NumberStyles.AllowDecimalPoint, new NumberFormatInfo(), out var price))
                    {
                        readData.Price = price;
                    }
                }
                result.Add(readData);
            }

            return result;
        }
    }
}