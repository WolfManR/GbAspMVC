using System;
using System.Collections.Generic;

namespace ParseLoader
{
    internal class ParseStrategySelector
    {
        private readonly List<(Predicate<string> CanHandle, ParseStrategy Parser)> _parsers;

        public ParseStrategySelector(List<(Predicate<string> CanHandle, ParseStrategy Parser)> parsers)
        {
            _parsers = parsers;
        }

        public ParseStrategy SelectParseStrategy(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            var dataToParse = data.Trim();

            foreach (var (canHandle, parser) in _parsers)
            {
                if (canHandle(dataToParse)) return parser;
            }

            throw new NotSupportedException("Can't figure out what type of data");
        }
    }
}