using System.Collections.Generic;
using System.Text.Json;
using ParseLoader.DataModels;

namespace ParseLoader.ParseStategies
{
    internal class JsonParseStrategy : ParseStrategy
    {
        public override IEnumerable<Data> Parse(string data)
        {
            var result = JsonSerializer.Deserialize<IEnumerable<Data>>(data);
            return result;
        }
    }
}