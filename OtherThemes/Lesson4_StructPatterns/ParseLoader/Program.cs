using DataToParse;

using System;
using System.Collections.Generic;
using System.Globalization;
using DataToParse.Serializers;

namespace ParseLoader
{
    internal class Program
    {
        private static List<SearchDataHelper> SearchDataHelpers { get; } = new()
        {
            new()
            {
                DataName = "Sofa",
                Properties = new List<XMlProperty>()
                {
                    new(nameof(Data.Name), "Name", typeof(string)),
                    new(nameof(Data.Width), "Size.Width", typeof(double)),
                    new(nameof(Data.Height), "Size.Height", typeof(double)),
                    new(nameof(Data.Price), "Price", typeof(decimal))
                }
            }
        };

        private static Dictionary<Type, Func<string, object>> Converters { get; } = new()
        {
            [typeof(string)] = s => s,
            [typeof(double)] = s => double.TryParse(s, NumberStyles.AllowDecimalPoint, new NumberFormatInfo(), out var result) ? result : default(object),
            [typeof(decimal)] = s => decimal.TryParse(s, NumberStyles.AllowDecimalPoint, new NumberFormatInfo(), out var result) ? result : default(object),
        };

        private static readonly List<(Predicate<string>, ParseStrategy)> Parsers = new()
        {
            (s => (s.StartsWith('[') && s.EndsWith(']')) || (s.StartsWith('{') && s.EndsWith('}')), new JsonParseStrategy()),
            (s => s.StartsWith("<?xml") && s.EndsWith(">"), new XMLParseStrategy(SearchDataHelpers, Converters)),
        };

        private static void Main(string[] args)
        {
            ParsedDataGenerator generator = new();
            DataParser parser = new DataParser();
            ParseStrategySelector strategySelector = new ParseStrategySelector(Parsers);
            DataStorage storage = new DataStorage();

            ISerializer serializer = new JsonSerializator();

            var chairs = generator.GetChairs(serializer, 2);
            var parsedData = parser.Parse(chairs, strategySelector.SelectParseStrategy(chairs));
            storage.AddRange(parsedData);

            serializer = new XMLSerializator();

            var sofas = generator.GetSofas(serializer, 5);
            parsedData = parser.Parse(sofas, strategySelector.SelectParseStrategy(sofas));
            storage.AddRange(parsedData);

            foreach (var data in storage.GetData())
                Console.WriteLine(data);
        }
    }
}
