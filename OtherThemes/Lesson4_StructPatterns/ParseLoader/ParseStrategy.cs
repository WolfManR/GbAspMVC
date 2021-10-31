using System.Collections.Generic;

namespace ParseLoader
{
    abstract class ParseStrategy
    {
        public abstract IEnumerable<Data> Parse(string data);
    }
}