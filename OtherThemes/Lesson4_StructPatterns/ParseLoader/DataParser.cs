using System.Collections.Generic;

namespace ParseLoader
{
    class DataParser
    {
        public IEnumerable<Data> Parse(string data, ParseStrategy parseStrategy)
        {
            return parseStrategy.Parse(data);
        }
    }
}