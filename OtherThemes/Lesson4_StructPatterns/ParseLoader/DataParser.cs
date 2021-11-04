using System.Collections.Generic;

namespace ParseLoader
{
    internal class DataParser
    {
        public IEnumerable<Data> Parse(string data, ParseStrategy parseStrategy)
        {
            return parseStrategy.Parse(data);
        }
    }
}