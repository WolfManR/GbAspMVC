using System.Collections.Generic;
using ParseLoader.DataModels;
using ParseLoader.ParseStategies;

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