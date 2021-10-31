using System;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;

using DataToParse;

namespace ParseLoader
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            ParsedDataGenerator generator = new ();
            DataParser parser = new DataParser();
            ParseStrategySelector strategySelector = new ParseStrategySelector();
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
