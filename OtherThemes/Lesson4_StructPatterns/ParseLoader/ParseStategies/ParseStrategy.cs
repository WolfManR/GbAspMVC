using System.Collections.Generic;
using ParseLoader.DataModels;

namespace ParseLoader.ParseStategies
{
    internal abstract class ParseStrategy
    {
        public abstract IEnumerable<Data> Parse(string data);
    }
}