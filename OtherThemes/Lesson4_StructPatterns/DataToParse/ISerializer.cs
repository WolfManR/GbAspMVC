using System.Collections.Generic;

namespace DataToParse
{
    public interface ISerializer
    {
        string Serialize<T>(List<T> data);
    }
}