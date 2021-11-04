using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using ParseLoader.DataModels;
using ParseLoader.Models;

namespace ParseLoader.ParseStategies
{
    internal class XMLParseStrategy : ParseStrategy
    {
        public XMLParseStrategy(List<SearchDataHelper> searchDataHelpers, Dictionary<Type, Func<string, object>> converters)
        {
            SearchDataHelpers = searchDataHelpers;
            Converters = converters;
        }
        private List<SearchDataHelper> SearchDataHelpers { get; }
        private Dictionary<Type, Func<string, object>> Converters { get; }

        public override IEnumerable<Data> Parse(string data)
        {
            using var reader = new StringReader(data);

            using var xmlReader = XmlReader.Create(reader);

            var helper = FindHelper(xmlReader);
            if (helper is null) yield break;

            do
            {
                XElement xElement = (XElement)XNode.ReadFrom(xmlReader);
                Dictionary<string, object> propertiesValues = new Dictionary<string, object>();
                foreach (var property in helper.Properties)
                {
                    var (success, value) = FindValue(xElement, property.PathParts);
                    if (!success) continue;
                    if (!Converters.TryGetValue(property.ValueType, out var converter)) continue;
                    propertiesValues.Add(property.AssignPropertyName, converter.Invoke(value));
                }

                Data temp = new Data()
                {
                    Name = propertiesValues[nameof(Data.Name)] as string,
                    Width = propertiesValues[nameof(Data.Width)] is double width ? width : default,
                    Height = propertiesValues[nameof(Data.Height)] is double height ? height : default,
                    Price = propertiesValues[nameof(Data.Price)] is decimal price ? price : default
                };

                yield return temp;
            } while (xmlReader.ReadToNextSibling(helper.DataName));

            xmlReader.ReadEndElement();
        }

        private static (bool, string) FindValue(XElement searchElement, string[] pathParts)
        {
            foreach (var path in pathParts)
            {
                if (searchElement.Element(path) is not { } element) return (false, null);
                searchElement = element;
            }
            return (true, searchElement.Value);
        }

        private SearchDataHelper FindHelper(XmlReader reader)
        {
            foreach (var helper in SearchDataHelpers)
            {
                if (reader.ReadToFollowing(helper.DataName))
                {
                    return helper;
                }
            }
            return null;
        }
    }
}