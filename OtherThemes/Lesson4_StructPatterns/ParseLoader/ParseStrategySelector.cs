using System;

namespace ParseLoader
{
    class ParseStrategySelector
    {
        public ParseStrategy SelectParseStrategy(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            var span = data.Trim();

            if((span.StartsWith('[') && span.EndsWith(']')) || (span.StartsWith('{') && span.EndsWith('}')))
                return new JsonParseStrategy();

            if(span.StartsWith("<?xml") && span.EndsWith(">"))
                return new XMLParseStrategy();

            throw new NotSupportedException("Can't figure out what type of data");
        }
    }
}