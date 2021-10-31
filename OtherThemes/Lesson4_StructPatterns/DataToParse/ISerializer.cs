using System.Collections.Generic;

namespace DataToParse
{
    public interface ISerializer
    {
        string Serialize<T>(IEnumerable<T> data);
    }
}