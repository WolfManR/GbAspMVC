using System.Collections.Generic;

namespace ParseLoader
{
    internal abstract class ParseStrategy
    {
        public abstract IEnumerable<Data> Parse(string data);
    }
}